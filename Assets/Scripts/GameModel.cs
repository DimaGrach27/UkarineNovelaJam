using System.Collections.Generic;

public static class GameModel
{
    private static bool _gameWasInit = false;
    
    private static readonly Dictionary<StatusEnum, bool> StatusMap = new();

    public static void Init()
    {
        if(_gameWasInit) return;
        
        _gameWasInit = true;
        StatusMap.Add(StatusEnum.BOTTLE, SaveService.GetStatusValue(StatusEnum.BOTTLE));
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

        SaveService.SetStatusValue(statusEnum, status);
    }

    public static int GetInt(CountType countType) => SaveService.GetIntValue(countType);
    public static void SetInt(CountType countType, int value) => SaveService.SetIntValue(countType, value);
}

public enum StatusEnum
{
    NONE = 0,
    
    BOTTLE = 100
}

public enum CountType
{
    NONE = 0,
    SEARCH_PLACE = 1,
    BELIEF = 2,
    ESCAPE = 3
}