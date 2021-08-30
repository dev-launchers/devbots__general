using UnityEngine;
public class UserInterface : MonoBehaviour
{
    [Header("Bot controllers for each bot")]
    [SerializeField] BotController playerBotController;
    [SerializeField] BotController enemyBotController;
    [Header("Bot health bar for each bot")]
    [SerializeField] HealthBar playerHealthBar;
    [SerializeField] HealthBar enemyHealthBar;
    //Prefab used to instantiate a cooldown bar
    [SerializeField] GameObject cooldownBarPrefab;
    [Header("Layout transform belonging to each bot for cooldown bars")]
    [SerializeField] Transform playerCooldownLayout;
    [SerializeField] Transform enemyCooldownLayout;

    private void Awake()
    {
        //Define which bot is which
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");
        foreach (GameObject bot in bots)
        {
            if (bot.GetComponent<BotSensor>().IsPlayer())
            {
                playerBotController = bot.GetComponent<BotController>();
            }
            else
            {
                enemyBotController = bot.GetComponent<BotController>();
            }
        }

        //Add listeners to the damage taken events attatched to both bots
        enemyBotController.DamageTakenEvent.AddListener(OnEnemyDamageTaken);
        playerBotController.DamageTakenEvent.AddListener(OnPlayerDamageTaken);
    }





    //When a bot takes damage these functions are called 
    private void OnPlayerDamageTaken()
    {
        playerHealthBar.SetSliderValue(playerBotController.GetGetHP());


    }
    private void OnEnemyDamageTaken()
    {
        enemyHealthBar.SetSliderValue(enemyBotController.GetGetHP());

    }

}
