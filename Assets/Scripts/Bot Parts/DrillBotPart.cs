using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//inherihting botpart
public class DrillBotPart : BotPart
{
    //TODO: create "private Animator drillAnimation;" // Animates the drill part when attack is active.

    [SerializeField] public Transform attackPoint; // References the attack point of the drill in the scene.
    [SerializeField] public float attackRange = 0.0f; // Range for attack to initiate.

    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] private bool isRunning;
    // Used to determine which objects are enemies by assigning all ememies to a layer using a layermask.

    // Inherited from BotPart
    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    // drillAttack
    public void drillAttack()
    {
        if (isRunning) {
            if(!IsPartCoolingDown()){
                ResetCooldownTimer();
                // TODO:  Play the drill attack animation.
                
                // Detect enemy in range of attack.
                Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);

                // Damage enemy
                // TODO: Implement damage to enemy health.

                if(enemy)
                {
                    // Outputs message to Unity Editor Console to verify the attack.
                    Debug.Log(enemy.name + " was attacked by drill.");
                }
            }
        }
    }

    // Allows developer/user to see the attack radius in Unity editor.
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return; //Return if attackPoint has not been set.

        // Draw a wire sphere at attack position to show its range in Unity editor.
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public override void BotPartUpdate()
    {     
        drillAttack();
    }
}
