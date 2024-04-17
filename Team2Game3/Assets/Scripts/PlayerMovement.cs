using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        //Input/Player movement
        if (lockXPosition) moveInput = new Vector2(0, moveInput.y);
        velocity += acceleration * moveInput;

        //Player top/bottom bounds
        if (transform.position.y > 3.5f)
        {
            velocity += acceleration*(transform.position.y-3)*Vector2.down;
        }
        else if(transform.position.y < -3.5f)
        {
            velocity += acceleration*(-transform.position.y - 3)*Vector2.up;
        }
        //

        //Max speed and friction
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        velocity *= friction;

        //Move
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
            gm.inCombat = true;
            gm.enemy = collision.gameObject.GetComponent<EnemyScript>();
            gm.GetEnemy(collision);
            SceneManager.LoadScene("CombatScene"); //TEMP <<< eventually combat scene and swim scene will be same, but in different groups
        }
        else if (collision.CompareTag("Item"))
        {
            //Add item to player inventory
            gm.playerItems.Add(collision.gameObject.GetComponent<ItemScript>());
            collision.gameObject.SetActive(false); //Physical item disappears
            collision.gameObject.transform.SetParent(gm.gameObject.transform); //Item persist through scenes
        }
    }

}
