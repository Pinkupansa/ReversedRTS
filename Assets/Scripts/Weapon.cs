using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] int baseDamage;
    [SerializeField] float baseSpeed;
    [SerializeField] float baseRange;

    public GameObject Prefab { get => prefab; }
    public int BaseDamage { get => baseDamage; }
    public float BaseSpeed { get => baseSpeed; }
    public float BaseRange { get => baseRange; }
}
