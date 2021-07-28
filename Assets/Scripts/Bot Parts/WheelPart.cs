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
        isRunning = true;
    }

    public void Update() {
        MoveStep();

    }

    public void MoveStep() {
        if (isRunning) {
            int enemyDirection = sensor.GetNearestSensedBotDirection();
            float targetSpeed = enemyDirection * moveSpeed;
            //changed if statement to check if velocity is lower than target speed and is now moving the bot using add force rather than explicitly changing the velocity
            if (Mathf.Abs(rb.velocity.x) <= (targetSpeed)) {
                //float increment = (targetSpeed - rb.velocity.x); //Mathf.Lerp(rb.velocity.x, targetSpeed, .5);
                //This needs to be scaled to haappen over time instead of instantly
                //rb.velocity += new Vector2(increment, 0);
                rb.AddRelativeForce(new Vector2(targetSpeed, 0),ForceMode2D.Force);
            }
            //sensor.PlayAudio("Move");
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }
}
