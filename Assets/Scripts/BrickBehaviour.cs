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
    Transform parent;


    private void Start() {
        parent = transform.parent;
        tiles = transform.GetComponentInParent<Tilemap>();
        cellPos = tiles.WorldToCell(transform.position);
        tile = tiles.GetTile(cellPos);
        currentLife = life;
    }

    private void Update() {
        print("childs: " + parent.childCount);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "ball") {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        currentLife -= 1;

        if(currentLife < 1) {
            if(parent.childCount <= 1) {
                GameManager.game.LevelWon();
            }
            tiles.SetTile(cellPos, null);
            Destroy(this.gameObject);

        }
    }
}
