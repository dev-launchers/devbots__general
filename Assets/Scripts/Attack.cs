using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default(Rigidbody2D);
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private LayerMask opponent = default(LayerMask);
    [SerializeField] private float range = default(float);
    [SerializeField] private float damage = default(float);
    //Should depend on specific attack part
    
    private float attacksize = 5.0f;


    public void AttackStep(float enemyPos) {
    //Called each turn, checks for collision and calls its TakeDamage

        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, range, opponent);
        if (collision != null) {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected() { 
    //Test function to draw hitbox for attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, range);
    }
}
