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
            TakeDamage(other.gameObject);
        }
    }
    
    private void TakeDamage(GameObject ballObj) {
        BallBehaviour ball = ballObj.GetComponent<BallBehaviour>();
        currentLife -= 1;
        anim.SetTrigger("touched");

        if(currentLife < 1 || ball.isExplosive) {
            tiles.SetTile(cellPos, null);

            if(ball.isExplosive) {
                print($"original tile: {cellPos}");
                for(int i = -1; i < 2; i++) {
                    for(int j = -1; j < 2; j++) {
                        if (Mathf.Abs(i) == Mathf.Abs(j)) continue;

                        Vector3Int pos = new Vector3Int(cellPos.x + i, cellPos.y + j, cellPos.z);
                        print($"i: {i} | j: {j}");
                        if(tiles.HasTile(pos)) {
                            tiles.SetTile(new Vector3Int(cellPos.x + i, cellPos.y + j, cellPos.z), null);
                        }
                    }
                }

                print("----- \n");
            }
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
        GameObject powerupInstance = Instantiate(powerup, transform.position, Quaternion.identity) as GameObject;
        GameManager.game.powerupsHolder.Add(powerupInstance);
    }

    private GameObject ChoosePowerUp() {
        int randomIndex = Random.Range(0, powerUps.Length);
        return powerUps[randomIndex];
    }
}
