using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TailFinPart : BotPart
{
    [Tooltip("Amount of backforce to apply")]
    [SerializeField] private float backThrust = default(float);//Amount of backforce to apply
    private WheelPart wheelPart;//Wheel part script attatched to this bot
    private TeleporterPart teleporterPart;//Teleporter part attatched to this bot
    private Rigidbody2D rb;
    private BotSensor sensor;
    private BotController controller;
    
    [SerializeField] private bool isRunning;
    private float timer;

    [SerializeField]private float coolDown = 4f;//cooldown time for back thrust

    public void Start()
    {

        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
        //wheelPart = transform.parent.GetComponentInChildren<WheelPart>();
        //teleporterPart = transform.parent.GetComponentInChildren<TeleporterPart>();
        timer = coolDown;  
    }

    public void Update()
    {
        BackStep();
    }

    public void BackStep()
    {
        if (isRunning)
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = coolDown; //Reset Timer
                //Use add relative force to rigidbody to thrust bot backwards. 
                rb.AddRelativeForce(new Vector2(-backThrust, 0), ForceMode2D.Impulse);
                controller.PlayAudio("Move");
            }
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }
}