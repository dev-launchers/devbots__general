using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndCDHolder : MonoBehaviour
{
    //Bot controller used with this healthbar and cooldown  bars
    [SerializeField]private BotController botController;
    //get the bot controller 
    public BotController GetBotController()
    {
        return botController;
    }
}
