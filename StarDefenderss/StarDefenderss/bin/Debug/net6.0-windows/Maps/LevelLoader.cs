using System.ComponentModel;
using System.IO;

namespace StarDefenderss;

public static class MapLoader
{
    public static string Load(LevelName levelName)
    {
        return File.ReadAllText("Maps/" + levelName.ToDescriptionString());
    }
}

public enum LevelName
{
    [Description("FirstLevel.txt")] LevelFirst,
}

public static class LevelNameExtensions
{
    public static string ToDescriptionString(this LevelName val)
    {
        var attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes!.Length > 0 ? attributes[0].Description : string.Empty;
    }
}