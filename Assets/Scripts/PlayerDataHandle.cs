using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    public static PlayerDataHandle Instance;

    public int playerScore;
    public string playerName;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
