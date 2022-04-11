using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float LevelTine = 40.0f;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
            return;
        if (LevelTine > 0)
        {
            LevelTine -= Time.deltaTime;
        }
        else
        {
            GameManager.isGameEnded = true;
        }

    }
}
