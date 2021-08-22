using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Class used to create and update each of the botparts cooldown bars attached to this bot
/// </summary>
class BotPartCooldownBars
{
    /// <summary>
    /// Class used to get the botPart and cooldown bar associated with each other
    /// </summary>
    public class BotPartCoolDownObject
    {
        public GameObject coolDownBar;
        public BotPart botPart;

        public BotPartCoolDownObject(GameObject cool_down_bar, BotPart bot_part)
        {
            coolDownBar = cool_down_bar;
            botPart = bot_part;
        }
    }

    /// <summary>
    /// Used to get a list of all cooldown objects attached to this bot 
    /// </summary>
    private List<BotPartCoolDownObject> coolDownObjects = new List<BotPartCoolDownObject>();
    /// <summary>
    /// Used to get a list of all botparts associated with this bot
    /// </summary>
    private BotPart[] botParts;

    /// <summary>
    /// Creating a new instance of this class instantiates all the cooldown bars for each botpart attatched to the bot
    /// </summary>
    /// <param name="bot_controller">Used to identify the bot and the botparts attatched to this bot</param>
    /// <param name="cooldownBarPrefab">The prefab used to instantiate each Cooldown bar</param>
    /// <param name="layout">The layout transform that each newly instantiated cooldown bar will be a child of</param>
    public BotPartCooldownBars(BotController bot_controller, GameObject cooldownBarPrefab, Transform layout)
    {
        //Get all botparts belonging to this bot
        botParts = bot_controller.GetComponentsInChildren<BotPart>();
        foreach (var botPart in botParts)
        {
            //Debug.Log(botPart.name);
            //Instantiate a cooldown prefab object and set it as a child of the choosen layout transform
            var cooldownBar = Object.Instantiate(cooldownBarPrefab, layout);
            //Set the max value of the slider of the cooldown prefab to the cooldown time of this botpart
            cooldownBar.GetComponentInChildren<Slider>().maxValue = botPart.GetCoolDown();
            //Set the text of the text object belonging to the prefab to the current botpart name
            cooldownBar.GetComponentInChildren<Text>().text = botPart.name;
            // Create a BotpartCoolDownObject using the cooldownBar Instantiated and the botpart as parameters
            var botPartData = new BotPartCoolDownObject(cooldownBar, botPart);
            //Add object to the list of cooldown objects belonging to this bot
            coolDownObjects.Add(botPartData);
        }                    
    }

    /// <summary>
    /// Used to update list of cooldown bars attatched to this bot
    /// </summary>
    public void Update()
    {
        foreach (var botPartCooldownObject in coolDownObjects)
        {
            //Set the slider belonging to each cooldown object
            Slider slider = botPartCooldownObject.coolDownBar.GetComponentInChildren<Slider>();
            //Get the cooldown time bbelonging to this botpart
            float coolDownTime = botPartCooldownObject.botPart.GetCoolDown();
            //Get the timer time attatch to thia bot
            float timer = botPartCooldownObject.botPart.GetCoolDownTimer();
            slider.value = coolDownTime - timer;
            /// Debug.Log(timer);
        }
    }
}