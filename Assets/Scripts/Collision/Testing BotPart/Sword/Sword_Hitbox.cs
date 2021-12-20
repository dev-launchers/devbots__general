using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Hitbox : MonoBehaviour, IHitDetector
{
    [SerializeField] private Collider2D m_collider; // Sprite's hitbox
    [SerializeField] private LayerMask m_layerMask; // Which layer to find enemies
    private Collider2D hit; // Collider hit

    private float m_thickness = 0.25f; // arbitrary thickness to test
    private IHitResponder m_hitResponder;
    private bool madeHit = false;
    public IHitResponder hitResponder { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void CheckHit()
    {
        
        throw new System.NotImplementedException();
    }

}
