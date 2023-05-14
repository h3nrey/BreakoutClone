using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickBehaviour : MonoBehaviour
{
    public int life;
    public int currentLife;
    public Tilemap tiles;
    private TileBase tile;
    Vector3Int cellPos;


    private void Start() {
        tiles = transform.GetComponentInParent<Tilemap>();
        cellPos = tiles.WorldToCell(transform.position);
        tile = tiles.GetTile(cellPos);
        currentLife = life;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "ball") {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        currentLife -= 1;

        if(currentLife < 1) {
            tiles.SetTile(cellPos, null);
            Destroy(this.gameObject);
        }
    }
}
