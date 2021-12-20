using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hit box takes in hit data from hurtboxes
/// </summary>
public class Base_Hitbox : MonoBehaviour, IHitDetector
{
    [SerializeField] private Collider2D m_collider; // Sprite's hitbox
    [SerializeField] private LayerMask m_layerMask; // Which layer to find enemies
    private Collider2D hit; // Collider hit

    private float m_thickness = 0.025f; // arbitrary thickness to test
    private IHitResponder m_hitResponder;

    public IHitResponder hitResponder { get => m_hitResponder; set => m_hitResponder = value; }

    public void CheckHit()
    {
        Debug.Log(this.gameObject.name + " Is checking hit now.");
    }
}
