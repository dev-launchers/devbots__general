using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private LayerMask enemy = default(LayerMask);
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float attackSize = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    //Should depend on specific attack part

    public void AttackStep(float enemyPos) {
    //Called each turn, checks for collision and calls its TakeDamage

        //if (abs(transform.position - enemy position)) > strikingDistance {
        //  Do attack stuff
        //}

        attackPos.localPosition = new Vector3(enemyPos * attackDistance, 0, 0); //Faces attack at enemy, handled as local position to parent Bot

        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, attackSize, enemy);
        //OverlapBox for rectangular hitbox

        if (collision != null) {
            collision.GetComponent<Health>().TakeDamage(damage);
            collision.GetComponent<Move>().TakeKnockback(enemyPos, knockback);
        }
    }

    void OnDrawGizmosSelected() { 
    //Test function to draw hitbox for attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackSize);
    }
}
