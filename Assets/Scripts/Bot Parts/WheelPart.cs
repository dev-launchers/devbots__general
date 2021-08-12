using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : BotPart
{
    [SerializeField] private float moveSpeed = default(float);
    [SerializeField] private float accelerationMagnitude = default(float);

    private Rigidbody2D rb;
    private BotSensor sensor;

    [SerializeField] private bool isRunning;

    public void Start() {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
    }

    public void Update() {
        MoveStep();

    }

    public void MoveStep() {
        if (isRunning) {
            int enemyDirection = sensor.GetNearestSensedBotDirection();

            float mySpeed = rb.velocity.x;
            if (mySpeed < moveSpeed && mySpeed > -moveSpeed) {
                rb.AddRelativeForce(new Vector2(accelerationMagnitude*enemyDirection, 0), ForceMode2D.Force);
            }
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }
}
