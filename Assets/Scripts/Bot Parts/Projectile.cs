using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public int enemyDirection;
    Rigidbody2D rb;
    [SerializeField] private UnityEvent projectileColisionEvent;
    public int enemyLayer;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check what layer collided game object is
        if (collision.gameObject.layer == enemyLayer)
        {
            //event invoke for unity event can add to in editor
            projectileColisionEvent.Invoke();   
            //Destroy projectile
            Destroy(this.gameObject);
            //Use health script attached to collided bot to take health from enemy
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
      
        }


    }
    //For an exploding bullet: 
    //Collider2D collision = Physics2D.OverlapCircle(new Vector2 (0,0), 1, "Bot"); 
    //when it explodes
}
