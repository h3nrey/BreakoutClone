using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Expandable] public Player data;

    #region Variables
    public Vector2 input;

    [Header("Tries")]
    [ReadOnly] public int currentTries;
    public UnityEvent gameOver;

    [Header("Events")]
    public UnityEvent onTeleport;

    [Header("move")]
    [SerializeField] float limitX;
    private Vector2 pos;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start() {
        onTeleport.AddListener(Teleport);
        currentTries = data.tries;
    }
    private void FixedUpdate() {
        Move();
        pos = rb.position;
    }

    #region Input
    public void GetInput(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }
    public void OnTeleport(InputAction.CallbackContext context) {
        if(context.started) {
            onTeleport?.Invoke();
        }
    }
    #endregion

    #region Movement
    private void Move() {
        rb.velocity = new Vector2(input.x * Time.deltaTime * data.speed, rb.velocity.y);

        if (rb.position.x > limitX) {
            rb.position = new Vector2(limitX, rb.position.y);
        } else if(rb.position.x < -limitX) {
            rb.position = new Vector2(-limitX, rb.position.y);
        }
    }

    private void Teleport() {
        rb.position = new Vector2(pos.x, -pos.y);
    }
    #endregion

    public void RemoveTrie() {
        currentTries--;
        
        if(currentTries < 1) {
            gameOver?.Invoke();
        }
    }
}
