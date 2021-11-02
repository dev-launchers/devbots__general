using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack Ability", menuName = "Ability Objects/Attack Ability", order = 1)]
public class AttackAbility : BotAbility
{
    public Vector2 dir;
    public float power;
}
