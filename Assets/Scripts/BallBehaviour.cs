using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] int endLayer;
    private Vector2 startPos;
    private void Start() {
        startPos = transform.position;
        Coroutines.DoAfter(InitialLaunch, launchDelay, this);
    }

    private int GetLaunchXForce() {
        int dirX = Random.Range(-1, 1);
        if (dirX == 0) dirX = 1;
        return dirX;
        
    }

    private void OnCollisionEnter2D(Collision2D otherCol) {
        GameObject other = otherCol.gameObject;
        if (other.tag != "ball") {
            rb.AddForce(rb.velocity * reflectX);
        }

        if(other.layer == endLayer) {
            RestartBall();
            PlayerController.instance.RemoveTrie();
        }
    }

    private void InitialLaunch() {
        float senseX = GetLaunchXForce();
        Vector2 launchDir = new Vector2(launchX * senseX, launchY);
        rb.AddForce(launchDir);
    }
    private void RestartBall() {
        rb.velocity = Vector2.zero;
        transform.position = startPos;
        Coroutines.DoAfter(InitialLaunch, launchDelay, this);
    }
}
