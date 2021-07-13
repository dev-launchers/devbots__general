using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPart : MonoBehaviour , IBotPart
{
    //[SerializeField] private float teleportDistance = default(float);

    private Rigidbody2D rb;
    private BotSensor sensor;

    private bool isRunning;
    private float timer;

    private const float COOLDOWN = 2.0f;

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

            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                timer = COOLDOWN; //Reset Timer
                Vector2 enemyPos = sensor.GetNearestSensedBotPosition();
                rb.position = enemyPos + new Vector2(0, 2);
                //Collider2D collision = Physics2D.OverlapCircle(new Vector2 (0,0), 1, "Bot");

                //teleport towards enemy bot by half distance on both axis?
                sensor.PlayAudio("Move");
            }
        }
    }

    public void SetState(State state) { 
        //Have the turn handler send this for now then check in Update
    }
}
