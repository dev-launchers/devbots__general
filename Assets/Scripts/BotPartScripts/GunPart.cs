using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPart : MonoBehaviour, IBotPart
{

    [SerializeField] private LayerMask enemy = default(LayerMask);
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float attackSize = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    [SerializeField] private GameObject projectile = default(GameObject);//Object to be fired by gun part
    [SerializeField] private float projectileSpeed = default(float);
    [SerializeField] private GameObject projectileStartPos;
    public void SetState(State state)
    {
        return;
    }

    public void AttackStep(List<GameObject> activeBots)
    {
        //Called each turn, checks for collision and calls its TakeDamage
        
        int enemyDirection = GetComponentInParent<BotSensor>().GetNearestSensedBotDirection();

        attackPos.localPosition = new Vector3(enemyDirection * attackDistance, 0, 0); //Faces attack at enemy, handled as local position to parent Bot

        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, attackSize, enemy);
        //OverlapBox for rectangular hitbox

        if (collision != null)
        {Debug.Log("Attack");
            var firePos = GetComponentInChildren<Transform>().localPosition;//Get the position of the start position of projetile
         GameObject projectileInstance=  Instantiate(projectile, projectileStartPos.transform.position, Quaternion.identity);//Create projectile at the start position
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
            projectileScript.enemyDirection = enemyDirection;//set the direction of the projectile
            projectileScript.damage = damage;//etState the damage of the projectile
            projectileScript.speed = projectileSpeed;//set the speed of the projectile

            //collision.GetComponent<Health>().TakeDamage(damage);
            //collision.GetComponent<Move>().TakeKnockback(enemyDirection, knockback);
        }
    }
    void OnDrawGizmosSelected()
    {
        //Test function to draw hitbox for attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
