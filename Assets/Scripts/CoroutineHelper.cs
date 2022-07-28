﻿using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper Inst {get; private set; }

    private void Awake()
    {
        if(Inst != null) return;
        
        Inst = this;
    }
}