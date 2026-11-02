using NUnit.Framework;
using Lab.Interfaces;

namespace Tests;

// ======================================================================
//  ТЕСТЫ "ЧЁРНОГО ЯЩИКА" (общие для всех реализаций)
//
//  Разрабатываются строго на основе спецификации (ТЗ),
//  без анализа внутренней реализации класса.
//
//  ВАЖНО: перед запуском раскомментируй ТОЛЬКО одну строку в Setup(),
//  соответствующую тестируемой реализации.
// ======================================================================
[TestFixture(Category = "BlackBox")]
public class BlackBoxTests
{
    private IColorParser _manager;

    [SetUp]
    public void Setup()
    {
        // Выбор тестируемой реализации:
        // _manager = new Lab.Implementations.GenCode1.ColorParser();
        // _manager = new Lab.Implementations.GenCode2.ColorParser();
         _manager = new Lab.Implementations.GenCode3.ColorParser();
    }

    // ------------------------------------------------------------------
    //  HexToRgb — позитивные сценарии
    // ------------------------------------------------------------------

    [Test]
    public void HexToRgb_ValidFullHex_ReturnsCorrectRgb()
    {
        // Проверяется: базовая конвертация HEX с "#" в RGB.
        // Вход: "#FF8800". Ожидание: "rgb(255, 136, 0)".
        var result = _manager.HexToRgb("#FF8800");
        Assert.That(result, Is.EqualTo("rgb(255, 136, 0)"));
    }

    [Test]
    public void HexToRgb_ValidHexWithoutHash_ReturnsCorrectRgb()
    {
        // Проверяется: конвертация HEX без символа "#".
        // Вход: "FF8800". Ожидание: "rgb(255, 136, 0)".
        var result = _manager.HexToRgb("FF8800");
        Assert.That(result, Is.EqualTo("rgb(255, 136, 0)"));
    }

    [Test]
    public void HexToRgb_ShortHex_ExpandsCorrectly()
    {
        // Проверяется: поддержка сокращённой записи "#F80".
        // Вход: "#F80". Ожидание: "rgb(255, 136, 0)".
        var result = _manager.HexToRgb("#F80");
        Assert.That(result, Is.EqualTo("rgb(255, 136, 0)"));
    }

    [Test]
    public void HexToRgb_LowerCaseHex_IsAccepted()
    {
        // Проверяется: регистронезависимость.
        // Вход: "#ff8800". Ожидание: "rgb(255, 136, 0)".
        var result = _manager.HexToRgb("#ff8800");
        Assert.That(result, Is.EqualTo("rgb(255, 136, 0)"));
    }

    // ------------------------------------------------------------------
    //  HexToRgb — негативные сценарии и исключения
    // ------------------------------------------------------------------

    [Test]
    public void HexToRgb_InvalidLength_ThrowsArgumentException()
    {
        // Проверяется: HEX неверной длины.
        // Вход: "#FF88". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.HexToRgb("#FF88"));
    }

    [Test]
    public void HexToRgb_InvalidCharacters_ThrowsArgumentException()
    {
        // Проверяется: HEX с недопустимыми символами.
        // Вход: "#GGGGGG". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.HexToRgb("#GGGGGG"));
    }

    [Test]
    public void HexToRgb_Null_ThrowsArgumentException()
    {
        // Проверяется: null в качестве входа.
        // Вход: null. Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.HexToRgb(null!));
    }

    [Test]
    public void HexToRgb_EmptyString_ThrowsArgumentException()
    {
        // Проверяется: пустая строка.
        // Вход: "". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.HexToRgb(""));
    }

    // ------------------------------------------------------------------
    //  RgbToHex
    // ------------------------------------------------------------------

    [Test]
    public void RgbToHex_ValidValues_ReturnsCorrectHex()
    {
        // Проверяется: базовая конвертация RGB в HEX.
        // Вход: (255, 136, 0). Ожидание: "#FF8800".
        var result = _manager.RgbToHex(255, 136, 0);
        Assert.That(result, Is.EqualTo("#FF8800"));
    }

    [Test]
    public void RgbToHex_ZeroValues_ReturnsBlack()
    {
        // Проверяется: граничный случай — нижняя граница диапазона.
        // Вход: (0, 0, 0). Ожидание: "#000000".
        var result = _manager.RgbToHex(0, 0, 0);
        Assert.That(result, Is.EqualTo("#000000"));
    }

    [Test]
    public void RgbToHex_MaxValues_ReturnsWhite()
    {
        // Проверяется: граничный случай — верхняя граница диапазона.
        // Вход: (255, 255, 255). Ожидание: "#FFFFFF".
        var result = _manager.RgbToHex(255, 255, 255);
        Assert.That(result, Is.EqualTo("#FFFFFF"));
    }

    [Test]
    public void RgbToHex_ValueBelowRange_ThrowsArgumentOutOfRangeException()
    {
        // Проверяется: значение ниже 0.
        // Вход: (-1, 0, 0). Ожидание: ArgumentOutOfRangeException.
        Assert.Throws<ArgumentOutOfRangeException>(() => _manager.RgbToHex(-1, 0, 0));
    }

    [Test]
    public void RgbToHex_ValueAboveRange_ThrowsArgumentOutOfRangeException()
    {
        // Проверяется: значение выше 255.
        // Вход: (256, 0, 0). Ожидание: ArgumentOutOfRangeException.
        Assert.Throws<ArgumentOutOfRangeException>(() => _manager.RgbToHex(256, 0, 0));
    }

    // ------------------------------------------------------------------
    //  GetColorName
    // ------------------------------------------------------------------

    [Test]
    public void GetColorName_Red_ReturnsRed()
    {
        // Проверяется: поиск базового цвета в справочнике.
        // Вход: "#FF0000". Ожидание: "Red".
        Assert.That(_manager.GetColorName("#FF0000"), Is.EqualTo("Red"));
    }

    [Test]
    public void GetColorName_LowerCaseHex_ReturnsRed()
    {
        // Проверяется: регистронезависимость поиска.
        // Вход: "#ff0000". Ожидание: "Red".
        Assert.That(_manager.GetColorName("#ff0000"), Is.EqualTo("Red"));
    }

    [Test]
    public void GetColorName_UnknownColor_ThrowsArgumentException()
    {
        // Проверяется: цвет, которого нет в справочнике.
        // Вход: "#123456". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.GetColorName("#123456"));
    }

    // ------------------------------------------------------------------
    //  NameToHex
    // ------------------------------------------------------------------

    [Test]
    public void NameToHex_Red_ReturnsHex()
    {
        // Проверяется: преобразование имени в HEX.
        // Вход: "Red". Ожидание: "#FF0000".
        Assert.That(_manager.NameToHex("Red"), Is.EqualTo("#FF0000"));
    }

    [Test]
    public void NameToHex_LowerCaseName_ReturnsHex()
    {
        // Проверяется: регистронезависимость имени.
        // Вход: "red". Ожидание: "#FF0000".
        Assert.That(_manager.NameToHex("red"), Is.EqualTo("#FF0000"));
    }

    [Test]
    public void NameToHex_UnknownName_ThrowsArgumentException()
    {
        // Проверяется: неизвестное название цвета.
        // Вход: "Pink". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.NameToHex("Pink"));
    }

    [Test]
    public void NameToHex_EmptyString_ThrowsArgumentException()
    {
        // Проверяется: пустая строка.
        // Вход: "". Ожидание: ArgumentException.
        Assert.Throws<ArgumentException>(() => _manager.NameToHex(""));
    }
}