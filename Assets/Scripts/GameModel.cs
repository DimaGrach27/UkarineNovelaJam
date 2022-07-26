using System.Collections.Generic;
using UnityEngine;

public static class GameModel
{
    private static bool _gameWasInit = false;
    

    public static void Init()
    {
        if(_gameWasInit) return;
        
        _gameWasInit = true;
    }

    public static bool GetStatus(StatusEnum statusEnum)
    {
        return SaveService.GetStatusValue(statusEnum);
    }
    
    public static void SetStatus(StatusEnum statusEnum, bool status)
    {
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
    weirdtoZakh = 1,
    RodandBuckettaken = 2,
    inSearch = 3,
    bulletFound = 4,
    numberFound = 5,
    IlonaRevealed = 6,
    bulletAnalyze = 7,
    diaryRead = 8,
    rudetoIlona = 9,
    connectionstoKorniy = 10,
    locketFound = 11,
    rudetoZakh = 12,
    IlonaGuilty = 13,
    isCanVisitBurshtyn = 14,
    wasVisitBurshtyn = 15,
    isCanVisitLake = 16,
    isSearchFishingSpot = 17,
    isSearchBeachSand = 18,
    isSearchAroundLake = 19,
    wasVisitLake = 20,
    isCanVisitIlonaAndTower = 21,
    isAngryToFriend = 22,
    
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