using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private int enemyLayer;
    private int enemyDirection;

    Rigidbody2D rb;
    //[SerializeField] private UnityEvent projectileCollisionEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(enemyDirection * speed,0), ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //On collision enter 2d?
        //BotSensor hitSensor = collision.GetComponent<BotSensor>();
        //hitSensor.TakeDamage(damage);
        //hitSensor.GetPosition
        //Vector3 newPos = Calculate new position
        //hitSensor.TakeKnockback(newPos);
    }

    public void SetValues(int dir, float dmg, float spd, Vector2 size, int layer) {
        enemyDirection = dir; //Set the direction of the projectile
        damage = dmg; //Set the damage of the projectile
        speed = spd; //Set the speed of the projectile
        gameObject.transform.localScale = size; //Set the projectile size
        enemyLayer = layer; //Set the target of the projectile, so it only hits the desired bot, will likely need to be array of layers for self-damaging items 
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    print("collided wtih " + collision.gameObject.name);
    //    //Check what layer collided game object is
    //    if (collision.gameObject.layer == enemyLayer)
    //    {
    //        //event invoke for unity event. can add to in editor
    //        //projectileColisionEvent.Invoke();   

    //        //Deal damage to collided enemy
    //        // collision.gameObject.GetComponent<BotController>().TakeDamage(damage);
    //    }

    //    //Destroy projectile
    //    Destroy(this.gameObject);
    //}

    //For an exploding bullet: 
    //Collider2D collision = Physics2D.OverlapCircle(new Vector2 (0,0), 1, "Bot"); 
    //when it explodes
}
