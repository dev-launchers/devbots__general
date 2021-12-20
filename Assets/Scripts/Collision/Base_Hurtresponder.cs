using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Hurtresponder : MonoBehaviour, IHurtResponder
{
    [SerializeField] private bool m_targetable = true;
    //[SerializeField] private Transform m_targetTransform;
    //[SerializeField] private Rigidbody m_rigidbody;

    private List<Base_Hurtbox> m_hurtboxes = new List<Base_Hurtbox>(); // If there are multiple hurtboxese per sprite, place this script in the most parent bot object.
    public bool CheckHit(HitData hitData)
    {
        Debug.Log(this.gameObject + " checked a hit");
        return true;
    }

    public void Response(HitData hitData)
    {
        Debug.Log(this.gameObject + " reacted with: hurt response");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hurtboxes = new List<Base_Hurtbox>(GetComponentsInChildren<Base_Hurtbox>());
        
        foreach (Base_Hurtbox _hurtbox in m_hurtboxes)
        {
            Debug.Log(this.gameObject.name + " hurtbox: " + _hurtbox);
            _hurtbox.hurtResponder = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
