﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float panelSpawnTimer = 0f;
    private float panelSpawnInternal = 2f;
    private float gameTime = 0;
    public int Score { get; private set; }
    private int nextLevelScore = 30;
    public float ScrollingSpeed = 1f;

    public int CurrentPatternIndex { get; private set; }

    [SerializeField] PatternPanelManager panelManager = null;
    [SerializeField] SpriteRenderer charaRender;
    [SerializeField] List<Sprite> charaPatterns;
    [SerializeField] GameObject gameOverTextObj;

    bool isEnableRespawn = true;
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
        if (panelSpawnTimer > panelSpawnInternal && isEnableRespawn)
        {
            isEnableRespawn = false;
            panelManager?.SpawnPatternPanel();
            GameJam.UI.IngameUIManager.I.ResetButtons();
            panelSpawnTimer = 0;
        }

        if (Score > nextLevelScore)
        {
            ChangeScrollingSpeed(ScrollingSpeed + 0.5f);
            nextLevelScore += 20;
        }
    }

    public void AddScore(int score)
    {
        isEnableRespawn = true;
        Score += score;
        IngameUIManager.Instance.AddScore(score);
    }

    public void ChangeScrollingSpeed(float speed)
    {
        ScrollingSpeed = speed;
    }

    /// <summary>
    /// 選択肢ボタン選択コールバック
    /// </summary>
    /// <param name="_index"></param>
    public void SetCurrentIndex(int _index)
    {
        CurrentPatternIndex = _index;
        var animator = charaRender.GetComponent<Animator>();
        animator.enabled = false;
        charaRender.sprite = charaPatterns[_index];
    }

    public void EnableRunning()
    {
        var animator = charaRender.GetComponent<Animator>();
        animator.enabled = true;
        CurrentPatternIndex = -1;
    }

    public void GameOver()
    {
        gameOverTextObj.SetActive(true);
        Time.timeScale = 0;
    }
}
