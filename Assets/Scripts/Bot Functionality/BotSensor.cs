using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotSensor : MonoBehaviour
{
    private GameObject nearestBot;
    private GameObject[] activeBots;
    private AudioManager audioManager;
    private Rigidbody2D rb;
    private int enemyLayer;

    public void Awake() {
        activeBots = GameObject.FindGameObjectsWithTag("Bot");
        rb = GetComponent<Rigidbody2D>();
        SenseStep(); //In multi-bot fights, needs to be called in Update
        if (gameObject.layer == 9) { //Player is shooting
            enemyLayer = 10;
        }
        else { //Opponent is shooting
            enemyLayer = 9;
        }
    }

    public int GetEnemyLayer() {
        return enemyLayer;
    }

    public GameObject GetNearestSensedBot() {
        UpdateActiveBots();
        return nearestBot;
    }

    public Vector2 GetNearestSensedBotPosition() {
        UpdateActiveBots();
        return nearestBot.transform.position;
    }

    // Returns -1 if left, and 1 if right
    public int GetNearestSensedBotDirection() {
        UpdateActiveBots();
        int enemyDirection = 1;
        //Find if enemy to the left or right
        if (gameObject.transform.position.x - nearestBot.transform.position.x > 0) {
            enemyDirection = -1;
        }
        return enemyDirection;
    }

    public void SenseStep() {
        //Updates the current "Nearest Bot," always the enemy in 1v1, closest enemy in multibot
        foreach(GameObject activeBot in activeBots) {
            if (activeBot != this.gameObject) {
                nearestBot = activeBot;
            }
        }
    }

    public Vector3 GetPosition() {
        return rb.position;
    }

    private void UpdateActiveBots() {
        if(nearestBot == null) { //This is a temporary workaround and needs to be fixed so that the sensor activates and gets bot list at proper time
            activeBots = GameObject.FindGameObjectsWithTag("Bot");
            SenseStep();
        }
    }
}
