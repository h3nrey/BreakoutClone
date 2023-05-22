using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="powerup")]
public class Powerup : ScriptableObject
{
    public string tag;
    public float fallSpeed;
    public Sprite sprite;
}
