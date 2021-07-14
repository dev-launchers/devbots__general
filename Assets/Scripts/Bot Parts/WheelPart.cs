using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MonoBehaviour , IBotPart
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
            if (rb.velocity.x != (targetSpeed)) {
                float increment = (targetSpeed - rb.velocity.x); //Mathf.Lerp(rb.velocity.x, targetSpeed, .5);
                //This needs to be scaled to haappen over time instead of instantly
                rb.velocity += new Vector2(increment, 0);
            }
            //sensor.PlayAudio("Move");
        }
    }

    public void SetState(State state) { 
        //Have the turn handler send this for now then check in Update
    }
}
