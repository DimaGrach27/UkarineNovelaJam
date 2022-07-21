using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper Inst {get; private set; }

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }
}