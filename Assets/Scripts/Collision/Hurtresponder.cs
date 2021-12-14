using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtresponder : MonoBehaviour, IHurtResponder
{
    [SerializeField] private bool m_targetable = true;
    //[SerializeField] private Transform m_targetTransform;
    //[SerializeField] private Rigidbody m_rigidbody;

    private List<Hurtbox> m_hurtboxes = new List<Hurtbox>();
    public bool CheckHit(HitData hitData)
    {
        return true;
    }

    public void Response(HitData hitData)
    {
        Debug.Log("hurt response");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hurtboxes = new List<Hurtbox>(GetComponentsInChildren<Hurtbox>());
        foreach(Hurtbox _hurtbox in m_hurtboxes)
        {
            _hurtbox.hurtResponder = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
