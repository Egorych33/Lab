#!/usr/bin/env python3
"""
Генератор параметризованных NUnit-тестов для IColorParser.
Адаптирован под реальные методы: HexToRgb, RgbToHex, GetColorName, NameToHex.
"""
import yaml
import argparse
from pathlib import Path
from typing import Dict, List, Any


TEST_FILE_TEMPLATE = """// ==============================================================
// AUTO-GENERATED TESTS. DO NOT EDIT MANUALLY.
// Source: {spec_source}
// Generator: gen_tests.py v2.0 (ColorParser)
// ==============================================================
using System;
using NUnit.Framework;
using Lab.Interfaces;

namespace Module.Tests
{{
    [TestFixture]
    [Description("Автоматически сгенерированные тесты для {module_name}")]
    public class {module_name}Tests
    {{
        private I{module_name} _sut;

        [SetUp]
        public void SetUp()
        {{
            _sut = new Lab.Implementations.GenCode1.ColorParser();
        }}

{test_methods}
    }}
}}
"""


TEST_METHOD_TEMPLATE = """
        [Test]
        [Description("Класс эквивалентности: {case_desc}")]
        public void Test_{method_name}_{case_name}()
        {{
            // === Arrange ===
            // Precondition: {pre}
            // Expected: {expected}

            // === Act & Assert ===
            {assert_code}
        }}
"""


def load_spec(spec_path: str) -> Dict[str, Any]:
    with open(spec_path, "r", encoding="utf-8") as f:
        return yaml.safe_load(f)


def format_csharp_input(value: Any) -> str:
    """Преобразует значение из YAML в C#-литерал."""
    if value is None:
        return "null"
    if isinstance(value, bool):
        return "true" if value else "false"
    if isinstance(value, str):
        # Экранируем обратные слэши и кавычки
        escaped = value.replace("\\", "\\\\").replace('"', '\\"')
        return f'"{escaped}"'
    return str(value)


def sanitize_name(name: str) -> str:
    """Превращает описание в валидный C#-идентификатор."""
    result = (name.replace(" ", "_")
                  .replace("(", "")
                  .replace(")", "")
                  .replace(",", "")
                  .replace("-", "_")
                  .replace("#", "Hex")
                  .replace(".", ""))
    # Убираем русские буквы — C# их не любит в именах
    cyrillic_map = {
        'А':'A','Б':'B','В':'V','Г':'G','Д':'D','Е':'E','Ж':'Zh','З':'Z',
        'И':'I','Й':'Y','К':'K','Л':'L','М':'M','Н':'N','О':'O','П':'P',
        'Р':'R','С':'S','Т':'T','У':'U','Ф':'F','Х':'H','Ц':'Ts','Ч':'Ch',
        'Ш':'Sh','Щ':'Sch','Ъ':'','Ы':'Y','Ь':'','Э':'E','Ю':'Yu','Я':'Ya',
        'а':'a','б':'b','в':'v','г':'g','д':'d','е':'e','ж':'zh','з':'z',
        'и':'i','й':'y','к':'k','л':'l','м':'m','н':'n','о':'o','п':'p',
        'р':'r','с':'s','т':'t','у':'u','ф':'f','х':'h','ц':'ts','ч':'ch',
        'ш':'sh','щ':'sch','ъ':'','ы':'y','ь':'','э':'e','ю':'yu','я':'ya',
    }
    result = "".join(cyrillic_map.get(ch, ch) for ch in result)
    return result


def generate_assert(method_name: str, inputs_str: str,
                    expected: str, signature: str) -> str:
    """Строит C#-код Act + Assert по expected."""
    expected = expected.strip()

    # --- Случай 1: ожидается исключение ---
    if "Exception" in expected:
        # Берём первое слово, содержащее "Exception"
        exc_type = "Exception"
        for token in expected.split():
            if "Exception" in token:
                exc_type = token.strip(".,;")
                break
        return (f"Assert.Throws<{exc_type}>("
                f"() => _sut.{method_name}({inputs_str}));")

    # --- Случай 2: метод void ---
    if signature.strip().startswith("void"):
        return f"_sut.{method_name}({inputs_str});"

    # --- Случай 3: метод возвращает значение ---
    # expected уже в C#-синтаксисе (например, "\"rgb(255, 0, 0)\"")
    return (f"var result = _sut.{method_name}({inputs_str});\n"
            f"            Assert.That(result, Is.EqualTo({expected}));")


def generate_method_tests(method_data: Dict[str, Any]) -> List[str]:
    blocks = []
    method_name = method_data["name"]
    signature = method_data.get("signature", "")
    pre = method_data.get("pre", "true")

    for eq_class in method_data.get("equivalence_classes", []):
        inputs = eq_class.get("inputs", [])
        inputs_str = ", ".join(format_csharp_input(i) for i in inputs)
        expected = eq_class["expected"]

        assert_code = generate_assert(method_name, inputs_str, expected, signature)

        blocks.append(TEST_METHOD_TEMPLATE.format(
            case_desc=eq_class["case"],
            method_name=method_name,
            case_name=sanitize_name(eq_class["case"]),
            pre=pre,
            expected=expected,
            assert_code=assert_code,
        ))
    return blocks


def render_and_save(spec: Dict[str, Any], config: Dict[str, Any]) -> None:
    module_name = spec["module"]
    test_methods = []
    for method in spec["methods"]:
        test_methods.extend(generate_method_tests(method))

    content = TEST_FILE_TEMPLATE.format(
        spec_source=config.get("spec_path", "N/A"),
        module_name=module_name,
        test_methods="\n".join(test_methods),
    )

    out_dir = Path(config.get("output_dir", "tests/Module.Tests"))
    out_dir.mkdir(parents=True, exist_ok=True)
    output_file = out_dir / f"{module_name}Tests.Generated.cs"
    output_file.write_text(content, encoding="utf-8")

    total_tests = sum(len(m.get("equivalence_classes", [])) for m in spec["methods"])
    print(f"[OK] Сгенерирован: {output_file}")
    print(f"[OK] Методов: {len(spec['methods'])}")
    print(f"[OK] Тестов:  {total_tests}")


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--config", default="config.yaml")
    args = parser.parse_args()

    with open(args.config, "r", encoding="utf-8") as f:
        config = yaml.safe_load(f)

    spec = load_spec(config["spec_path"])
    render_and_save(spec, config)