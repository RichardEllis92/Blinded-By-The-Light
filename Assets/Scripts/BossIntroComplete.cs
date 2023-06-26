using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroComplete : MonoBehaviour
{
    public static BossIntroComplete Instance;
    public bool bossIntroComplete;

    private void Awake()
    {
        Instance = this;

    }

}
