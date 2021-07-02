using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSensor : MonoBehaviour
{
    private GameObject nearestBot;

    public GameObject GetNearestSensedBot() {
        return nearestBot;
    }

    // Returns -1 if left, and 1 if right
    public int GetNearestSensedBotDirection() {
        GameObject player = this.gameObject;
        GameObject opponent = nearestBot;
        int enemyDirection = 1;
        //Find if enemy to the left or right
        if (player.transform.position.x - opponent.transform.position.x > 0) {
            enemyDirection = -1;
        }
        return enemyDirection;
    }

    public void SenseStep(List<GameObject> activeBots) {
        Debug.Log("entered sensestep");
        // Find nearest bot? (future stuff)
        foreach(GameObject activeBot in activeBots) {
            if (activeBot == this.gameObject) continue; // Change in future

            nearestBot = activeBot;
        }
    }
}
