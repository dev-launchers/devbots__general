using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wheel Ability", menuName = "Ability Objects/Movement Abilities/Wheel", order = 2)]

public class WheelAbility : MoveAbility
{
    [SerializeField] private float moveSpeed = default(float);
    [SerializeField] private float accelerationMagnitude = default(float);

    public override void Activate(GameObject parent)
    {
        rb = parent.gameObject.GetComponentInParent<Rigidbody2D>();
                sensor = parent.GetComponentInParent<BotSensor>();
                controller = parent.GetComponentInParent<BotController>();
        int enemyDirection = sensor.GetNearestSensedBotDirection();
        int currentDirection = enemyDirection;
        
        
        float mySpeed = rb.velocity.x;
       
        if (mySpeed <= moveSpeed && mySpeed >= -moveSpeed) {
            rb.AddRelativeForce(new Vector2(accelerationMagnitude*currentDirection, 0), ForceMode2D.Impulse);
            
        }
    }
}
