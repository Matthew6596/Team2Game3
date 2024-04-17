using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    public static GameObject Instance;
    public static GameManager gm;
    private void Awake()
    {
        /*if (Instance == null)
        {
            Instance = gameObject;
            gm = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }*/
        Instance = gameObject;
        gm = this;
    }

    //Private Variables
    [Header("Options")]
    [SerializeField]
    private float timeToBeatGame;
    [SerializeField]
    private int goodEndingScoreMinimum;
    public int playerHp;
    public int playerMaxHp;
    public int playerMana;
    public int playerMaxMana;
    public int playerAttack;
    public Transform progressBar;
    public Transform canvas;
    public GameObject enemyObj;
    public GameObject combatScene;
    public GameObject swimScene;

    //Public Variables
    [Space]
    [Header("For Viewing Only")]
    public List<ItemScript> playerItems = new();
    public bool inCombat;
    public int score;
    public EnemyScript enemy;
    public TMP_Text scoreTxt;

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
        else
        {
            if(playerHp <= 0)
            {
                SceneManager.LoadScene("GameOverScene");
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

    public void GetEnemy(Collider2D collision)
    {
        enemyObj = collision.gameObject;
    }

    public void IncrementScore(int amt)
    {
        score += amt;
        string txtScore = score.ToString();
        if (amt < 10) { txtScore = "000" + score; }
        else if (amt < 100) { txtScore = "00" + score; }
        else if (amt < 1000) { txtScore = "0" + score; }
        scoreTxt.text = txtScore;
    }

    public void LoadCombat()
    {
        combatScene.SetActive(true);
        swimScene.SetActive(false);
    }
    public void LoadSwimming()
    {
        combatScene.SetActive(false);
        swimScene.SetActive(true);
    }
}
