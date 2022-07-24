 using System;
 using System.Collections.Generic;
 using System.IO;
 using UnityEngine;

 public static class SaveService
 {
     private const string CAMERA_FILM_LEFT_KEY = "camera_film_left";
     private const string HEALTH_COUNT_KEY = "health_count";

     private static SaveFile _saveFile;
     public static SaveFile SaveFile
     {
         get
         {
             if (_saveFile != null) return _saveFile;
             
             if (!ExistFile)
             {
                 _saveFile = new SaveFile();

                 File.WriteAllText(Path, JsonUtility.ToJson(_saveFile));
             }
             else
             {
                 _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(Path));
             }

             return _saveFile;
         }
     }

     private static bool ExistFile =>  File.Exists(Path);
     private static string Path => Application.persistentDataPath + $"/progress_{Application.productName}.json";
     public static void SaveJson() => File.WriteAllText(Path, JsonUtility.ToJson(SaveFile));
     
     public static void SavePart(int current)
     {
         SaveFile.currentPart = current;
         SaveJson();
     }
     
     public static void SaveScene(string sceneKey)
     {
         SaveFile.currentScene = sceneKey;
         SaveJson();
     }
     

     public static string GetScene => SaveFile.currentScene;
     public static int GetPart => SaveFile.currentPart;

     public static int CameraFilmLeft
     {
         get => PlayerPrefs.GetInt(CAMERA_FILM_LEFT_KEY, 24);
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
         
         if(choosesList.chooseKeys == null) return;
         
         for (int i = 0; i < choosesList.chooseKeys.Length; i++)
         {
             if (choosesList.chooseKeys[i] == choose)
             {
                 choosesList.chooseStatus[i] = true;
                 
                 SetChoosesList(key, choosesList);
             }
         }
     }
     
     public static void SetChoosesList(string key, ChoosesList choosesList)
     {
         ChoosesList chooses = GetListFromJson(key);
         
         chooses.blockKey = choosesList.blockKey;
         chooses.chooseKeys = choosesList.chooseKeys;
         chooses.chooseStatus = choosesList.chooseStatus;
         
         SaveJson();
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
         SaveJson();
         
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
     
     public static void ResetAllSaves()
     {
         PlayerPrefs.DeleteAll();
         
         _saveFile = new SaveFile();

         SaveJson();
     }
 }

 [Serializable]
 public class SaveFile
 {
     public int currentPart = 0;
     public string currentScene = "screen_scene_0";

     public List<ChoosesList> choosesLists = new ();

     public bool bottle;
 }

 [Serializable]
 public class ChoosesList
 {
     public string blockKey;
     public string[] chooseKeys;
     public bool[] chooseStatus;
 }