using System.Collections.Generic;

public static class GameModel
{

    private static bool _gameWasInit = false;
    
    private static readonly Dictionary<StatusEnum, bool> StatusMap = new();

    public static void Init()
    {
        if(_gameWasInit) return;
        
        _gameWasInit = true;
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

        switch (statusEnum)
        {
            case StatusEnum.BOTTLE:
                SaveService.SaveFile.bottle = status;
                break;
        }
        
        SaveService.SaveJson();
    }
}

public enum StatusEnum
{
    NONE = 0,
    
    BOTTLE = 100
}