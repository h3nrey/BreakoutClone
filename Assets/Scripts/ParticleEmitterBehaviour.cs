using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitterBehaviour : MonoBehaviour
{
    [SerializeField] float dieCooldown;

    private void Start() {
        Invoke("Die", dieCooldown);
    }

    private void Die() {
        Destroy(this.gameObject);
    }
}
