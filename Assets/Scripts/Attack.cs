using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private LayerMask opponent = default(LayerMask);
    [SerializeField] private float range = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    //Should depend on specific attack part

    public void AttackStep(float enemyPos) {
    //Called each turn, checks for collision and calls its TakeDamage

        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, range, opponent);
        //OverlapBox for rectangular hitbox

        if (collision != null) {
            collision.GetComponent<Health>().TakeDamage(damage);
            collision.GetComponent<Move>().TakeKnockback(enemyPos, knockback);
        }
    }

    void OnDrawGizmosSelected() { 
    //Test function to draw hitbox for attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, range);
    }
}
