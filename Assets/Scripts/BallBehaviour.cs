using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
public class BallBehaviour : MonoBehaviour
{
    //[SerializeField] Vector2 launchDir;
    [SerializeField] float launchDelay;
    [SerializeField] float launchY;
    [SerializeField] float launchX;
    [SerializeField] float launchForce;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float reflectX, reflexctY;
    [SerializeField] float maxSpeed, minSpeed;

    [SerializeField] int endLayer;
    private Vector2 startPos;
    public UnityEvent onBallEnd;

    [SerializeField] TrailRenderer trail;
    private void Start() {
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
            rb.AddForce(rb.velocity * reflectX);
            GameManager.game.playSound(GameSounds.reflect);
        }

        if(other.layer == endLayer) {
            //RestartBall();
            onBallEnd?.Invoke();
            PlayerController.instance.RemoveTrie();
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
        Coroutines.DoAfter(InitialLaunch, launchDelay, this);
    }
}
