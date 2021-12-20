using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Hitresponder : MonoBehaviour, IHitResponder
{
    [SerializeField] private bool m_attack;
    [SerializeField] private int m_damange = 15;
    [SerializeField] private Sword_Hitbox m_hitbox;
    [SerializeField] private float m_knockback = default(float);
    [SerializeField] private Vector2 thrustForce = default(Vector2);

    public int Damage { get => m_damange; }
    public float knockback { get => m_knockback; }

    public bool CheckHit(HitData hitData)
    {
        return true;
    }

    public void Response(HitData hitData)
    {
        // Sword applies a knockback to the hurtbox
        Vector2 knockbackForce = new Vector2(knockback * hitData.hurtBox.Transform.position.x, 0);
        //hitData.hurtBox.Owner.GetComponent<BotController>().ApplyForce(knockbackForce);
        Debug.Log(this.gameObject.name + "Is responding");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
