 using System;
 using System.IO;
 using UnityEngine;

 public static class SaveService
 {
     private const string CAMERA_FILM_LEFT_KEY = "camera_film_left";
     
     private static SaveFile _saveFile;
     private static SaveFile SaveFile
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

     public static void SavePart(int current)
     {
         SaveFile.currentPart = current;
         File.WriteAllText(Path, JsonUtility.ToJson(SaveFile));
     }
     
     public static void SaveScene(string sceneKey)
     {
         SaveFile.currentScene = sceneKey;
         File.WriteAllText(Path, JsonUtility.ToJson(SaveFile));
     }

     public static string GetScene => SaveFile.currentScene;
     public static int GetPart => SaveFile.currentPart;

     public static int CameraFilmLeft
     {
         get => PlayerPrefs.GetInt(CAMERA_FILM_LEFT_KEY, 24);

         set => PlayerPrefs.SetInt(CAMERA_FILM_LEFT_KEY, value);
     }
     
     public static void ResetAllSaves()
     {
         PlayerPrefs.DeleteAll();
         
         SaveFile.currentPart = 0;
         SaveFile.currentScene = "screen_scene_0";
     }
 }

 [Serializable]
 public class SaveFile
 {
     public int currentPart = 0;
     public string currentScene = "screen_scene_0";
 }