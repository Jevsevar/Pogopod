﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persist : MonoBehaviour {

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
