using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBots : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot"); //Gets all bots in combat scene
        foreach(GameObject bot in bots) {
            foreach(BotPart botPart in bot.GetComponentsInChildren<BotPart>()) {
            //Activate components on bot, readying it for battle
            botPart.SetState(new State(true));
            }
        }
    }
}
