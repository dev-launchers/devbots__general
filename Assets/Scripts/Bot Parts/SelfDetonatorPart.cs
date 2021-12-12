using UnityEngine;

public class SelfDetonatorPart : BotPart
{
    //TODO: create "private Animator sideDetonatorAnimation;"
    // Animates the side detonator part when attack is active
    [SerializeField] private Transform attackPoint;
    // References the attack point of the side detonator in the scene.

    [SerializeField] private float attackRange = 0.0f;
    // Range for attack to initiate.
    
    [SerializeField] private float knockBackStrength;
    [SerializeField] private float upwardForce;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private bool isRunning;
    
    // Inherited from BotPart
    override public void SetState(State state)
    {
        return;
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
            BotController controller = enemyCollider2D.GetComponentInParent<BotController>();
            BotSensor sensor = enemyCollider2D.GetComponentInParent<BotSensor>();
            if (controller != null)
            {
                Vector2 direction = sensor.GetPosition() - transform.position;
                controller.ApplyForce((direction.normalized * knockBackStrength)
                                     +(new Vector2(0.0f,upwardForce)));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public override void BotPartUpdate()
    {
        SelfDetonatorAttack();
    }
}
