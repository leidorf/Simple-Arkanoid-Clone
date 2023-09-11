using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;

    public Text CurrentPlayerName;
    public Text BestPlayerStats;

    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private static int bestScore;
    private static string bestPlayer;

    void Awake()
    {
        LoadGameRank();
    }



    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        CurrentPlayerName.text = PlayerDataHandle.Instance.playerName;
        SetBestPlayer();

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandle.Instance.playerScore = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestPlayer();
        GameOverText.SetActive(true);
    }

    private void CheckBestPlayer()
    {
        int CurrentScore = PlayerDataHandle.Instance.playerScore;

        if (CurrentScore > bestScore)
        {
            bestPlayer = PlayerDataHandle.Instance.playerName;
            bestScore = CurrentScore;

            BestPlayerStats.text = $"Best Score  - {bestPlayer}:{bestScore}";

            SaveGameRank(bestPlayer, bestScore);
        }
    }

    private void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            BestPlayerStats.text = "";
        }
        else
        {
            BestPlayerStats.text = $"Best Score - {bestPlayer}:{bestScore}";
        }
    }

    public void SaveGameRank(string bestPlayerName, int bestPlayerScore)
    {
        SaveData data = new SaveData();

        data.theBestPlayer = bestPlayerName;
        data.highestScore = bestPlayerScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.theBestPlayer;
            bestScore = data.highestScore;
        }
    }

    [SerializeField]
    public class SaveData
    {
        public int highestScore;
        public string theBestPlayer;
    }
}
