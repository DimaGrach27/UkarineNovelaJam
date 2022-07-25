using System.Collections.Generic;
using UnityEngine;

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

    public static string GetName(CharacterName characterName)
    {
        string result = "";
        
        switch (characterName)
        {
            case CharacterName.NONE:
                result = GlobalConstant.NONE_NAME;
                break;
            case CharacterName.VILSHANKA:
                result = GlobalConstant.VILSHANKA_NAME;
                break;
            case CharacterName.VILSHANKA_FUTURE:
                result = GlobalConstant.VILSHANKA_FUTURE_NAME;
                break;
            case CharacterName.ILONA:
                result = GlobalConstant.ILONA_NAME;
                break;
            case CharacterName.ZAHARES:
                result = GlobalConstant.ZAHARES_NAME;
                break;
            case CharacterName.OLEKSIY:
                result = GlobalConstant.OLEKSIY_NAME;
                break;
        }

        return result;
    }
    
    public static Sprite GetSprite(CharacterSprite characterName)
    {
        Sprite result = null;
        
        switch (characterName)
        {
            case CharacterSprite.VILSHANKA_DEFAULT :
                result = GlobalConstant.VILSHANKA_DEFAULT_SPRITE;
                break;
            case CharacterSprite.VILSHANKA_FUTURE_DEFAULT :
                result = GlobalConstant.VILSHANKA_FUTURE_DEFAULT_SPRITE;
                break;
            case CharacterSprite.ILONA_DEFAULT :
                result = GlobalConstant.ILONA_DEFAULT_SPRITE;
                break;
            case CharacterSprite.ZAHARES_DEFAULT :
                result = GlobalConstant.ZAHARES_DEFAULT_SPRITE;
                break;
            case CharacterSprite.OLEKSIY_DEFAULT :
                result = GlobalConstant.OLEKSIY_DEFAULT_SPRITE;
                break;
        }

        return result;
    }
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


public enum CharacterName
{
    NONE = 0,
    VILSHANKA = 1,
    ILONA = 2,
    ZAHARES = 3,
    OLEKSIY = 4,
    VILSHANKA_FUTURE = 5
}

public enum CharacterSprite
{
    NONE = 0,
    VILSHANKA_DEFAULT = 1,
    ILONA_DEFAULT = 2,
    ZAHARES_DEFAULT = 3,
    OLEKSIY_DEFAULT = 4,
    VILSHANKA_FUTURE_DEFAULT = 5
}