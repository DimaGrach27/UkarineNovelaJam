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
         private const string PROGRESS_KEY = "progress";
         private const string CHAPTER_NOTES_KEY = "chapter_notes";
         private const string SETTINGS_KEY = "settings";
         private const string STATUSES_KEY = "statuses";

         private static StatusFlagFile _statusFlagFile;
         private static StatusFlagFile StatusFlagFile
         {
             get
             {
                 if (_statusFlagFile != null) return _statusFlagFile;

                 if (!ExistFile(STATUSES_KEY))
                 {
                     _statusFlagFile = new StatusFlagFile
                     {
                         statuses = new int[Enum.GetNames(typeof(StatusEnum)).Length],
                         killersValue = new int[Enum.GetNames(typeof(KillerName)).Length],
                         countValue = new int[Enum.GetNames(typeof(CountType)).Length]
                     };

                     File.WriteAllText(Path(STATUSES_KEY), JsonUtility.ToJson(_statusFlagFile));
                 }
                 else
                 {
                     _statusFlagFile = JsonUtility.FromJson<StatusFlagFile>(File.ReadAllText(Path(STATUSES_KEY)));
                 }

                 return _statusFlagFile;
             }
         }
         
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
                 
                 case STATUSES_KEY:
                     json = StatusFlagFile;
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
             get => SaveFile.filmCount;
             set
             {
                 SaveFile.filmCount = value;
                 SaveJson(PROGRESS_KEY);
             }
         }

         public static int HealthCount
         {
             get => SaveFile.healthCount;
             set
             {
                 SaveFile.healthCount = value;
                 SaveJson(PROGRESS_KEY);
             }
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
             return (BgEnum)SaveFile.currentBg;
         }

         public static void SetCurrentBg(BgEnum bgEnum)
         {
             int bg = (int)bgEnum;
             SaveFile.currentBg = bg;
             
             SaveJson(PROGRESS_KEY);
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
             int flag = StatusFlagFile.statuses[(int)statusEnum];
             return flag == 1;
         }

         public static void SetStatusValue(StatusEnum statusEnum, bool status)
         {
             StatusFlagFile.statuses[(int)statusEnum] = status ? 1 : 0;
             SaveJson(STATUSES_KEY);
         }

         public static int GetIntValue(CountType countType)
         {
             return StatusFlagFile.countValue[(int)countType];
         }
         
         public static void SetIntValue(CountType countType, int value)
         {
             StatusFlagFile.countValue[(int)countType] = value;
             SaveJson(STATUSES_KEY);
         }

         public static int GetIntValue(KillerName countType)
         {
             return StatusFlagFile.killersValue[(int)countType];
         }

         public static void SetIntValue(KillerName countType, int value)
         {
             StatusFlagFile.killersValue[(int)countType] = value;
             SaveJson(STATUSES_KEY);
         }

         public static void ResetAllSaves()
         {
             PlayerPrefs.DeleteAll();

             _saveFile = new SaveFile();
             _chapterNotesFile = new ChapterNotesFile();

             SaveJson(CHAPTER_NOTES_KEY);
             SaveJson(PROGRESS_KEY);
         }
         
         public static float MusicVolume
         {
             get => SettingFile.musicVolume;
             set
             {
                 SettingFile.musicVolume = value;
                 SaveJson(SETTINGS_KEY);
             }
         }
         
         public static float AudioVolume
         {
             get => SettingFile.soundVolume;
             set
             {
                 SettingFile.soundVolume = value;
                 SaveJson(SETTINGS_KEY);
             }
         }
         
         public static float TypingSpeed
         {
             get => SettingFile.typingSpeed;
             set
             {
                 SettingFile.typingSpeed = value;
                 SaveJson(SETTINGS_KEY);
             }
         }
         
         public static float BrightnessValue
         {
             get => SettingFile.brightnessValue;
             set
             {
                 SettingFile.brightnessValue = value;
                 SaveJson(SETTINGS_KEY);
             }
         }
         
         public static bool BrightnessStatus
         {
             get => SettingFile.isBrightnessWasChange;
             set
             {
                 SettingFile.isBrightnessWasChange = value;
                 SaveJson(SETTINGS_KEY);
             }
         }

         public static void SaveGame(int index)
         {
             string pathProgress = Path($"{PROGRESS_KEY}_{index}_save");
             string pathStatuses = Path($"{STATUSES_KEY}_{index}_save");
             
             File.WriteAllText(pathProgress, JsonUtility.ToJson(SaveFile));
             File.WriteAllText(pathStatuses, JsonUtility.ToJson(StatusFlagFile));
         }

         public static void GetSaveGame(int index)
         {
             string pathProgress = Path($"{PROGRESS_KEY}_{index}_save");
             string pathStatuses = Path($"{STATUSES_KEY}_{index}_save");
             
             _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(pathProgress));
             _statusFlagFile = JsonUtility.FromJson<StatusFlagFile>(File.ReadAllText(pathStatuses));
             
             SaveJson(PROGRESS_KEY);
             SaveJson(STATUSES_KEY);
         }
     }

     [Serializable]
     public class SaveFile
     {
         public int filmCount = 10;
         public int healthCount = GlobalConstant.MAX_HEALTH;
         public int currentBg = -1;
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
         public bool isBrightnessWasChange = false;
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

     [Serializable]
     public class StatusFlagFile
     {
         public int[] statuses;
         public int[] killersValue;
         public int[] countValue;
     }
 }