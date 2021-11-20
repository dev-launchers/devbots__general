using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Teleport Ability", menuName = "Ability Objects/Movement Abilities/Teleport", order = 2)]

public class TeleportAbility : MoveAbility
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        rb = parent.GetComponentInParent<Rigidbody2D>();
        sensor = parent.GetComponentInParent<BotSensor>();
        controller = parent.GetComponentInParent<BotController>();
        
        Vector2 enemyPos = sensor.GetNearestSensedBotPosition();
        rb.position = enemyPos + new Vector2(0, 2);
    }
}
