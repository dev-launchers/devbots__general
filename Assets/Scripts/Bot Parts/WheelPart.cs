using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : BotPart
{
    [SerializeField] private float moveSpeed = default(float);

    private Rigidbody2D rb;
    private BotSensor sensor;

    private bool isRunning;

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
            float targetSpeed = enemyDirection * moveSpeed;
            if (rb.velocity.x != (targetSpeed)) {
                float increment = (targetSpeed - rb.velocity.x); //Mathf.Lerp(rb.velocity.x, targetSpeed, .5);
                //This needs to be scaled to haappen over time instead of instantly
                rb.velocity += new Vector2(increment, 0);
            }
            //sensor.PlayAudio("Move");
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }
}
