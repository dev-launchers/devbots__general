using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack Ability", menuName = "Ability Objects/Attack Ability/Sword", order = 1)]
public class AttackAbility : BotAbility
{
    public Vector2 dir;
    public float damage;
    public float attackDistance;//what is this ?
    public float knockback;
    
    private Animator animator;//Animator used for sword rotation
    private Rigidbody2D rb;
    private BotSensor sensor;
    private int enemyLayer;
    public override void Activate(GameObject parent)//we may split this up further if necessary with more layers of abstraction
    {
        base.Activate(parent);
        
        //SET UP
        animator = parent.GetComponent<Animator>();
        rb = parent.GetComponentInParent<Rigidbody2D>();
        sensor = parent.GetComponentInParent<BotSensor>();
        enemyLayer = sensor.GetEnemyLayer();
        ////////////
        
        //ACTION
        // Set trigger to play animation of sword rotating 
        animator.SetTrigger("swordAttack");
        // add thrust to lunge bot forward 
        Vector2 appliedForce = new Vector2(dir.x * sensor.GetNearestSensedBotDirection(), dir.y);
        rb.AddRelativeForce(appliedForce, ForceMode2D.Impulse);

        Vector2 attackPos = parent.transform.position + new Vector3(sensor.GetNearestSensedBotDirection(), 0, 0);
        //Should be cleaned up, but currently creates Vector2 for current position + 1 in direction of enemy
        Collider2D collision = Physics2D.OverlapCircle(attackPos, attackDistance); 
        //Needs to attack only in front using swordPos

        if (collision.gameObject.layer == enemyLayer)
        {
            Debug.Log("collision");
            BotController collisionController = collision.transform.GetComponent<BotController>();
            collisionController.TakeDamage(damage);
            collisionController.ApplyForce(new Vector2(knockback * sensor.GetNearestSensedBotDirection(),0));
        }
        
        
        Debug.Log(parent.name+" performed attack ability");
        
    }
}
