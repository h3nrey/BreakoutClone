using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using NaughtyAttributes;
public class BallBehaviour : MonoBehaviour
{
    //[SerializeField] Vector2 launchDir;

    [Header("Launch")]
    [SerializeField] float launchDelay;
    [SerializeField] float launchY;
    [SerializeField] float launchX;
    [SerializeField] float launchForce;
    
    [Header("Reflect")]
    [SerializeField] float reflectX, reflexctY, paddleReflect;

    [Header("Speed")]
    [ReadOnly] 
    public float maxSpeed, minSpeed;
    [SerializeField] float baseMinSpeed, baseMaxSpeed, fastSpeed;

    [Header("Trail")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] Color baseTrailColor;

    [Header("Explosive")]
    public bool isExplosive;
    [SerializeField] Sprite explosiveSprite;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprRenderer;
    [SerializeField] Sprite baseSprite;
    [SerializeField] int endLayer;
    private Vector2 startPos;
    public UnityEvent onBallEnd;


    private void Start() {
        minSpeed = baseMinSpeed;
        maxSpeed = baseMaxSpeed;
        startPos = transform.position;
        Coroutines.DoAfter(InitialLaunch, launchDelay, this);
        onBallEnd.AddListener(RestartBall);
    }

    private int GetLaunchXForce() {
        int dirX = Random.Range(-1, 1);
        if (dirX == 0) dirX = 1;
        return dirX;
        
    }

    private void FixedUpdate() {
        float ballSpeed = rb.velocity.magnitude;
        if(rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        } 
        if (rb.velocity.magnitude < minSpeed) {
            Vector2 ballvel = rb.velocity.normalized * minSpeed;
            rb.velocity = ballvel;
        }
    }
    private void OnCollisionEnter2D(Collision2D otherCol) {
        GameObject other = otherCol.gameObject;
        if (other.tag != "ball") {
            rb.AddForce(rb.velocity * reflectX, ForceMode2D.Impulse);

            if(other.CompareTag("Player")) {
                rb.AddForce(Vector2.up * paddleReflect, ForceMode2D.Impulse);
            }
            GameManager.game.playSound(GameSounds.reflect);
        }

        if(other.layer == endLayer) {
            //RestartBall();
            onBallEnd?.Invoke();
            PlayerController.instance.RemoveTrie();
            GameManager.game.playSound(GameSounds.death);
            GameManager.game.CleanPowerups();
        }
    }

    private void InitialLaunch() {
        float senseX = GetLaunchXForce();
        Vector2 launchDir = new Vector2(launchX * senseX, launchY);
        rb.AddForce(launchDir);
    }
    private void RestartBall() {
        trail.Clear();
        rb.velocity = Vector2.zero;
        transform.position = startPos;
        ClearSpeed();
        Coroutines.DoAfter(InitialLaunch, launchDelay, this);
    }
    
    public void SpeedUp() {
        minSpeed = fastSpeed;
        maxSpeed = fastSpeed;
        trail.material.color = Color.white;
    }
    private void ClearSpeed() {
        minSpeed = baseMinSpeed;
        maxSpeed = baseMaxSpeed;
        trail.material.color = baseTrailColor;
    }

    public void BecameExplosive() {
        isExplosive = true;
        sprRenderer.sprite = explosiveSprite;
        anim.SetBool("isExplosive", true);
    }


    private void ClearExplosive() {
        isExplosive = false;
        sprRenderer.sprite = baseSprite;
        anim.SetBool("isExplosive", false);
    }
    public void ClearBall() {
        ClearExplosive();
        ClearSpeed();
    }


}
