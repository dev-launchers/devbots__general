using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaPart : BotPart
{
    [SerializeField] private LayerMask enemyLayer = default(LayerMask);
    [SerializeField] float damage = default(float);
    [SerializeField] GameObject teslaEffect = default(GameObject);//the gameobject used as an effect for the tesla tower
    [SerializeField] private float attackRadius = default(float);

    private BotSensor sensor;
    private BotController controller;
    [SerializeField] private bool isRunning;
    private float timer;
    private const float COOLDOWN = 2.0f;

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    void Start()
    {
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackStep();
    }

    public void AttackStep()
    {

        if (isRunning)
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = COOLDOWN; //Reset Timer

                Collider2D collision = Physics2D.OverlapCircle(transform.position, attackRadius, enemyLayer);
                
                if (collision)
                {
                    // Debug.Log(collision.gameObject.name);
                    //Instantiate effect at the position of the tesla tower and parent it with this transfom to keep its position with the bot
                    GameObject effect = Instantiate(teslaEffect, this.gameObject.transform.position, Quaternion.identity);
                    // change the size of the effect relative to the size of the attack radius
                    effect.transform.localScale = new Vector2(attackRadius * 1.25f, attackRadius * 1.25f);
                    effect.transform.SetParent(this.transform);

                    collision.GetComponent<BotController>().TakeDamage(damage);
                    //Destroy effect after time
                    Destroy(effect, 0.5f);
                    //Debug.Log("Attack");
                }


                /*
                //check if distance between this bot and the closest bot is within the attack radius
                if(Vector2.Distance(this.transform.position,enemy.transform.position) < attackRadius)
                {
                    //Instantiate effect at the position of the tesla tower and parent it with this transfom to keep its position with the bot
                    var effect = Instantiate(teslaEffect, this.gameObject.transform.position,Quaternion.identity);  
                    effect.transform.SetParent(this.transform);
                    // change the size of the effect relative to the size of the attack radius
                    effect.transform.localScale = new Vector2(attackRadius * 1.25f, attackRadius * 1.25f);
  
                    enemy.GetComponent<Health>().TakeDamage(damage);
                    //Destroy effect after time
                    Destroy(effect, 0.5f);
                    //Debug.Log("Attack");
 
                }
            
                */
            }

          //  sensor.PlayAudio("Hit");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the attack radius when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
