using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed;

    [Header("Enemy Stuff")]
    public bool targetPlayer;
    public float verticalSpeed;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (targetPlayer)
        {
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = speed * Time.deltaTime * Vector3.left;
        if (targetPlayer)
        {
            bool playerAbove = (player.transform.position.y > transform.position.y);
            translation += Vector3.up*((playerAbove) ? (verticalSpeed) : (-verticalSpeed));
        }
        transform.Translate(translation);
    }
}
