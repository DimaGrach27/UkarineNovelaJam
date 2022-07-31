using System.Collections.Generic;
using GameScene.ScreenPart;
using UnityEngine;

public static class GameModel
{
    public static bool GameWasInit = false;
    
    private static readonly Dictionary<string, ScreenSceneScriptableObject> ScreenScenesMap = new();

    public static void Init()
    {
        if(GameWasInit) return;
        
        GameWasInit = true;
        
        ScreenSceneScriptableObject[] list = 
            Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

        foreach (var screenScene in list)
        {
            ScreenScenesMap.Add(screenScene.SceneKey, screenScene);
        }
    }

    public static ScreenSceneScriptableObject GetScene(string key)
    {
        if (ScreenScenesMap.ContainsKey(key))
            return ScreenScenesMap[key];

        return null;
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
    
    public static int GetInt(KillerName countType) => SaveService.GetIntValue(countType);
    public static void SetInt(KillerName countType, int value) => SaveService.SetIntValue(countType, value);
    

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
            case CharacterName.OREST:
                result = GlobalConstant.OREST_NAME;
                break;
            case CharacterName.UNKNOW:
                result = GlobalConstant.UNKNOW_NAME;
                break;
            case CharacterName.ILONA_SUR:
                result = GlobalConstant.ILONA_SURNAME;
                break;
            case CharacterName.ZAHARES_SUR:
                result = GlobalConstant.ZAHARES_SUR_NAME;
                break;
            case CharacterName.OLEKSIY_SUR:
                result = GlobalConstant.OLEKSIY_SUR_NAME;
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
    isThrowStick = 23,
    ILONA_HAVE_SHOW = 24,
    ZAHARES_HAVE_SHOW = 25,
    OLEKSII_HAVE_SHOW = 26,
    CHOOSE_WAS_PICK = 27,
    
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
    VILSHANKA_FUTURE = 5,
    OREST = 6,
    UNKNOW = 7,
    ILONA_SUR = 8,
    ZAHARES_SUR = 9,
    OLEKSIY_SUR = 10,
}

public enum CharacterSprite
{
    NONE = 0,
    VILSHANKA_DEFAULT = 1,
    ILONA_DEFAULT = 2,
    ZAHARES_DEFAULT = 3,
    OLEKSIY_DEFAULT = 4,
    VILSHANKA_FUTURE_DEFAULT = 5,
    ZAHARES_DARK = 6,
    OLEKSIY_DARK = 7,
    VILSHANKA_PSYCHO = 8,
    OLEKSIY_PSYCHO = 9,
    ZAHARES_PSYCHO = 10,
    ILONA_PSYCHO = 11,
    VILSHANKA_FUTURE_PSYCHO = 12,
}

public enum KillerName
{
    NONE_VOR = 0,
    ILONA_VOR = 1,
    ZAHARES_VOR = 2,
    OLEKSIY_VOR = 3,
}