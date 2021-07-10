using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MonoBehaviour , IBotPart
{
    [SerializeField] private float moveSpeed = default(float);
    [SerializeField] private float jumpSize = default(float);

    private Rigidbody2D rb;
    private AudioManager audioManager;

    private bool isRunning;

    public void Start() {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>();  //Maybe outsource this to bot parent thing
    }

    //public void Update() {
        //MoveStep();
    //}

    public void MoveStep() {
        int enemyDirection = GetComponentInParent<BotSensor>().GetNearestSensedBotDirection();
        Debug.Log("enemyDir: " + enemyDirection + " movespd: " + moveSpeed);
        rb.velocity = new Vector2(enemyDirection * moveSpeed, jumpSize);
        audioManager.Play("Move");
    }

    public void SetState(State state) { 
        //Have the turn handler send this for now then check in Update
    }

    public void TakeKnockback(float enemyPos, float knockback) { //This needs to go somewhere
        rb.velocity = new Vector2(enemyPos * knockback, knockback);
    }

}
