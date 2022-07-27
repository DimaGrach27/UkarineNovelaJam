using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        if(!GameModel.GameWasInit) 
            DontDestroyOnLoad(this);
        else
        {
            Destroy(gameObject);
        }
    }
}