using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSensor : MonoBehaviour
{
    private GameObject nearestBot;
    private GameObject[] activeBots;
    private AudioManager audioManager;

    public GameObject GetNearestSensedBot() {
        return nearestBot;
    }
    
    public void Start() {
        activeBots = GameObject.FindGameObjectsWithTag("Bot");
        SenseStep();
        audioManager = FindObjectOfType<AudioManager>();
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

    public Vector2 GetNearestSensedBotPosition() {
        return nearestBot.transform.position;
    }

    public void SenseStep() {
        //Updates the current "Nearest Bot" in case of multibot battles
        // Find nearest bot? (future stuff)
        foreach(GameObject activeBot in activeBots) {
            if (activeBot == this.gameObject) continue; // Change in future

            nearestBot = activeBot;
        }
    }

    public void PlayAudio(string audioName) {
        audioManager.Play(audioName);
    }
}
