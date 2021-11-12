using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tesla Ability", menuName = "Ability Objects/Attack Ability/Tesla", order = 1)]

public class TeslaAbility : BotAbility
{
    [SerializeField] float damage = default(float);
    [SerializeField] GameObject teslaEffect = default(GameObject);//the gameobject used as an effect for the tesla tower
    [SerializeField] private float attackRadius = default(float);

    public override void Activate(GameObject parent)
    {
        BotSensor sensor = parent.GetComponentInParent<BotSensor>();
        BotController controller = parent.GetComponentInParent<BotController>(); 
        int enemyLayer = sensor.GetEnemyLayer();
        
        

                //Instantiate effect at the position of the tesla tower and parent it with this transfom to keep its position with the bot
                GameObject effect = Instantiate(teslaEffect, parent.transform.position, Quaternion.identity);
                // change the size of the effect relative to the size of the attack radius
                effect.transform.localScale = new Vector2(attackRadius * 1.25f, attackRadius * 1.25f);
                effect.transform.SetParent(parent.transform);
                //Destroy effect after time
                Destroy(effect, 0.5f);

                List<Collider2D> collisions = new List<Collider2D>(Physics2D.OverlapCircleAll(parent.transform.position, attackRadius));
                //print(collisions.Count);
                foreach (Collider2D collision in collisions) {
                    if (collision.gameObject.layer == enemyLayer) {
                        //print(GetInstanceID()+ " is colliding with "+ collision.gameObject.GetInstanceID());
                        collision.transform.GetComponent<BotController>().TakeDamage(damage);
                    }
                }
                //For some reason Tesla Tower collides with enemy twice every collision


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
}
