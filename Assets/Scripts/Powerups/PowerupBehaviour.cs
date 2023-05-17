using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PowerupBehaviour : MonoBehaviour
{
    [Expandable] public Powerup data;
    [SerializeField] private SpriteRenderer sprRenderer;
    private void OnEnable() {
        sprRenderer.sprite = data.sprite;
    }

    void Update() {
        transform.position -= Vector3.up * data.fallSpeed * Time.deltaTime;
    }
}
