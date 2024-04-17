using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [Header("Options")]
    [SerializeField]
    private float timeToBeatGame;
    [SerializeField]
    private int goodEndingScoreMinimum;
    public int playerHp;
    public int playerMana;
    public int playerAttack;
    public MagicScript[] magicOptions;
    public Transform progressBar;
    public Transform canvas;

    //Public Variables
    [Space]
    [Header("For Viewing Only")]
    public List<ItemScript> playerItems = new();
    public bool inCombat;
    public int score;
    public EnemyScript enemy;

    //Things to remember when change scene
    public float gameProgress;
    public Vector2 playerPosition;

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            progressBar.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!inCombat)
        {
            gameProgress += Time.deltaTime;
            int numBarsToFill = (int)(gameProgress / timeToBeatGame * 20f);
            for(int i=0; i<numBarsToFill; i++)
            {
                progressBar.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (gameProgress > timeToBeatGame)
            {
                EndGame();
            }
        }
    }
    
    void EndGame()
    {
        inCombat = true;
        EndingScript.inst.BeginEnding(score >= goodEndingScoreMinimum);
        StartCoroutine(HideSwimUI());
    }

    float uiSpd = 5f;
    IEnumerator HideSwimUI()
    {
        while (canvas.localPosition.y < 100)
        {
            canvas.Translate(Time.deltaTime * uiSpd * Vector3.Distance(canvas.localPosition, Vector3.up * 100) * Vector3.up);
            yield return null;
        }
    }
}
