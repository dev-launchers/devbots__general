using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public int enemyDirection;
    Rigidbody2D rb;
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

    //For an exploding bullet: 
    //Collider2D collision = Physics2D.OverlapCircle(new Vector2 (0,0), 1, "Bot"); 
    //when it explodes
}
