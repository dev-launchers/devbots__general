using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Drill Ability", menuName = "Ability Objects/Attack Ability/Drill", order = 1)]

public class DrillAbility : BotAbility
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        // TODO:  Play the drill attack animation.
                
        // Detect enemy in range of attack.
        //Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);

        // Damage enemy
        // TODO: Implement damage to enemy health.
        /*
        if(enemy)
        {
            // Outputs message to Unity Editor Console to verify the attack.
            Debug.Log(enemy.name + " was attacked by drill.");
        }*/
    }
}
