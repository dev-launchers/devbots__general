using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Hitbox : MonoBehaviour, IHitBox
{
    [SerializeField] private Collider2D m_collider; // Sprite's hitbox
    [SerializeField] private Transform m_transform; 
    [SerializeField] private LayerMask m_layerMask; // Which layer to find enemies
    [SerializeField] private float m_thickness;
    [SerializeField] private bool canHit;

    private Collider2D hit; // Collider hit

    private IHitResponder m_hitResponder;
    private bool madeHit = false;
    public IHitResponder hitResponder { get => m_hitResponder; set => m_hitResponder = value; }

    public void canHitOn()
    {
        canHit = true;
    }

    public void canHitOff()
    {
        canHit = false;
    }

    public void CheckHit()
    {
        if (!canHit)
            return;

        Debug.Log("I am checking hits now");

        HitData _hitData = null;

        // Check for collision
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_transform.position, m_thickness, m_layerMask);
   
        // Iterate through objects collided
        foreach (Collider2D _hit in hitEnemies)
        {
            Debug.Log("we hit " + _hit.name);
            //BotController collisionController = _hit.GetComponent<BotController>();
            IHurtbox _hurtbox = _hit.GetComponent<IHurtbox>();

            //Debug.Log("Collision controller is not null: " + collisionController.name);
            if (m_hitResponder != null)
            {
                Debug.Log("hit responder is not null: " + m_hitResponder.ToString());
                if (_hurtbox.Active)
                {
                    Debug.Log("Hurtbox is active");
                    // Generate HitData
                    _hitData = new HitData
                    {
                        damage = m_hitResponder == null ? 0 : m_hitResponder.Damage,
                        hurtBox = _hurtbox,
                        hitDetector = this
                    };

                    // Validate a response
                    if (_hitData.Validate())
                    {
                        Debug.Log("hitdata is valid");
                        _hitData.hitDetector.hitResponder?.Response(_hitData);
                        _hitData.hurtBox.hurtResponder?.Response(_hitData);
                    }


                }
            }
                // Damaged is calculated using the damage param in Hitresponder as it communicates to the hurtbox of the thing with which it collided.          
                // collisionController.ApplyForce(new Vector2(knockback * sensor.GetNearestSensedBotDirection(), 0));
        }
        //throw new System.NotImplementedException();
    }

}
