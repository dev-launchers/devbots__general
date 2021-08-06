using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDetonatorPart : MonoBehaviour, IBotPart
{
    //TODO: create "private Animator sideDetonatorAnimation;"
    // Animates the side detonator part when attack is active.

    [SerializeField] private Transform attackPoint;
    // References the attack point of the side detonator in the scene.
    
    [SerializeField] private float attackRange = 0.0f;
    // Range for attack to initiate.
    
    [SerializeField] private float knockBackStrength;
    [SerializeField] private LayerMask enemyLayers;


    // Inherited from IBotPart
    public void SetState(State state)
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        SelfDetonatorAttack();
    }

    // Side Detonator Attack
    public void SelfDetonatorAttack()
    {
        // Detect enemy in range of attack.
        Collider2D enemyCollider2D = Physics2D.OverlapCircle(attackPoint.position,
                                                   attackRange,
                                                   enemyLayers);

        if (enemyCollider2D)
        {
            Debug.Log(enemyCollider2D.name + " was attacked by self detonator part.");
            // TODO: Play the side detonator attack animation.
            // TODO: Implement damage to enemy health. (Use separate class?)
            // TODO: Implement small damage to player health. (Use separate class?)

            // Knockback opponent
            Rigidbody2D rb = enemyCollider2D.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = rb.transform.position - transform.position;
                rb.AddForce(direction.normalized * knockBackStrength, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}