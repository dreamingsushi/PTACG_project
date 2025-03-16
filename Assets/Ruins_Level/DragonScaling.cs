using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dragon Parameters", menuName = "ScriptableObjects/Boss Scaling", order = 1)]
public class DragonScaling : ScriptableObject
{
    public float fireballDamage;
    public float flamesDamage;
    public float clawDamage;
    public float punchDamage;
    public float flamethrowerDamage;
    public float meteorDamage;
    public float takenDamage;
}
