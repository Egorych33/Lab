// ==============================================================
// AUTO-GENERATED TESTS. DO NOT EDIT MANUALLY.
// Source: spec/colorparser.yaml
// Generator: gen_tests.py v2.0 (ColorParser)
// ==============================================================
using System;
using NUnit.Framework;
using Lab.Interfaces;

namespace Module.Tests
{
    [TestFixture]
    [Description("Автоматически сгенерированные тесты для ColorParser")]
    public class ColorParserTests
    {
        private IColorParser _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Lab.Implementations.GenCode1.ColorParser();
        }


        [Test]
        [Description("Класс эквивалентности: Валидный 6-значный HEX")]
        public void Test_HexToRgb_Validnyy_6_znachnyy_HEX()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: "rgb(255, 0, 0)"

            // === Act & Assert ===
            var result = _sut.HexToRgb("#FF0000");
            Assert.That(result, Is.EqualTo("rgb(255, 0, 0)"));
        }


        [Test]
        [Description("Класс эквивалентности: Валидный 3-значный HEX")]
        public void Test_HexToRgb_Validnyy_3_znachnyy_HEX()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: "rgb(255, 0, 0)"

            // === Act & Assert ===
            var result = _sut.HexToRgb("#F00");
            Assert.That(result, Is.EqualTo("rgb(255, 0, 0)"));
        }


        [Test]
        [Description("Класс эквивалентности: HEX без решётки")]
        public void Test_HexToRgb_HEX_bez_reshёtki()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: "rgb(0, 255, 0)"

            // === Act & Assert ===
            var result = _sut.HexToRgb("00FF00");
            Assert.That(result, Is.EqualTo("rgb(0, 255, 0)"));
        }


        [Test]
        [Description("Класс эквивалентности: Null")]
        public void Test_HexToRgb_Null()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.HexToRgb(null));
        }


        [Test]
        [Description("Класс эквивалентности: Пустая строка")]
        public void Test_HexToRgb_Pustaya_stroka()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.HexToRgb(""));
        }


        [Test]
        [Description("Класс эквивалентности: Невалидные символы")]
        public void Test_HexToRgb_Nevalidnye_simvoly()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.HexToRgb("#GGGGGG"));
        }


        [Test]
        [Description("Класс эквивалентности: Неверная длина")]
        public void Test_HexToRgb_Nevernaya_dlina()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex) && hex — валидный HEX
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.HexToRgb("#FFF0"));
        }


        [Test]
        [Description("Класс эквивалентности: Валидные значения")]
        public void Test_RgbToHex_Validnye_znacheniya()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: "#FF0000"

            // === Act & Assert ===
            var result = _sut.RgbToHex(255, 0, 0);
            Assert.That(result, Is.EqualTo("#FF0000"));
        }


        [Test]
        [Description("Класс эквивалентности: Нули")]
        public void Test_RgbToHex_Nuli()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: "#000000"

            // === Act & Assert ===
            var result = _sut.RgbToHex(0, 0, 0);
            Assert.That(result, Is.EqualTo("#000000"));
        }


        [Test]
        [Description("Класс эквивалентности: Максимум")]
        public void Test_RgbToHex_Maksimum()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: "#FFFFFF"

            // === Act & Assert ===
            var result = _sut.RgbToHex(255, 255, 255);
            Assert.That(result, Is.EqualTo("#FFFFFF"));
        }


        [Test]
        [Description("Класс эквивалентности: R вне диапазона")]
        public void Test_RgbToHex_R_vne_diapazona()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: ArgumentOutOfRangeException

            // === Act & Assert ===
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.RgbToHex(256, 0, 0));
        }


        [Test]
        [Description("Класс эквивалентности: G отрицательный")]
        public void Test_RgbToHex_G_otritsatelnyy()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: ArgumentOutOfRangeException

            // === Act & Assert ===
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.RgbToHex(0, -1, 0));
        }


        [Test]
        [Description("Класс эквивалентности: B вне диапазона")]
        public void Test_RgbToHex_B_vne_diapazona()
        {
            // === Arrange ===
            // Precondition: 0 <= r, g, b <= 255
            // Expected: ArgumentOutOfRangeException

            // === Act & Assert ===
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.RgbToHex(0, 0, 300));
        }


        [Test]
        [Description("Класс эквивалентности: Известный цвет (Red)")]
        public void Test_GetColorName_Izvestnyy_tsvet_Red()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: "Red"

            // === Act & Assert ===
            var result = _sut.GetColorName("#FF0000");
            Assert.That(result, Is.EqualTo("Red"));
        }


        [Test]
        [Description("Класс эквивалентности: Известный цвет (Green)")]
        public void Test_GetColorName_Izvestnyy_tsvet_Green()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: "Green"

            // === Act & Assert ===
            var result = _sut.GetColorName("#00FF00");
            Assert.That(result, Is.EqualTo("Green"));
        }


        [Test]
        [Description("Класс эквивалентности: 3-значный HEX")]
        public void Test_GetColorName_3_znachnyy_HEX()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: "White"

            // === Act & Assert ===
            var result = _sut.GetColorName("#FFF");
            Assert.That(result, Is.EqualTo("White"));
        }


        [Test]
        [Description("Класс эквивалентности: HEX без решётки")]
        public void Test_GetColorName_HEX_bez_reshёtki()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: "Blue"

            // === Act & Assert ===
            var result = _sut.GetColorName("0000FF");
            Assert.That(result, Is.EqualTo("Blue"));
        }


        [Test]
        [Description("Класс эквивалентности: Неизвестный цвет")]
        public void Test_GetColorName_Neizvestnyy_tsvet()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.GetColorName("#123456"));
        }


        [Test]
        [Description("Класс эквивалентности: Null")]
        public void Test_GetColorName_Null()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.GetColorName(null));
        }


        [Test]
        [Description("Класс эквивалентности: Невалидный HEX")]
        public void Test_GetColorName_Nevalidnyy_HEX()
        {
            // === Arrange ===
            // Precondition: hex != null && !string.IsNullOrWhiteSpace(hex)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.GetColorName("#GGGGGG"));
        }


        [Test]
        [Description("Класс эквивалентности: Red")]
        public void Test_NameToHex_Red()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: "#FF0000"

            // === Act & Assert ===
            var result = _sut.NameToHex("Red");
            Assert.That(result, Is.EqualTo("#FF0000"));
        }


        [Test]
        [Description("Класс эквивалентности: Имя в нижнем регистре")]
        public void Test_NameToHex_Imya_v_nizhnem_registre()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: "#FF0000"

            // === Act & Assert ===
            var result = _sut.NameToHex("red");
            Assert.That(result, Is.EqualTo("#FF0000"));
        }


        [Test]
        [Description("Класс эквивалентности: Имя с пробелами")]
        public void Test_NameToHex_Imya_s_probelami()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: "#00FF00"

            // === Act & Assert ===
            var result = _sut.NameToHex("  Green  ");
            Assert.That(result, Is.EqualTo("#00FF00"));
        }


        [Test]
        [Description("Класс эквивалентности: White")]
        public void Test_NameToHex_White()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: "#FFFFFF"

            // === Act & Assert ===
            var result = _sut.NameToHex("White");
            Assert.That(result, Is.EqualTo("#FFFFFF"));
        }


        [Test]
        [Description("Класс эквивалентности: Неизвестное имя")]
        public void Test_NameToHex_Neizvestnoe_imya()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.NameToHex("Purple"));
        }


        [Test]
        [Description("Класс эквивалентности: Null")]
        public void Test_NameToHex_Null()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.NameToHex(null));
        }


        [Test]
        [Description("Класс эквивалентности: Пустая строка")]
        public void Test_NameToHex_Pustaya_stroka()
        {
            // === Arrange ===
            // Precondition: name != null && !string.IsNullOrWhiteSpace(name)
            // Expected: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.NameToHex(""));
        }

    }
}
