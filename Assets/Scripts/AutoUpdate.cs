using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUpdate : MonoBehaviour
{

    //SerializeField allows for objects to be dragged in using Unity Editor
    [SerializeField] private GameObject player = default(GameObject);
    [SerializeField] private GameObject opponent = default(GameObject);

    private Move playerMovement;
    private Attack playerAttack;
    private Move opponentMovement;
    private Attack opponentAttack;

    private const float TURN_TIME = 2.0f;
    private float timer = TURN_TIME;

    private float relativeOpponentPos;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<Move>();
        playerAttack = player.GetComponent<Attack>();
        opponentMovement = opponent.GetComponent<Move>();
        opponentAttack = opponent.GetComponent<Attack>();

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


            //Find if enemy to the left or right
            if (player.transform.position.x - opponent.transform.position.x > 0) {
                relativeOpponentPos = -1.0f;
                //Sprite flips go here
            }
            else {
                relativeOpponentPos = 1.0f;
            }


            //Make the bots move
            playerMovement.MoveStep(relativeOpponentPos);
            opponentMovement.MoveStep(-relativeOpponentPos);

            //Make the bots attack
            playerAttack.AttackStep(relativeOpponentPos);
            opponentAttack.AttackStep(-relativeOpponentPos);

        }

    }
}
