using System;
using System.Collections.Generic;
using Lab.Interfaces;

namespace Lab.Implementations.GenCode2
{
    public class ColorParser : IColorParser
    {
        // Справочники для поиска по HEX и по имени
        private static readonly Dictionary<string, string> HexToNameMap = new(StringComparer.OrdinalIgnoreCase)
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
            { "Red", "#FF0000" },
            { "Green", "#00FF00" },
            { "Blue", "#0000FF" },
            { "White", "#FFFFFF" },
            { "Black", "#000000" },
            { "Yellow", "#FFFF00" }
        };

        public string HexToRgb(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Некорректный HEX-код.");

            // Удаляем символ # и приводим к верхнему регистру
            string cleanHex = hex.Trim().TrimStart('#').ToUpperInvariant();

            // Поддержка сокращенной записи #RGB
            if (cleanHex.Length == 3)
            {
                cleanHex = $"{cleanHex[0]}{cleanHex[0]}{cleanHex[1]}{cleanHex[1]}{cleanHex[2]}{cleanHex[2]}";
            }

            if (cleanHex.Length != 6)
                throw new ArgumentException("Некорректный HEX-код.");

            // Проверка на допустимые символы
            if (!IsHexStringValid(cleanHex))
                throw new ArgumentException("Некорректный HEX-код.");

            // Парсинг компонентов
            int r = Convert.ToInt32(cleanHex.Substring(0, 2), 16);
            int g = Convert.ToInt32(cleanHex.Substring(2, 2), 16);
            int b = Convert.ToInt32(cleanHex.Substring(4, 2), 16);

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
                throw new ArgumentException("Название цвета не найдено.");

            if (NameToHexMap.TryGetValue(name.Trim(), out string hex))
            {
                return hex;
            }

            throw new ArgumentException("Название цвета не найдено.");
        }

        // Приводит HEX к стандартному виду #RRGGBB
        private static string NormalizeHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Некорректный HEX-код.");

            string cleanHex = hex.Trim().TrimStart('#').ToUpperInvariant();

            if (cleanHex.Length == 3)
            {
                cleanHex = $"{cleanHex[0]}{cleanHex[0]}{cleanHex[1]}{cleanHex[1]}{cleanHex[2]}{cleanHex[2]}";
            }

            if (cleanHex.Length != 6 || !IsHexStringValid(cleanHex))
                throw new ArgumentException("Некорректный HEX-код.");

            return $"#{cleanHex}";
        }

        // Проверяет, что строка состоит только из шестнадцатеричных символов
        private static bool IsHexStringValid(string hex)
        {
            foreach (char c in hex)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}