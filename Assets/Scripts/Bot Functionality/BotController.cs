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

    //Get this bot's current HP
    public float GetGetHP()
    {
        return HP;
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
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
