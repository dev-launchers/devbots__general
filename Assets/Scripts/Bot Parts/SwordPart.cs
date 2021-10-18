using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPart : BotPart
{

    private Vector2 attackPos = default(Vector2);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    [SerializeField] private Vector2 thrustForce = default(Vector2);
    [SerializeField] private bool isRunning;
    private int enemyLayer;

    private Animator swordAnimator;//Animator used for sword rotation
    private Rigidbody2D rb;
    private BotSensor sensor;

    public override void SetState(State state) {
        isRunning = state.isActive;
    }

    private void Start()
    {
        swordAnimator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
        enemyLayer = sensor.GetEnemyLayer();
        timer = GetCoolDown();
    }
    public void AttackStep()
    {

        if (isRunning)
        {
            if(!IsPartCoolingDown())        
            {
                ResetCooldownTimer();

                // Set trigger to play animation of sword rotating 
                swordAnimator.SetTrigger("swordAttack");
                // add thrust to lunge bot forward 
                Vector2 appliedForce = new Vector2(thrustForce.x * sensor.GetNearestSensedBotDirection(), thrustForce.y);
                rb.AddRelativeForce(appliedForce, ForceMode2D.Impulse);

                attackPos = transform.position + new Vector3(sensor.GetNearestSensedBotDirection(), 0, 0);
                //Should be cleaned up, but currently creates Vector2 for current position + 1 in direction of enemy
                Collider2D collision = Physics2D.OverlapCircle(attackPos, attackDistance); 
                //Needs to attack only in front using swordPos

                if (collision.gameObject.layer == enemyLayer)
                {
                    print("collision");
                    BotController collisionController = collision.transform.GetComponent<BotController>();
                    collisionController.TakeDamage(damage);
                    collisionController.ApplyForce(new Vector2(knockback * sensor.GetNearestSensedBotDirection(),0));
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Display the attack radius when selected
        Gizmos.color = Color.green;
        attackPos = transform.position;
        if (Application.isPlaying)
        {
        attackPos = transform.position + new Vector3(sensor.GetNearestSensedBotDirection(), 0, 0);
        }


        Gizmos.DrawWireSphere(attackPos, attackDistance);

    }

    public override void BotPartUpdate()
    {
        AttackStep();
    }
}
