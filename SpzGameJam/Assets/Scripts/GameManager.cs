using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float panelSpawnTimer = 0f;
    private float panelSpawnInternal = 2f;
    private float gameTime = 0;
    public int Score { get; private set; }
    private int nextLevelScore = 30;
    public float ScrollingSpeed = 1f;

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

        if (Score > nextLevelScore)
        {
            ChangeScrollingSpeed(ScrollingSpeed + 0.5f);
            nextLevelScore += 20;
        }
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public void ChangeScrollingSpeed(float speed)
    {
        ScrollingSpeed = speed;
    }

    /// <summary>
    /// 選択肢ボタン選択コールバック
    /// </summary>
    /// <param name="_index"></param>
    public void SetCurrentIndex( int _index )
    {

    }
}
