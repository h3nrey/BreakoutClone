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
    [SerializeField] Animator anim;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject longCol;

    [Header("Ball")]
    [SerializeField] BallBehaviour ball;
    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentTries = data.tries;
        startPos = transform.position;
    }

    private void Update() {

        if(input.x > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        } else if(input.x < 1) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        anim.SetFloat("velocity", Mathf.Abs(input.x));

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
        anim.SetTrigger("move");
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
    #endregion

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        
        if(otherObj.tag.StartsWith("powerup")) {
            Destroy(otherObj);
            HandlePowerup(otherObj.tag);
        }
    }

    private void HandlePowerup(string tag) {
        clearPowerup();
        switch(tag) {
            case "powerup_short":
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;

            case "powerup_long":
                sprRenderer.sprite = longSprite;

                longCol.SetActive(true);
                col.enabled = false;
                break;
            case "powerup_fast":
                ball.SpeedUp();
                break;

            case "powerup_explosiveball":
                ball.BecameExplosive();
                break;

        }
        hasPowerup = true;
        GameManager.game.playSound(GameSounds.powerup);
        powerupTime = powerupDuration + Time.time;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject otherObj = other.gameObject;

        if (otherObj.tag == "ball") {
            anim.SetTrigger("shake");
        }

    }

    private void clearPowerup() {
        transform.localScale = Vector3.one;
        hasPowerup = false;
        sprRenderer.sprite = baseSprite;
        longCol.SetActive(false);
        col.enabled = true;
        ball.ClearBall();
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
