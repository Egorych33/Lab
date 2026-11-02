using NUnit.Framework;
using Lab.Interfaces;

namespace Tests;

// ======================================================================
//  ТЕСТЫ "БЕЛОГО ЯЩИКА" (на основе анализа реализации)
//
//  Разрабатываются ПОСЛЕ анализа кода каждой конкретной реализации.
//  Их цель — выявить дефекты, не описанные в ТЗ явно:
//    - регистрозависимость словарей,
//    - обработка пробельных символов,
//    - использование конструкторов исключений,
//    - особенности нормализации входных данных.
// ======================================================================
[TestFixture(Category = "WhiteBox")]
public class WhiteBoxTests
{
    // ==================================================================
    //  GenCode1 (DeepSeek)
    //  Особенности:
    //   - Словари: StringComparer.OrdinalIgnoreCase (регистронезависимые).
    //   - NormalizeHex вызывает Trim() — пробелы вокруг HEX допустимы.
    //   - NameToHex вызывает Trim() — пробелы вокруг имени допустимы.
    //   - ArgumentOutOfRangeException: (nameof(r), message) — корректно.
    // ==================================================================
    [TestFixture]
    public class GenCode1_WhiteBoxTests
    {
        private IColorParser _parser = new Lab.Implementations.GenCode1.ColorParser();

        [Test]
        public void NameToHex_WithSpaces_ReturnsHex()
        {
            // Проверяется: устойчивость к пробелам вокруг имени.
            // Вход: "  Red  ". Ожидание: "#FF0000".
            Assert.That(_parser.NameToHex("  Red  "), Is.EqualTo("#FF0000"));
        }

        [Test]
        public void GetColorName_LowerCaseHex_IsCaseInsensitive()
        {
            // Проверяется: регистронезависимость словаря HexToNameMap.
            // Вход: "#ff0000". Ожидание: "Red".
            Assert.That(_parser.GetColorName("#ff0000"), Is.EqualTo("Red"));
        }

        [Test]
        public void HexToRgb_WithSpaces_ReturnsCorrectRgb()
        {
            // Проверяется: Trim() в NormalizeHex.
            // Вход: "  #FF0000  ". Ожидание: "rgb(255, 0, 0)".
            Assert.That(_parser.HexToRgb("  #FF0000  "), Is.EqualTo("rgb(255, 0, 0)"));
        }

