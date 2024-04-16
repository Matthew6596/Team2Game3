using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float friction;
    public bool lockXPosition;

    private Vector2 moveInput;
    private Vector2 velocity;

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockXPosition) moveInput = new Vector2(0, moveInput.y);
        velocity += acceleration * moveInput;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        velocity *= friction;

        transform.Translate(velocity * Time.deltaTime);
    }

    public void PlayerMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Enter combat with enemy
        }
        else if (collision.CompareTag("Item"))
        {
            //Add item to player inventory
            gm.playerItems.Add(collision.gameObject.GetComponent<ItemScript>());
        }
    }
}
