#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.CompilerServices;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public Text bestPlayerStats;
    private string bestPlayerName;
    private int highestScore;

    public Button startButton;
    public InputField playerNameInput;

    private void Awake()
    {
        WriteBestStats();    
    }

    void Start()
    {
        startButton.interactable = false;
        playerNameInput.onValueChanged.AddListener(ShowStartButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerName()
    {
        PlayerDataHandle.Instance.playerName = playerNameInput.text;
    }

    private void ShowStartButton(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadBestStats()
    {
       
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data.theBestPlayer;
            highestScore = data.highestScore;
        }
        else
        {

        }
    }

    private void WriteBestStats()
    {
        LoadBestStats();
        bestPlayerStats.text = $"Best Score  - {bestPlayerName}:{highestScore}";
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
    [SerializeField]
    public class SaveData
    {
        public int highestScore;
        public string theBestPlayer;
    }
