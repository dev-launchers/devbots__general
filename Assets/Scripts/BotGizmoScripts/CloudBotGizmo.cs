using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBotGizmo : MonoBehaviour
{
    [SerializeField] private float moveSpeed = default(float);
    [SerializeField] private float jumpSize = default(float);

    private Rigidbody2D rb;
    private AudioManager audioManager;

    public void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>();  //Maybe outsource this to bot parent thing
    }

    public void MoveStep(float enemyPos) {
        rb.velocity = new Vector2(enemyPos * moveSpeed, jumpSize);
        audioManager.Play("Move");
    }

    public void TakeKnockback(float enemyPos, float knockback) {
        rb.velocity = new Vector2(enemyPos * knockback, knockback);
    }

}
