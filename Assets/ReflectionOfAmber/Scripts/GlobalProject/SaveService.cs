using System;
using System.Collections.Generic;
using System.IO;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GlobalProject
 {
     public static class SaveService
     {
         private const string CAMERA_FILM_LEFT_KEY = "camera_film_left";
         private const string HEALTH_COUNT_KEY = "health_count";
         private const string CURRENT_BG_KEY = "current_bg";

         private const string PROGRESS_KEY = "progress";
         private const string CHAPTER_NOTES_KEY = "chapter_notes";
         private const string SETTINGS_KEY = "settings";


         private static SaveFile _saveFile;
         private static SaveFile SaveFile
         {
             get
             {
                 if (_saveFile != null) return _saveFile;

                 if (!ExistFile(PROGRESS_KEY))
                 {
                     _saveFile = new SaveFile();

                     File.WriteAllText(Path(PROGRESS_KEY), JsonUtility.ToJson(_saveFile));
                 }
                 else
                 {
                     _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(Path(PROGRESS_KEY)));
                 }

                 return _saveFile;
             }
         }
         
         private static ChapterNotesFile _chapterNotesFile;
         public static ChapterNotesFile ChapterNotesFile
         {
             get
             {
                 if (_chapterNotesFile != null) return _chapterNotesFile;

                 if (!ExistFile(CHAPTER_NOTES_KEY))
                 {
                     _chapterNotesFile = new ChapterNotesFile();

                     File.WriteAllText(Path(CHAPTER_NOTES_KEY), JsonUtility.ToJson(_chapterNotesFile));
                 }
                 else
                 {
                     _chapterNotesFile = JsonUtility.FromJson<ChapterNotesFile>(File.ReadAllText(Path(CHAPTER_NOTES_KEY)));
                 }

                 return _chapterNotesFile;
             }
         }

         private static SettingFile _settingFile;
         private static SettingFile SettingFile
         {
             get
             {
                 if (_settingFile != null) return _settingFile;

                 if (!ExistFile(SETTINGS_KEY))
                 {
                     _settingFile = new SettingFile();

                     File.WriteAllText(Path(SETTINGS_KEY), JsonUtility.ToJson(_settingFile));
                 }
                 else
                 {
                     _settingFile = JsonUtility.FromJson<SettingFile>(File.ReadAllText(Path(SETTINGS_KEY)));
                 }

                 return _settingFile;
             }
         }

         private static bool ExistFile(string key) => File.Exists(Path(key));

         private static string Path(string key) =>
             Application.persistentDataPath + $"/{key}_{Application.productName}.json";

         private static void SaveJson(string key) => File.WriteAllText(Path(key), JsonUtility.ToJson(GetJson(key)));
         public static void SaveChapterNotesJson() => File.WriteAllText(Path(CHAPTER_NOTES_KEY), 
             JsonUtility.ToJson(GetJson(CHAPTER_NOTES_KEY)));

         private static object GetJson(string key)
         {
             object json = null;

             switch (key)
             {
                 case PROGRESS_KEY:
                     json = SaveFile;
                     break;

                 case SETTINGS_KEY:
                     json = SettingFile;
                     break;
                 
                 case CHAPTER_NOTES_KEY:
                     json = ChapterNotesFile;
                     break;
             }

             return json;
         }

         public static void SavePart(int current)
         {
             SaveFile.currentPart = current;
             SaveJson(PROGRESS_KEY);
         }

         public static void SaveScene(string sceneKey)
         {
             SaveFile.currentScene = sceneKey;
             SaveJson(PROGRESS_KEY);
         }


         public static string GetScene => SaveFile.currentScene;
         public static int GetPart => SaveFile.currentPart;

         public static int CameraFilmLeft
         {
             get => PlayerPrefs.GetInt(CAMERA_FILM_LEFT_KEY, 10);
             set => PlayerPrefs.SetInt(CAMERA_FILM_LEFT_KEY, value);
         }

         public static int HealthCount
         {
             get => PlayerPrefs.GetInt(HEALTH_COUNT_KEY, GlobalConstant.MAX_HEALTH);
             set => PlayerPrefs.SetInt(HEALTH_COUNT_KEY, value);
         }

         public static void SetChoose(string key, string choose)
         {
             Debug.Log("SHOW");
             ChoosesList choosesList = GetListFromJson(key);

             if (choosesList.chooseKeys == null) return;

             for (int i = 0; i < choosesList.chooseKeys.Length; i++)
             {
                 if (choosesList.chooseKeys[i] == choose)
                 {
                     choosesList.chooseStatus[i] = true;

                     SetChoosesList(key, choosesList);
                 }
             }
         }

         public static BgEnum GetCurrentBg()
         {
             int bg = PlayerPrefs.GetInt(CURRENT_BG_KEY, -1);

             return (BgEnum)bg;
         }

         public static void SetCurrentBg(BgEnum bgEnum)
         {
             int bg = (int)bgEnum;

             PlayerPrefs.SetInt(CURRENT_BG_KEY, bg);
         }

         public static void SetChoosesList(string key, ChoosesList choosesList)
         {
             ChoosesList chooses = GetListFromJson(key);

             chooses.blockKey = choosesList.blockKey;
             chooses.chooseKeys = choosesList.chooseKeys;
             chooses.chooseStatus = choosesList.chooseStatus;

             SaveJson(PROGRESS_KEY);
         }


         public static ChoosesList GetListFromJson(string key, out bool isFirstInit)
         {
             ChoosesList choosesList = new ChoosesList();
             isFirstInit = false;

             foreach (var choose in SaveFile.choosesLists)
             {
                 if (choose.blockKey == key)
                 {
                     return choose;
                 }
             }

             isFirstInit = true;
             SaveFile.choosesLists.Add(choosesList);
             SaveJson(PROGRESS_KEY);

             return choosesList;
         }

         private static ChoosesList GetListFromJson(string key)
         {
             ChoosesList choosesList = new ChoosesList();

             foreach (var choose in SaveFile.choosesLists)
             {
                 if (choose.blockKey == key)
                 {
                     choosesList = choose;
                 }
             }

             return choosesList;
         }

         public static bool GetStatusValue(StatusEnum statusEnum)
         {
             int flag = PlayerPrefs.GetInt(statusEnum.ToString(), 0);

             return flag != 0;
         }

         public static void SetStatusValue(StatusEnum statusEnum, bool status)
         {
             int flag = status ? 1 : 0;

             PlayerPrefs.SetInt(statusEnum.ToString(), flag);
         }

         public static int GetIntValue(CountType countType)
         {
             return PlayerPrefs.GetInt(countType.ToString(), 0);
         }


         public static void SetIntValue(CountType countType, int value)
         {
             PlayerPrefs.SetInt(countType.ToString(), value);
         }

         public static int GetIntValue(KillerName countType)
         {
             return PlayerPrefs.GetInt(countType.ToString(), 0);
         }

         public static void SetIntValue(KillerName countType, int value)
         {
             PlayerPrefs.SetInt(countType.ToString(), value);
         }

         public static void ResetAllSaves()
         {
             PlayerPrefs.DeleteAll();

             _saveFile = new SaveFile();
             _chapterNotesFile = new ChapterNotesFile();

             SaveJson(CHAPTER_NOTES_KEY);
             SaveJson(PROGRESS_KEY);
         }

         public static void SaveTypingSpeed(float speed)
         {
             SettingFile.typingSpeed = speed;
             SaveJson(SETTINGS_KEY);
         }

         public static void SaveMusicVolume(float volume)
         {
             SettingFile.musicVolume = volume;
             SaveJson(SETTINGS_KEY);
         }

         public static void SaveAudioVolume(float volume)
         {
             SettingFile.soundVolume = volume;
             SaveJson(SETTINGS_KEY);
         }
         
         public static void SaveBrightnessValue(float value)
         {
             SettingFile.brightnessValue = value;
             SaveJson(SETTINGS_KEY);
         }

         public static float GetAudioVolume() => SettingFile.soundVolume;
         public static float GetMusicVolume() => SettingFile.musicVolume;
         public static float GetTypingSpeed() => SettingFile.typingSpeed;
         public static float GetBrightnessValue() => SettingFile.brightnessValue;
     }

     [Serializable]
     public class SaveFile
     {
         public int currentPart = 0;
         public string currentScene = "scene_0_0";

         public List<ChoosesList> choosesLists = new();
     }

     [Serializable]
     public class SettingFile
     {
         public float musicVolume = 1.0f;
         public float soundVolume = 1.0f;
         public float typingSpeed = 0.5f;
         public float brightnessValue = 0.5f;
     }

     [Serializable]
     public class ChoosesList
     {
         public string blockKey;
         public string[] chooseKeys;
         public bool[] chooseStatus;
         public bool isUseCamera;
     }
     
     [Serializable]
     public class ChapterNotesFile
     {
         public List<NoteChapterPart> chapters = new();
     }

     [Serializable]
     public class NoteChapterPart
     {
         public string name;
         public string text;
     }
 }