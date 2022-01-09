using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Hurtbox : MonoBehaviour, IHurtbox
{
    [SerializeField] private bool m_active = false;
    [SerializeField] private GameObject m_owner = null;
    [SerializeField] private HurtBoxType m_hurtboxType = HurtBoxType.Player;    
    private IHurtResponder m_hurtResponder;
    public bool Active { get => m_active; }
    public GameObject Owner { get => m_owner; }
    public Transform Transform { get => transform; }
    public IHurtResponder hurtResponder { get => m_hurtResponder; set => m_hurtResponder = value; }
    public HurtBoxType Type { get => m_hurtboxType;}


    /// <summary>
    /// This determines whether or not the collision with the bot results in an reaction
    /// armor (reduce damage), do damage, use a shield, etc.
    /// </summary>
    /// <param name="hitData"></param>
    /// <returns></returns>
    public bool Checkhit(HitData hitData)
    {
        if (m_hurtResponder == null)
        {
            Debug.Log("No responder");
            return false;
        }
         
        Debug.Log(this.gameObject + " confirming hit");
        return true;
    }
}

