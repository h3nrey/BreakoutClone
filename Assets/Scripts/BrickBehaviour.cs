using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public class BrickBehaviour : MonoBehaviour
{
    public int life;
    public int currentLife;
    public Tilemap tiles;
    private TileBase tile;
    Vector3Int cellPos;
    Transform parent;

    [SerializeField]
    private bool isMistery;
    [SerializeField]
    GameObject[] powerUps;
    [SerializeField] GameObject particleEmitter;
    [SerializeField] Color particleColor;
    [SerializeField] Animator anim;


    private void Start() {
        parent = transform.parent;
        tiles = transform.GetComponentInParent<Tilemap>();
        cellPos = tiles.WorldToCell(transform.position);
        tile = tiles.GetTile(cellPos);
        currentLife = life;
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "ball") {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        currentLife -= 1;
        anim.SetTrigger("touched");

        if(currentLife < 1) {
            tiles.SetTile(cellPos, null);
            if(parent.childCount <= 1) {
                GameManager.game.LevelWon();
                return;
            }
            if(isMistery)
                GeneratePowerup();

            GameObject emitter = Instantiate(particleEmitter, transform.position, Quaternion.identity);
            ParticleSystem emitterParticles = emitter.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule _main = emitterParticles.main;
            _main.startColor = particleColor;

            emitterParticles.Play();
            Destroy(this.gameObject);
            GameManager.game.playSound(GameSounds.breakBrick);

        }
    }

    private void GeneratePowerup() {
        GameObject powerup = ChoosePowerUp();
        Instantiate(powerup, transform.position, Quaternion.identity);
    }

    private GameObject ChoosePowerUp() {
        int randomIndex = Random.Range(0, powerUps.Length);
        return powerUps[randomIndex];
    }
}
