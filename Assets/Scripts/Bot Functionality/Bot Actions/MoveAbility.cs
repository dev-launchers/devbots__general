using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Move Ability", menuName = "Ability Objects/Movement Ability", order = 2)]

public class MoveAbility : BotAbility
{
    
    
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public BotSensor sensor;
    [HideInInspector]public BotController controller;
    public override void Activate(GameObject parent)
    {
        //SET UP
        
        
        
    }
}
