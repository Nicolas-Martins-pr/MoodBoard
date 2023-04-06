using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data combat", menuName = "Data combat")]
public class CombatData : ScriptableObject
{
    public float timer;
    public float timerIncreaseBlackness;
    public float badAimValue;
    public float niceAimValue;
}
