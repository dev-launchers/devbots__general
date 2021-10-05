using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BotController : MonoBehaviour
{
    private BotSensor sensor;
    private AudioManager audioManager;
    private Rigidbody2D rb;
    public UnityEvent DamageTakenEvent;
    [SerializeField] private float HP = 1;
    [SerializeField] private float deathAnimationTime = 0;
    //class used to locate and change slots and the botparts which are on each slot
    public Slots slots;
    //bool used to determine whether this bot has already been created
    public static bool created = false;
    //Get this bot's current HP
    public float GetGetHP()
    {
        return HP;
    }

    public void Awake()
    {
        if (!created && sensor.IsPlayer())
        {
            //if this bot hasn't been created add it to dontdestroy on load
      DontDestroyOnLoad(this);
            created = true;
        }
        else if(sensor.IsPlayer())
        {
            //if this bot has been created already destroy this bot
            Destroy(this.gameObject);
        }
  
    }
    public void OnEnable()
    {
        //Delegate used to trigger Onsceneloaded method when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Start()
    {        
        sensor = GetComponent<BotSensor>();
        audioManager = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        if (DamageTakenEvent == null)
            DamageTakenEvent = new UnityEvent();

    }

    public void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Combat" || currentScene == "General Testing Scene")
        {
            FaceEnemy();
        }
    }
    /// <summary>
    /// Method called when a scene is loaded
    /// </summary>
    /// <param name="scene">the name of new scene that is loaded</param>
    /// <param name="loadSceneMode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode loadSceneMode)
    {
        //check new loaded scene's name
if (scene.name == "Main Menu Scene" || scene.name == "Bot Customize Scene"|| scene.name == "Combat")
        {
            //activate this bot
            gameObject.SetActive(true);
            //set bot's position to the botStartPos gameobjects position
            transform.position = GameObject.Find("botStartPos").transform.position;
        }
        else if (scene.name == "Marketplace Scene"|| scene.name == "Settings Scene")
        {
            //deactivate this bot
            gameObject.SetActive(false);
        }
    }


    private void FaceEnemy()
    {
        foreach (Transform childtransform in transform)
        {
            childtransform.localScale = new Vector3(sensor.GetNearestSensedBotDirection(), 1, 1);
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        //The desired new position is sent by the attacking bot, but may be countered by certain effects
        rb.position = newPosition;
    }

    public void ApplyForce(Vector3 force)
    {
        //The desired force is sent by the attacking bot, but may be countered by certain effects
        rb.AddRelativeForce(force, ForceMode2D.Impulse);
    }

    public void PlayAudio(string audioName)
    {
        audioManager.Play(audioName);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        DamageTakenEvent.Invoke();
        if (HP <= 0.0f)
        {
            //start botdestroyed coroutine when bot reaches zero health
            StartCoroutine(BotDestroyed());

            //Destroy(sensor.GetNearestSensedBot());
            //Destroy(gameObject);


            //audioManager.Play("Death");
            //animator.Play("death");
            //Make a new gameObject for dead hull, or disable scripts?
            //Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        else
        {
            audioManager.Play("Hit");
            //Instantiate(damageFX, transform.position, Quaternion.identity);
        }
    }
    public IEnumerator BotDestroyed()
    {
        //run death animation here and change deathAnimationTime in the inspector

        //delay to play animation before changing scene
        yield return new WaitForSeconds(deathAnimationTime);

        //check if bot desstroyed is the players or not then load appropiate scene
        if (sensor.IsPlayer())
        {
            SceneHandler.LoadLoseScene();
        }
        else
        {
            SceneHandler.LoadVictoryScene();
        }
    }
}
