﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float panelSpawnTimer = 0f;
    private float panelSpawnInternal = 2f;
    private float gameTime = 0;

    [SerializeField] PatternPanelManager panelManager = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {

    }

    void Update()
    {
        panelSpawnTimer += Time.deltaTime;
        if (panelSpawnTimer > panelSpawnInternal)
        {
            panelManager?.SpawnPatternPanel();
            panelSpawnTimer = 0;
        }
    }

    /// <summary>
    /// 選択肢ボタン選択コールバック
    /// </summary>
    /// <param name="_index"></param>
    public void SetCurrentIndex( int _index )
    {

    }
}
