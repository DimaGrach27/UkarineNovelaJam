using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneService
{
    private static Coroutine _loadCoroutine;
    
    public static void LoadEndGame()
    {
        if(_loadCoroutine != null) return;

        SaveService.ResetAllSaves();
        
        _loadCoroutine = CoroutineHelper.Inst.StartCoroutine(LoadRoutine());
    }

    private static IEnumerator LoadRoutine()
    {
        FadeService.FadeService.FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndScene");
    }
}