using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPart : BotPart
{

    [SerializeField] private LayerMask enemy = default(LayerMask);
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float attackSize = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);

    private bool isRunning;

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public void AttackStep(List<GameObject> activeBots) {
    //Called each turn, checks for collision and calls its TakeDamage

        int enemyDirection = GetComponent<BotSensor>().GetNearestSensedBotDirection();

        attackPos.localPosition = new Vector3(enemyDirection * attackDistance, 0, 0); //Faces attack at enemy, handled as local position to parent Bot

        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, attackSize, enemy);
        //OverlapBox for rectangular hitbox

        if (collision != null) {
            //collision.GetComponent<Health>().TakeDamage(damage);
            //collision.GetComponent<Move>().TakeKnockback(enemyDirection, knockback);
        }
    }
}
