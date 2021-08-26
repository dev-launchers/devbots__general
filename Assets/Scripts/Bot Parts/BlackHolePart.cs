using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolePart : BotPart
{
    //TODO: create "private Animator blackHoleAnimation;"
    // Animates the black hole vortex when attack is active
    [SerializeField] private Transform attackPoint;
    // References the attack point of the black hole vortex in the scene.

    [SerializeField] private float attackRange = 0.0f;
    // Range for attack to initiate.
    
    [SerializeField] private float pullStrength;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private bool isRunning;
    
    public override void SetState(State state)
    {
       isRunning = state.isActive;
    }

    // Side Detonator Attack
    public void BlackHoleAttack()
    {
        // Detect enemy in range of attack.
        Collider2D enemyCollider2D = Physics2D.OverlapCircle(attackPoint.position,
                                                             attackRange,
                                                             enemyLayers);
        if (enemyCollider2D && isRunning)
        {
            if (!IsPartCoolingDown()){
                ResetCooldownTimer();

                Debug.Log(enemyCollider2D.name + " was attacked by Black Hole part.");
                // TODO: Play the Black Hole attack animation.

                // Pull opponent rapidly toward player
                BotController controller = enemyCollider2D.GetComponentInParent<BotController>();
                BotSensor sensor = enemyCollider2D.GetComponentInParent<BotSensor>();
                if (controller != null)
                {
                    Vector2 direction = sensor.GetPosition() - transform.position;
                    controller.ApplyForce(-1 * (direction.normalized * pullStrength));
                }
            }
        }
    }
    public override void BotPartUpdate()
    {
        BlackHoleAttack();
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}