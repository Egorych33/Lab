namespace Lab.Interfaces;

public interface IColorParser
{
    string HexToRgb(string hex);
    string RgbToHex(int r, int g, int b);
    string GetColorName(string hex);
    string NameToHex(string name);
}