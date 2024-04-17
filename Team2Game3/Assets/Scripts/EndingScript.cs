using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class EndingScript : MonoBehaviour
{
    static public EndingScript inst;

    public GameObject endUI;
    
    //Score
    public TMP_Text endScoreTxt;

    public Transform player;
    public Transform badShark;
    public Transform fishyWifey;

    public Transform[] playerLocations;
    public Transform[] badLocations;
    public Transform[] goodLocations;

    public float playerSpd;
    public float sharkSpd;
    public float wifeySpd;

    bool began = false;
    bool goodEnding;

    int sequence;

    bool[] timedActionDone;

    Animator wifeyAnim;
    Animator sharkAnim;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        timedActionDone = new bool[3];

        gm = GameManager.gm;

        //Anim
        wifeyAnim = GameObject.Find("wife").gameObject.GetComponent<Animator>();
        sharkAnim = GameObject.Find("shark").gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        endScoreTxt.text = gm.scoreTxt.text;

        if (began)
        {
            if(goodEnding)
                GoodEndSequence();
            else
                BadEndSequence();
        }
    }

    public void BeginEnding(bool goodEnd)
    {
        sequence = 0;
        goodEnding = goodEnd;
        began = true;
    }

    //These update every frame!
    void GoodEndSequence()
    {
        if (sequence >= 0)
        {
            player.position = Vector3.MoveTowards(player.position, playerLocations[0].position, playerSpd*Time.deltaTime);
            StartTimedAction(() => { sequence++; }, 1, 0);
        }
        if (sequence >= 1)
        {
            fishyWifey.position = Vector3.MoveTowards(fishyWifey.position, goodLocations[0].position, wifeySpd*Time.deltaTime);
            StartTimedAction(() => {
                //Instatiate heart and show end button
                endUI.SetActive(true);
            }, 1, 1);
        }
    }
    void BadEndSequence()
    {
        if (sequence >= 0)
        {
            player.position = Vector3.MoveTowards(player.position, playerLocations[0].position, playerSpd * Time.deltaTime);
            StartTimedAction(() => { sequence++; }, 1, 0);
        }
        if (sequence >= 1)
        {
            fishyWifey.position = Vector3.MoveTowards(fishyWifey.position, badLocations[0].position, wifeySpd * Time.deltaTime);
            wifeyAnim.SetTrigger("badEnding");
            StartTimedAction(() => {sequence++; }, 1, 1);
        }
        if (sequence >= 2)
        {
            badShark.position = Vector3.MoveTowards(badShark.position, badLocations[1].position, sharkSpd * Time.deltaTime);
            sharkAnim.SetBool("isAttacking", true);
            StartTimedAction(() => {
                //Instatiate heart and show end button
                endUI.SetActive(true);
            }, 1, 2);
        }
    }

    void StartTimedAction(Action act, float delay, int id)
    {
        if (!timedActionDone[id])
        {
            StartCoroutine(timedAction(act, delay));
            timedActionDone[id] = true;
        }
    }

    IEnumerator timedAction(Action act, float delay)
    {
        yield return new WaitForSeconds(delay);
        act.Invoke();
    }
}
