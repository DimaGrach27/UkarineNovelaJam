 using System;
 using System.IO;
 using UnityEngine;

 public static class SaveService
 {
     private static SaveFile _saveFile;
     private static SaveFile SaveFile
     {
         get
         {
             if(_saveFile == null)
             {
                 if (!ExistFile())
                 {
                     _saveFile = new SaveFile();

                     File.WriteAllText(Path, JsonUtility.ToJson(_saveFile));
                 }
                 else
                 {
                     _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(Path));
                 }
             }

             return _saveFile;
         }
     }

     private static bool ExistFile() =>  File.Exists(Path);
     private static string Path => Application.dataPath + "/jsonSave.json";

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

     public static int GetPart()
     {
         return SaveFile.currentPart;
     }
     public static string GetScene()
     { 
         return SaveFile.currentScene;
     }
     
     public static void ResetAllSaves()
     {
         PlayerPrefs.DeleteAll();
         File.Delete(Path);
     }
 }

 [Serializable]
 public class SaveFile
 {
     public int currentPart = 0;
     public string currentScene = "screen_scene_0";
 }