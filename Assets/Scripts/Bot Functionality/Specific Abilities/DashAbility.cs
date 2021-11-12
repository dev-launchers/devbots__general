using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dash Ability", menuName = "Ability Objects/Movement Abilities/Dash", order = 2)]

public class DashAbility : MoveAbility
{
    public Vector2 forceOfMovement;
    public override void Activate(GameObject parent)
    {
        rb = parent.gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = parent.GetComponentInParent<BotSensor>();
        controller = parent.GetComponentInParent<BotController>();
                
        //Use add relative force to rigidbody to thrust bot up and slightly forward. 
        Vector2 appliedForce = new Vector2(forceOfMovement.x * sensor.GetNearestSensedBotDirection(), forceOfMovement.y);
        rb.AddRelativeForce(appliedForce, ForceMode2D.Impulse);
    }
}
