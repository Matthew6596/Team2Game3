using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float speed;
    public float leftEndPoint;
    public float rightStartPoint;

    // Update is called once per frame
    void Update()
    {
        //Move background left
        transform.Translate(speed * Time.deltaTime * Vector3.left);
        //If its position goes too far left, teleport it back to the right so it loops
        if (transform.position.x <= leftEndPoint) 
            transform.position = new Vector3(rightStartPoint, transform.position.y, 0);
    }
}
