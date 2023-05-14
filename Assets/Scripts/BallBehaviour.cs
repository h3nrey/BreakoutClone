using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 launchDir;
    [SerializeField] float launchForce;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float reflectX, reflexctY;

    private void Start() {
        rb.AddForce(launchDir);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "ball") {
            rb.AddForce(rb.velocity * reflectX);
        }
    }
}
