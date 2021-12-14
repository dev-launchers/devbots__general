using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hit box takes in hit data from hurtboxes
/// </summary>
public class Hitbox : MonoBehaviour, IHitDetector
{
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private LayerMask m_layerMask;
    private Collider2D hit;

    private float m_thickness = 0.025f;
    private IHitResponder m_hitResponder;

    public IHitResponder hitResponder { get => m_hitResponder; set => m_hitResponder = value; }

    /// <summary>
    /// Using OnCollisionEnter2D for bullet projectile as it's animation may not collide with anything 
    /// therefore we only need to CheckHit if collision is detected first.
    /// </summary>
    /// <param name="collision"> object bullet collided with </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collided with " + collision.gameObject.name);
        CheckHit();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collided with " + collision.gameObject.name);
        hit = collision;
        CheckHit();
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckHit()
    {
        //Vector2 m_startSize = this.transform.position;

        //// The size of the box collider on the gameobject
        ////Vector2 _scaledSize = new Vector2(
        ////    m_collider.size.x * transform.localScale.x,
        ////    m_collider.size.y * transform.localScale.y);


        ////float _distance = _scaledSize.y - m_thickness;
        ////float _direction = Vector2.Angle(m_startSize, _scaledSize);

        ////Vector2 _center = m_collider.offset;
        ////Vector2 _start = m_collider.offset - (_scaledSize / 2);
        ////Vector2 _halfExtent = new Vector2(_scaledSize.x, m_thickness) / 2;
        ////Quaternion _orientation = transform.rotation;

        HitData _hitData = null;
        IHurtbox _hurtbox = null;

        //print("Casting Box");
        //RaycastHit2D[] _hits = Physics2D.BoxCastAll(
        //    m_collider.bounds.center,
        //    m_collider.bounds.size,
        //    0f,
        //    Vector2.up,
        //    m_thickness,
        //    m_layerMask);

        _hurtbox = hit.GetComponent<IHurtbox>();
        if(m_hitResponder != null)
        {
            Debug.Log("Hit " + hit.name);
            if(_hurtbox.Active)
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

        //foreach (RaycastHit2D _hit in _hits)
        //{
        //    _hurtbox = _hit.collider.GetComponent<IHurtbox>();
        //    if (_hurtbox != null)
        //    {
        //        Debug.Log("Hit {0}" + _hits);
        //        if (_hurtbox.Active)
        //        {
        //            // Generate HitData
        //            _hitData = new HitData
        //            {
        //                damage = m_hitResponder == null ? 0 : m_hitResponder.Damage,
        //                hurtBox = _hurtbox,
        //                hitDetector = this
        //            };

        //            // Validate a response
        //            if (_hitData.Validate())
        //            {
        //                _hitData.hitDetector.hitResponder?.Response(_hitData);
        //                _hitData.hurtBox.hurtResponder?.Response(_hitData);
        //            }
        //        }
        //    }
        //}

    }
}
