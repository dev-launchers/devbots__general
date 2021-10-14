using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TailFinPart : BotPart
{
    [Tooltip("Amount of backforce to apply")]
    [SerializeField] private Vector2 backThrust = default(Vector2);//Amount of backforce to apply
    private WheelPart wheelPart;//Wheel part script attatched to this bot
    private TeleporterPart teleporterPart;//Teleporter part attatched to this bot
    private Rigidbody2D rb;
    private BotSensor sensor;
    private BotController controller;
    
    [SerializeField] private bool isRunning;

    public void Start()
    {

        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
        //wheelPart = transform.parent.GetComponentInChildren<WheelPart>();
        //teleporterPart = transform.parent.GetComponentInChildren<TeleporterPart>();
        timer = GetCoolDown();  
    }

    public void BackStep()
    {
        if (isRunning)
        {
            if (!IsPartCoolingDown())
            {
                ResetCooldownTimer();

                //Use add relative force to rigidbody to thrust bot backwards. 
                Vector2 appliedForce = new Vector2(-backThrust.x * sensor.GetNearestSensedBotDirection(), backThrust.y);
                rb.AddRelativeForce(appliedForce, ForceMode2D.Impulse);
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
        BackStep();
    }
}
