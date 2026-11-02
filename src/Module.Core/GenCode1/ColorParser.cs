using System;
using System.Collections.Generic;
using Lab.Interfaces;

namespace Lab.Implementations.GenCode1
{
    public class ColorParser : IColorParser
    {
        private static readonly Dictionary<string, string> HexToNameMap = new(StringComparer.OrdinalIgnoreCase)
        {
            {"#FF0000", "Red" },
            {"#00FF00", "Green" },
            {"#0000FF", "Blue" },
            {"#FFFFFF", "White" },
            {"#000000", "Black" },
            {"#FFFF00", "Yellow" }
        };

        private static readonly Dictionary<string, string> NameToHexMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Red", "#FF0000" },
            { "Green", "#00FF00" },
            { "Blue", "#0000FF" },
            { "White", "#FFFFFF" },
            { "Black", "#000000" },
            {"Yellow", "#FFFF00" }
        };

        public string HexToRgb(string hex)
        {
            string normalizedHex = NormalizeHex(hex);

            int r = Convert.ToInt32(normalizedHex.Substring(1, 2), 16);
            int g = Convert.ToInt32(normalizedHex.Substring(3, 2), 16);
            int b = Convert.ToInt32(normalizedHex.Substring(5, 2), 16);

            return $"rgb({r}, {g}, {b})";
        }

        public string RgbToHex(int r, int g, int b)
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(r),
                    "Значение RGB должно быть в диапазоне от 0 до 255.");
            }

            return $"#{r:X2}{g:X2}{b:X2}".ToUpperInvariant();
        }

        public string GetColorName(string hex)
        {
            string normalizedHex = NormalizeHex(hex);

            if (HexToNameMap.TryGetValue(normalizedHex, out string name))
            {
                return name;
            }

            throw new ArgumentException("Цвет не найден в справочнике.");
        }

        public string NameToHex(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название цвета не найдено.");
            }

            if (NameToHexMap.TryGetValue(name.Trim(), out string hex))
            {
                return hex;
            }

            throw new ArgumentException("Название цвета не найдено.");
        }

        private static string NormalizeHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                throw new ArgumentException("Некорректный HEX-код.");
            }

            string cleaned = hex.Trim();

            if (cleaned.StartsWith("#"))
            {
                cleaned = cleaned.Substring(1);
            }

            if (cleaned.Length == 3)
            {
                cleaned = string.Concat(
                    cleaned[0], cleaned[0],
                    cleaned[1], cleaned[1],
                    cleaned[2], cleaned[2]);
            }

            if (cleaned.Length != 6)
            {
                throw new ArgumentException("Некорректный HEX-код.");
            }

            foreach (char c in cleaned)
            {
                if (!Uri.IsHexDigit(c))
                {
                    throw new ArgumentException("Некорректный HEX-код.");
                }
            }

            return "#" + cleaned.ToUpperInvariant();
        }
    }
}