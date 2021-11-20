using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : BotPart
{
    [SerializeField] private float moveSpeed = default(float);
    [SerializeField] private float accelerationMagnitude = default(float);

    private int currentDirection = 0;

    private Rigidbody2D rb;
    private BotSensor sensor;

    [SerializeField] private bool isRunning;

    public void Start() {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
    }


    public void MoveStep() {
        if (isRunning) {
            // Should we change direction? (can only change directions once every cooldown period)
            if (!IsPartCoolingDown()) {
                int enemyDirection = sensor.GetNearestSensedBotDirection();
                currentDirection = enemyDirection;
                ResetCooldownTimer();
            }

            float mySpeed = rb.velocity.x;
            if (mySpeed <= moveSpeed && mySpeed >= -moveSpeed) {
                rb.AddRelativeForce(new Vector2(accelerationMagnitude*currentDirection, 0), ForceMode2D.Force);
                
            }
        }

        //base.MoveStep();
    }

    public override void SetState(State state) {
        isRunning = state.isActive;
    }

    public override void BotPartUpdate()
    {
        MoveStep();
    }
}
