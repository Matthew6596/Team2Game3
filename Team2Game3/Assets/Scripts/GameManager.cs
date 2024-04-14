using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject Instance;
    public static GameManager gm;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject;
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Private Variables
    [SerializeField]
    private float timeToBeatGame;

    //Public Variables
    public List<ItemScript> playerItems = new();
    public bool inCombat;
    public int score;

    //Things to remember when change scene
    public float gameProgress;
    public Vector2 playerPosition;

    private void Update()
    {
        if (!inCombat)
        {
            gameProgress += Time.deltaTime;
            if (gameProgress > timeToBeatGame)
            {
                //End game
            }
        }
    }

}
