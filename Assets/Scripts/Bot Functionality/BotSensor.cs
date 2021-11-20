using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotSensor : MonoBehaviour
{
    private GameObject nearestBot;
    private GameObject[] activeBots;
    private Rigidbody2D rb;
    private int enemyLayer;
    private bool isPlayer;

    public void Awake() {
        activeBots = GameObject.FindGameObjectsWithTag("Bot");
        rb = GetComponent<Rigidbody2D>();
        SenseStep(); //In multi-bot fights, needs to be called in Update

        if (gameObject.layer == 9) { //This bot is Player
            isPlayer = true;
            enemyLayer = 10;
        }
        else { //This bot is Opponent
            isPlayer = false;
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
        if (gameObject.transform.position.x > nearestBot.transform.position.x) {
            enemyDirection = -1;
        }
        return enemyDirection;
    }

    /*GetNearestSensedBotAbove returns 1 if nearestBot is above,
    *    and -1 if below.
    * @param xPosBuffer is a valid float representing the maximum distance
    *    from the left or right of the players center mass.
    * @param yPosBuffer is a valid float representing the minimum distance
    *    above player center mass for enemy bot to be considerd above.
    */
    public int GetNearestSensedBotAbove(float xMaxPos, float yMinPos)
    {
        int enemyBotAbove = -1; //Enemy bot is NOT above player bot. 
        float playerYPos = gameObject.transform.position.y;
        float playerXPos = gameObject.transform.position.x;
        float enemyYPos  = nearestBot.transform.position.y;
        float enemyXPos  = nearestBot.transform.position.x;

        UpdateActiveBots();
        
        // Verify enemy is within the maximum X range distance from player.
        if(enemyXPos >= playerXPos - xMaxPos && enemyXPos <= playerXPos + xMaxPos)
        {
            // Verify enemy is above the minimum domain distance from player.
            if (enemyYPos >= playerYPos + yMinPos)
            {
                enemyBotAbove = 1; // Enemy bot is above player bot.
            }
        }
        return enemyBotAbove;
    }

    public void SenseStep() {
        //Updates the current "Nearest Bot," always the enemy in 1v1, closest enemy in multibot
        foreach(GameObject activeBot in activeBots) {
            if (activeBot != this.gameObject) {
                nearestBot = activeBot;
            }
        }
    }

    private void UpdateActiveBots() {
        if(nearestBot == null) { //This is a temporary workaround and needs to be fixed so that the sensor activates and gets bot list at proper time
            activeBots = GameObject.FindGameObjectsWithTag("Bot");
            SenseStep();
        }
    }
    
    public Vector3 GetPosition() {
        return rb.position;
    }

    public bool IsPlayer() {
        return isPlayer;
    }
}
