using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LoadGameRank : MonoBehaviour
{

    public Text bestPlayerName;

    private static int bestScore;
    private static string bestPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        LoadGameStats();   
    }

    private void SetBestPlayer()
    {
        if(bestPlayer == null && bestScore == 0)
        {
            bestPlayerName.text = "";
        }
        else
        {
            bestPlayerName.text = $"Best Score - {bestPlayer}:{bestScore}";
        }
    }

    public void LoadGameStats()
    {
        string path = Application.persistentDataPath + "/savefile.json" ;
        if(File.Exists(path) )
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.theBestPlayer;
            bestScore = data.highestScore;
            SetBestPlayer();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highestScore;
        public string theBestPlayer;
    }
}
