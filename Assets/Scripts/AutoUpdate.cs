using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUpdate : MonoBehaviour
{

    //SerializeField allows for objects to be dragged in using Unity Editor
    [SerializeField] private GameObject player = default(GameObject);
    [SerializeField] private GameObject enemy = default(GameObject);

    private Move enemyMovement;
    private Attack enemyAttack;

    private const float TURN_TIME = 2.0f;
    private float timer = TURN_TIME;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = enemy.GetComponent<Move>();
        enemyAttack = enemy.GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            timer = TURN_TIME;


            //Make the bots move
            enemyMovement.MoveStep(-1.0f);


            //Make the bots attack
            enemyAttack.AttackStep(-1.0f);


        }

    }
}
