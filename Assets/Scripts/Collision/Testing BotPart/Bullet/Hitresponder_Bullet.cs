using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitresponder_Bullet : MonoBehaviour, IHitResponder
{
    [SerializeField] private bool m_attack;
    [SerializeField] private int m_damange = 10;
    [SerializeField] private HitboxBullet m_bulletHitbox;

    public int Damage { get => m_damange; }



    // Start is called before the first frame update
    void Start()
    {
        m_bulletHitbox.hitResponder = this;
        Debug.Log(this.gameObject.name + " has assigned the hitboxes (HB) hitresponder (HR) to this HR script");
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(m_attack)
        //{
        //}
    }

    public bool CheckHit(HitData hitData)
    {
        Debug.Log("HitData DAMAGE = " + hitData.damage);
        Debug.Log("HitData HitDetector = " + hitData.hitDetector);
        Debug.Log("HitData hurtBox = " + hitData.hurtBox);

        return true;
    }

    public void Response(HitData hitData)
    {
        Debug.Log(this.gameObject.name + " Hitresponder's response function");
        Destroy(this.gameObject);
        // Implement Response to      
    }


}
