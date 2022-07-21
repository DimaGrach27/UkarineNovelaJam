using System.Collections.Generic;

public static class GameModel
{
    private static readonly Dictionary<StatusEnum, bool> StatusMap = new();

    public static void Init()
    {
        StatusMap.Add(StatusEnum.BOTTLE, SaveService.SaveFile.bottle);
    }

    public static bool GetStatus(StatusEnum statusEnum)
    {
        if (!StatusMap.ContainsKey(statusEnum)) return false;

        return StatusMap[statusEnum];
    }
    
    public static void SetStatus(StatusEnum statusEnum, bool status)
    {
        if (!StatusMap.ContainsKey(statusEnum)) return;

        StatusMap[statusEnum] = status;
    }
}

public enum StatusEnum
{
    NONE = 0,
    
    BOTTLE = 100
}