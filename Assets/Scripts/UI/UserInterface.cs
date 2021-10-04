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
    //the gameobject for the settings panel
    [SerializeField] GameObject settingsPanel;
    // bool used to determine whether game is paused or not
    bool isPaused = false;



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

    private void Start()
    {
        //set pause panel and its child objects to not active at the start of the scene
        settingsPanel.SetActive(false);
    }



    /// <summary>
    /// When a bot takes damage these functions are called 
    /// </summary>
    private void OnPlayerDamageTaken()
    {
        playerHealthBar.SetSliderValue(playerBotController.GetGetHP());


    }
    /// <summary>
    /// When a bot takes damage these functions are called 
    /// </summary>
    private void OnEnemyDamageTaken()
    {
        enemyHealthBar.SetSliderValue(enemyBotController.GetGetHP());

    }
    /// <summary>
    /// Method used to open and close the settings panel during the combat scene
    /// </summary>
    public void SettingsButtonPressed()
    {
        if (isPaused)
        {
            Debug.Log("Un paused");
            //set timescale to 1 to unpause the game
            Time.timeScale = 1;
            isPaused = false;
            //deactivate settingspanel
            settingsPanel.SetActive(false);
        }
        else
        {
            Debug.Log("paused");
            //set time scale to 0 to pause the game
            Time.timeScale = 0;
            isPaused = true;
            //activate settings panel
            settingsPanel.SetActive(true);
        }
    }
    /// <summary>
    /// Method used to set the timescale of the game
    /// </summary>
    /// <param name="timeScale"></param>
    public static void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
