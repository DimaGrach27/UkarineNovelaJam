using UnityEngine;

public static class GlobalConstant
{
    public const int MAX_HEALTH = 4;
    
    public const float TYPING_SPEED = 0.05f;
    public const float ANIMATION_DISSOLVE_DURATION = 0.75f;
    public const float DEFAULT_FADE_DURATION = 1.5f;
    public const float CAMERA_ACTION_FLASH_DURATION = 0.5f;
    
    public static readonly Color ColorWitheClear = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    
    public const string NONE_NAME = "";
    public const string VILSHANKA_NAME = "Вільшанка";
    public const string VILSHANKA_FUTURE_NAME = "Вільшанка 2";
    public const string ILONA_NAME = "Ілона";
    public const string ZAHARES_NAME = "Захарес";
    public const string OLEKSIY_NAME = "Олексій";

    private static Sprite _VILSHANKA_DEFAULT_SPRITE;
    public static Sprite VILSHANKA_DEFAULT_SPRITE
    {
        get
        {
            if (_VILSHANKA_DEFAULT_SPRITE == null)
            {
                _VILSHANKA_DEFAULT_SPRITE = Resources.Load<Sprite>("Configs/Characters/character_vilshanka");
            }

            return _VILSHANKA_DEFAULT_SPRITE;
        }
    }
    
    private static Sprite _ILONA_DEFAULT_SPRITE;
    public static Sprite ILONA_DEFAULT_SPRITE
    {
        get
        {
            if (_ILONA_DEFAULT_SPRITE == null)
            {
                _ILONA_DEFAULT_SPRITE = Resources.Load<Sprite>("Configs/Characters/character_ilona");
            }

            return _ILONA_DEFAULT_SPRITE;
        }
    }
    
    private static Sprite _ZAHARES_DEFAULT_SPRITE;
    public static Sprite ZAHARES_DEFAULT_SPRITE
    {
        get
        {
            if (_ZAHARES_DEFAULT_SPRITE == null)
            {
                _ZAHARES_DEFAULT_SPRITE = Resources.Load<Sprite>("Configs/Characters/character_zakhares");
            }

            return _ZAHARES_DEFAULT_SPRITE;
        }
    }
    
    private static Sprite _OLEKSIY_DEFAULT_SPRITE;
    public static Sprite OLEKSIY_DEFAULT_SPRITE
    {
        get
        {
            if (_OLEKSIY_DEFAULT_SPRITE == null)
            {
                _OLEKSIY_DEFAULT_SPRITE = Resources.Load<Sprite>("Configs/Characters/character_oleksii");
            }

            return _OLEKSIY_DEFAULT_SPRITE;
        }
    }
    
    private static Sprite _VILSHANKA_FUTURE_DEFAULT_SPRITE;
    public static Sprite VILSHANKA_FUTURE_DEFAULT_SPRITE
    {
        get
        {
            if (_VILSHANKA_FUTURE_DEFAULT_SPRITE == null)
            {
                _VILSHANKA_FUTURE_DEFAULT_SPRITE = Resources.Load<Sprite>("Configs/Characters/character_vilshanka_future");
            }

            return _VILSHANKA_FUTURE_DEFAULT_SPRITE;
        }
    }
}
