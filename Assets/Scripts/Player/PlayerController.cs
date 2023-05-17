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
    private Vector2 startPos;

    [Header("powerups")]
    [SerializeField] Sprite longSprite;
    [SerializeField] Sprite baseSprite;
    [SerializeField] float powerupDuration;
    [SerializeField] float powerupTime;
    [SerializeField] bool hasPowerup;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprRenderer;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject longCol;
    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start() {
        onTeleport.AddListener(Teleport);
        currentTries = data.tries;
        startPos = transform.position;
    }

    private void Update() {
        if(Time.time > powerupTime && hasPowerup) {
            clearPowerup();
        }
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

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        
        if(otherObj.tag == "powerup_long") {
            Destroy(otherObj);
            sprRenderer.sprite = longSprite;

            hasPowerup = true;
            longCol.SetActive(true);
            col.enabled = false;
            powerupTime = powerupDuration + Time.time;
        }

    }

    private void clearPowerup() {
        hasPowerup = false;
        sprRenderer.sprite = baseSprite;
        longCol.SetActive(false);
        col.enabled = true;
    }
    public void ResetPlayerPos() {
        transform.position = startPos;
        rb.velocity = Vector2.zero;
    }
    public void RemoveTrie() {
        currentTries--;
        
        if(currentTries < 1) {
            gameOver?.Invoke();
        }
    }
}
