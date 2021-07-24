using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private AudioManager audioManager;
    private Health health;
    private Rigidbody2D rb;

    public void Start() {
        audioManager = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    public void SetPosition(Vector3 newPosition) {
        //The desired new position is sent by the attacking bot, but may be countered by certain effects
        rb.position = newPosition;
    }

    public void ApplyForce(Vector3 force) {
        //The desired force is sent by the attacking bot, but may be countered by certain effects
        rb.AddForce(force);
    }

    public void PlayAudio(string audioName) {
        audioManager.Play(audioName);
    }

    public void TakeDamage(float damage) {
        health.HP -= damage;
    }
}
