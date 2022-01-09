using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hit box takes in hit data from hurtboxes
/// </summary>
public class GunPart_Projectile_Hitbox : MonoBehaviour, IHitBox
{
    [SerializeField] private Collider2D m_collider; // Sprite's hitbox
    [SerializeField] private LayerMask m_layerMask; // Which layer to find enemies
    [SerializeField] private HurtboxMask m_hurtboxMask = HurtboxMask.Enemy;
    private Collider2D hit; // Collider hit

    private float m_thickness = 0.25f; // arbitrary thickness to test
    private IHitResponder m_hitResponder;
    private bool madeHit = false;

    public IHitResponder hitResponder { get => m_hitResponder; set => m_hitResponder = value; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<IHurtbox>() != null)
        {
            madeHit = true;
            hit = collision.collider;
            CheckHit();
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IHurtbox>() != null)
        {
            madeHit = true;
            hit = collision;
            CheckHit();
            Destroy(this);
        }
    }

    public void CheckHit()
    {
        HitData _hitData = null;
        IHurtbox _hurtbox = hit.GetComponent<IHurtbox>();

        if (m_hitResponder != null)
        {
            Debug.Log("Hit " + hit.name);
            if (_hurtbox.Active)
            {
                if (m_hurtboxMask.HasFlag((HurtboxMask)_hurtbox.Type))
                {
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
                        _hitData.hitDetector.hitResponder?.Response(_hitData);
                        _hitData.hurtBox.hurtResponder?.Response(_hitData);
                    }
                }
            }
        }
    }
}
