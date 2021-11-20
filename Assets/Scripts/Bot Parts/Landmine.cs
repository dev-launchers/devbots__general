using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{

    private float damage;
    private int enemyLayer;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetValues(float dmg, Vector3 size, int layer)
    {
        damage = dmg; //Set the damage of the projectile
        gameObject.transform.localScale = size; //Set the projectile size
        enemyLayer = layer; //Set the target of the projectile, so it only hits the desired bot, will likely need to be array of layers for self-damaging items 
    }

    // TO-DO make the landmine stick to surfaces
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check what layer collided game object is
        if (collision.gameObject.layer == enemyLayer)
        {
            //event invoke for unity event. can add to in editor
            //landmineColisionEvent.Invoke();   

            //Deal damage to collided enemy
            collision.gameObject.GetComponent<BotController>().TakeDamage(damage);

            //Destroy landmine if hit by an enemy
            Destroy(gameObject);
        }

        


    }
}
