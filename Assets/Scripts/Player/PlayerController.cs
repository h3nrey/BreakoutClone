using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Expandable] public Player data;
    public Vector2 input;

    [Header("move")]
    [SerializeField] float limitX;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;


    public void GetInput(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        rb.velocity = new Vector2(input.x * Time.deltaTime * data.speed, rb.velocity.y);

        if (rb.position.x > limitX) {
            rb.position = new Vector2(limitX, rb.position.y);
        } else if(rb.position.x < -limitX) {
            rb.position = new Vector2(-limitX, rb.position.y);
        }
    }
}