        [Test]
        public void RgbToHex_OutOfRange_ExceptionHasParamName()
        {
            // Проверяется: корректное использование конструктора
            // ArgumentOutOfRangeException с параметром nameof(r).
            // Вход: (300, 0, 0). Ожидание: ex.ParamName == "r".
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _parser.RgbToHex(300, 0, 0));
            Assert.That(ex!.ParamName, Is.EqualTo("r"));
        }
    }

    // ==================================================================
    //  GenCode2 (GigaChat)
    //  Особенности:
    //   - Словари: StringComparer.OrdinalIgnoreCase.
    //   - Trim() применяется и к HEX, и к имени.
    //   - ДЕФЕКТ: в RgbToHex сообщение передано в paramName
    //     конструктора ArgumentOutOfRangeException.
    // ==================================================================
    [TestFixture]
    public class GenCode2_WhiteBoxTests
    {
        private IColorParser _parser = new Lab.Implementations.GenCode2.ColorParser();

        [Test]
        public void HexToRgb_ShortHexLowerCase_Works()
        {
            // Проверяется: сокращённая запись в нижнем регистре.
            // Вход: "#f80". Ожидание: "rgb(255, 136, 0)".
            Assert.That(_parser.HexToRgb("#f80"), Is.EqualTo("rgb(255, 136, 0)"));
        }

        [Test]
        public void HexToRgb_WithSpacesBeforeHash_Works()
        {
            // Проверяется: Trim() перед удалением "#".
            // Вход: "   #FF8800". Ожидание: "rgb(255, 136, 0)".
            Assert.That(_parser.HexToRgb("   #FF8800"), Is.EqualTo("rgb(255, 136, 0)"));
        }

        [Test]
        public void NameToHex_WithSpaces_Works()
        {
            // Проверяется: Trim() имени.
            // Вход: "  Blue  ". Ожидание: "#0000FF".
            Assert.That(_parser.NameToHex("  Blue  "), Is.EqualTo("#0000FF"));
        }

        [Test]
        public void GetColorName_LowerCaseHex_IsCaseInsensitive()
        {
            // Проверяется: регистронезависимость HexToNameMap.
            // Вход: "#ff0000". Ожидание: "Red".
            Assert.That(_parser.GetColorName("#ff0000"), Is.EqualTo("Red"));
        }

        [Test]
        public void RgbToHex_OutOfRange_MessageMisplacedInParamName()
        {
            // ДЕФЕКТ: сообщение передано в paramName.
            // Проверяется: ex.ParamName содержит текст сообщения об ошибке.
            // Вход: (300, 0, 0).
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _parser.RgbToHex(300, 0, 0));
            Assert.That(ex!.ParamName, Does.Contain("Значение RGB"));
        }
    }

    // ==================================================================
    //  GenCode3 (Qwen)
    //  Особенности:
    //   - HexToNameMap: StringComparer.Ordinal (регистрозависимый),
    //     НО NormalizeHex приводит к верхнему регистру, поэтому
    //     поиск всё равно работает регистронезависимо.
    //   - NormalizeHex НЕ вызывает Trim() — пробелы недопустимы.
    //   - NameToHex НЕ вызывает Trim() — пробелы в имени недопустимы.
    //   - Корректно использует nullable-аннотации (out string?).
    // ==================================================================
    [TestFixture]
    public class GenCode3_WhiteBoxTests
    {
        private IColorParser _parser = new Lab.Implementations.GenCode3.ColorParser();

        [Test]
        public void GetColorName_LowerCaseHex_WorksBecauseOfNormalization()
        {
            // Вопреки Ordinal-словарю, NormalizeHex приводит
            // строку к верхнему регистру — поэтому поиск находит цвет.
            // Вход: "#ff0000". Ожидание: "Red".
            Assert.That(_parser.GetColorName("#ff0000"), Is.EqualTo("Red"));
        }

        [Test]
        public void GetColorName_UpperCaseHex_ReturnsRed()
        {
            // Контрольный позитивный тест.
            // Вход: "#FF0000". Ожидание: "Red".
            Assert.That(_parser.GetColorName("#FF0000"), Is.EqualTo("Red"));
        }

        [Test]
        public void NameToHex_WithSpaces_ThrowsBecauseNoTrim()
        {
            // ДЕФЕКТ: NameToHex не тримит входную строку.
            // Вход: "  Red  ". Ожидание: ArgumentException.
            var ex = Assert.Throws<ArgumentException>(() => _parser.NameToHex("  Red  "));
            Assert.That(ex!.Message, Does.Contain("не найдено"));
        }

        [Test]
        public void NameToHex_WithoutSpaces_Works()
        {
            // Контроль: без пробелов имя находится.
            // Вход: "Red". Ожидание: "#FF0000".
            Assert.That(_parser.NameToHex("Red"), Is.EqualTo("#FF0000"));
        }

        [Test]
        public void HexToRgb_WithSpaces_ThrowsBecauseNoTrim()
        {
            // ДЕФЕКТ: NormalizeHex не тримит HEX.
            // Вход: "  #FF0000  ". Ожидание: ArgumentException.
            Assert.Throws<ArgumentException>(() => _parser.HexToRgb("  #FF0000  "));
        }

        [Test]
        public void HexToRgb_LowerCaseHex_Works()
        {
            // IsHexChar принимает и нижний, и верхний регистр.
            // Вход: "#ff8800". Ожидание: "rgb(255, 136, 0)".
            Assert.That(_parser.HexToRgb("#ff8800"), Is.EqualTo("rgb(255, 136, 0)"));
        }
    }
}