using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPart : BotPart, IHitResponder
{

    private Vector2 attackPos = default(Vector2);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private float knockback = default(float);
    [SerializeField] private Vector2 thrustForce = default(Vector2);
    [SerializeField] private bool isRunning;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Sword_Hitbox hitbox;

    private Animator swordAnimator;//Animator used for sword rotation
    private Rigidbody2D rb;
    private BotSensor sensor;

    // HitResponder data
    public float Damage { get => damage; }

    public override void SetState(State state) {
        isRunning = state.isActive;
    }

    private void Start()
    {
        
        swordAnimator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sensor = GetComponentInParent<BotSensor>();
        //enemyLayer = sensor.GetEnemyLayer();
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

                // Calling check hit to validate collision
                hitbox.CheckHit();                               
            }
        }
    }

    public override void BotPartUpdate()
    {
        AttackStep();
    }

    /* HitResponder Methods */
    public bool CheckHit(HitData hitData)
    {
        // No addition checks required for sword 
        return true;
    }

    public void Response(HitData hitData)
    {
        // No response to successful hit 
    }
}
