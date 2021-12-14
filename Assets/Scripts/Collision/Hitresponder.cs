using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitresponder : MonoBehaviour, IHitResponder
{
    [SerializeField] private bool m_attack;
    [SerializeField] private int m_damange = 10;
    [SerializeField] private Hitbox m_hitbox;

    public int Damage { get => m_damange; }



    // Start is called before the first frame update
    void Start()
    {
        m_hitbox.hitResponder = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_attack)
        {
            // Debug.Log("Collided and hit");
            // m_hitbox.CheckHit();
        }
    }

    public bool CheckHit(HitData hitData)
    {
        Debug.Log("Hit for " + hitData.damage);
        return true;
    }

    public void Response(HitData hitData)
    {
        // Implement Response to      
    }


}
