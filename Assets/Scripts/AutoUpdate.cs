using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoUpdate : MonoBehaviour
{

    //SerializeField allows for objects to be dragged in using Unity Editor
    [SerializeField] private GameObject player = default(GameObject);
    [SerializeField] private GameObject opponent = default(GameObject);

    private Move playerMovement;
    private Attack playerAttack;
    private UpdateSprite playerSprite;
    private Move opponentMovement;
    private Attack opponentAttack;
    private UpdateSprite opponentSprite;

    private const float TURN_TIME = 2.0f;
    private float timer = TURN_TIME;

    // Events
    public UnityEvent<List<GameObject>> senseEvent;
    public UnityEvent<List<GameObject>> moveEvent;
    public UnityEvent<List<GameObject>> attackEvent;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<Move>();
        playerAttack = player.GetComponent<Attack>();
        playerSprite = player.GetComponent<UpdateSprite>();
        opponentMovement = opponent.GetComponent<Move>();
        opponentAttack = opponent.GetComponent<Attack>();
        opponentSprite = player.GetComponent<UpdateSprite>();

        //GameObject.FindGameObjectWithTag("Player") If objects need to be instantiated later and can't be assigned in menu
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            timer = TURN_TIME;

            List<GameObject> activeBots = new List<GameObject>{player, opponent};

            // Make the bots sense their surroundings
            senseEvent.Invoke(activeBots);

            //Make the bots move
            moveEvent.Invoke(activeBots);

            //Make the bots attack
            attackEvent.Invoke(activeBots);

            //Update Sprites
            //playerSprite.FlipSprite(relativeOpponentPos);
            //opponentSprite.FlipSprite(relativeOpponentPos);
        }
    }
}
