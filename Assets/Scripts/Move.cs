using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default(Rigidbody2D);
    [SerializeField] private float movespeed = default(float);
    [SerializeField] private float jumpSize = default(float);

    private AudioManager audioManager;

    public void Start() {
        //Grab values from parts
        audioManager = FindObjectOfType<AudioManager>();
    }

    // public void MoveStep(float enemyPos) {
    //     //Should depend on specific movement part
    //     rb.velocity = new Vector2(enemyPos * movespeed, jumpSize);
    //     audioManager.Play("Move");
    // }

    public void MoveStep() {
        Debug.Log("entered movestep");
            int enemyDirection = GetComponent<BotSensor>().GetNearestSensedBotDirection();

            Debug.Log("Jumping: "+enemyDirection+" : "+enemyDirection * movespeed + ", " + jumpSize);
            //Should depend on specific movement part
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(enemyDirection * movespeed, jumpSize);

            audioManager = FindObjectOfType<AudioManager>(); //PROBLEME

            audioManager.Play("Move");

    }

    public void TakeKnockback(float enemyPos, float knockback) {
        Debug.Log("Knockback : "+ enemyPos * knockback + ", " + knockback);
        rb.velocity = new Vector2(enemyPos * knockback, knockback);
    }

}
