using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PowerupBehaviour : MonoBehaviour
{
    public Powerup[] powerups;
    [SerializeField] private SpriteRenderer sprRenderer;
    float speed = 0;
    private void OnEnable() {
        int randomIndex = Random.Range(0, powerups.Length);
        Powerup data = powerups[randomIndex];

        transform.tag = data.tag;
        sprRenderer.sprite = data.sprite;
        speed = data.fallSpeed;
    }

    void Update() {
        transform.position -= Vector3.up * speed * Time.deltaTime;
    }
}
