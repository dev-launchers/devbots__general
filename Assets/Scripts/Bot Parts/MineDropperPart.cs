using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDropperPart : BotPart
{
    private Rigidbody2D rb;
    private BotSensor sensor;
    private BotController controller;

    [SerializeField] private GameObject landmine;

    [SerializeField] private bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackStep()
    {
        // drop landmines
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public override void BotPartUpdate()
    {
        
    }
}
