using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hurtresponder : MonoBehaviour, IHurtResponder
{
    [SerializeField] private bool m_targetable = true;
    [SerializeField] private int m_health = default;
    [SerializeField] private BotController m_controller = default;

    //[SerializeField] private Transform m_targetTransform;
    //[SerializeField] private Rigidbody m_rigidbody;

    private List<Enemy_Hurtbox> m_hurtboxes = new List<Enemy_Hurtbox>(); // If there are multiple hurtboxese per sprite, place this script in the most parent bot object.
    public BotController controller { get { return m_controller; } }

    public bool CheckHit(HitData hitData)
    {
        Debug.Log(this.gameObject + " checked a hit");
        return true;
    }

    public void Response(HitData hitData)
    {
        Debug.Log(this.gameObject + " lost " + hitData.damage + " health!");
        // Call the controller to take damage
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hurtboxes = new List<Enemy_Hurtbox>(GetComponentsInChildren<Enemy_Hurtbox>());
        Debug.Log(this.gameObject.name + " Hurtresponder's start function");
        foreach (Enemy_Hurtbox _hurtbox in m_hurtboxes)
        {
            _hurtbox.hurtResponder = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
