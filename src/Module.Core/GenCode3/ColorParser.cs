using System;
using System.Collections.Generic;
using Lab.Interfaces;

namespace Lab.Implementations.GenCode3;

public class ColorParser : IColorParser
{
    private static readonly Dictionary<string, string> HexToNameMap = new(StringComparer.Ordinal)
    {
        { "#FF0000", "Red" },
        { "#00FF00", "Green" },
        { "#0000FF", "Blue" },
        { "#FFFFFF", "White" },
        { "#000000", "Black" },
        { "#FFFF00", "Yellow" }
    };

    private static readonly Dictionary<string, string> NameToHexMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Red",    "#FF0000" },
        { "Green",  "#00FF00" },
        { "Blue",   "#0000FF" },
        { "White",  "#FFFFFF" },
        { "Black",  "#000000" },
        { "Yellow", "#FFFF00" }
    };

    public string HexToRgb(string hex)
    {
        string normalized = NormalizeHex(hex);

        int r = Convert.ToInt32(normalized.Substring(1, 2), 16);
        int g = Convert.ToInt32(normalized.Substring(3, 2), 16);
        int b = Convert.ToInt32(normalized.Substring(5, 2), 16);

        return $"rgb({r}, {g}, {b})";
    }

    public string RgbToHex(int r, int g, int b)
    {
        if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            throw new ArgumentOutOfRangeException("Значение RGB должно быть в диапазоне от 0 до 255.");

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    public string GetColorName(string hex)
    {
        string normalized = NormalizeHex(hex);

        if (HexToNameMap.TryGetValue(normalized, out string? name))
            return name;

        throw new ArgumentException("Цвет не найден в справочнике.");
    }

    public string NameToHex(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название цвета не найдено.");

        if (NameToHexMap.TryGetValue(name, out string? hex))
            return hex;

        throw new ArgumentException("Название цвета не найдено.");
    }

    /// <summary>
    /// Нормализует HEX-строку к формату "#RRGGBB" (верхний регистр).
    /// Поддерживает запись с '#' и без него, а также сокращённую форму "#RGB".
    /// </summary>
    private static string NormalizeHex(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            throw new ArgumentException("Некорректный HEX-код.");

        string clean = hex.TrimStart('#');

        if (clean.Length == 3)
        {
            if (!IsHexChar(clean[0]) || !IsHexChar(clean[1]) || !IsHexChar(clean[2]))
                throw new ArgumentException("Некорректный HEX-код.");

            clean = string.Create(6, clean, static (span, src) =>
            {
                span[0] = src[0]; span[1] = src[0];
                span[2] = src[1]; span[3] = src[1];
                span[4] = src[2]; span[5] = src[2];
            });
        }
        else if (clean.Length == 6)
        {
            for (int i = 0; i < clean.Length; i++)
            {
                if (!IsHexChar(clean[i]))
                    throw new ArgumentException("Некорректный HEX-код.");
            }
        }
        else
        {
            throw new ArgumentException("Некорректный HEX-код.");
        }

        return "#" + clean.ToUpperInvariant();
    }

    private static bool IsHexChar(char c)
    {
        return (c >= '0' && c <= '9')
            || (c >= 'a' && c <= 'f')
            || (c >= 'A' && c <= 'F');
    }
}