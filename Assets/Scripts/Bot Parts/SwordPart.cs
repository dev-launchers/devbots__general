using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPart : BotPart
{

    [SerializeField] private LayerMask enemy = default(LayerMask);
    [SerializeField] private Transform attackPos = default(Transform);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    [SerializeField] private float forwardThrustForce = default(float);
    private bool isRunning;
    private float timer;
    private const float COOLDOWN = 2.0f;
    private Animator swordAnimator;//Animator used for sword rotation
    private Rigidbody2D botRigidbody;
    public override void SetState(State state) {
        return;
    }
    private void Start()
    {
        swordAnimator = GetComponent<Animator>();
        botRigidbody = GetComponentInParent<Rigidbody2D>();
        isRunning = true;
    }
    private void Update()
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
                                  //get the enemy gameobject which is closest using the sensor script
                                  //var enemy = sensor.GetNearestSensedBot().gameObject;

                Collider2D collision = Physics2D.OverlapCircle(transform.position, attackDistance, enemy);
                //OverlapBox for rectangular hitbox

                if (collision != null)
                {
                    // Set trigger to play animation of sword rotating 
                    swordAnimator.SetTrigger("swordAttack");
                    // add thrust to lunge bot forwaard 
                    botRigidbody.AddRelativeForce(new Vector2(forwardThrustForce, 0), ForceMode2D.Impulse);
                    collision.GetComponent<BotController>().TakeDamage(damage);
                    collision.GetComponent<BotController>().ApplyForce(new Vector2(-knockback,0));
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Display the attack radius when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }
}
