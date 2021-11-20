using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPart : BotPart
{
    //[SerializeField] private float teleportDistance = default(float);

    private Rigidbody2D rb;
    private BotSensor sensor;
    private BotController controller;
    [SerializeField] private bool isRunning;

    public void Start() {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
        timer = GetCoolDown();
    }

    public void MoveStep() {
        if (isRunning) {
            if (!IsPartCoolingDown()) {
                ResetCooldownTimer();
                Vector2 enemyPos = sensor.GetNearestSensedBotPosition();
                rb.position = enemyPos + new Vector2(0, 2);
                //Collider2D collision = Physics2D.OverlapCircle(new Vector2 (0,0), 1, "Bot");

                //teleport towards enemy bot by half distance on both axis?
                //controller.PlayAudio("Move");
            }
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public override void BotPartUpdate()
    {
        MoveStep();
    }
}
