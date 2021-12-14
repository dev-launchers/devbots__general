using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  HitData is the data included to be sent to responders
/// </summary>
public class HitData
{
    // Data objects need 
    // TODO: Add more possible datas
    public int damage;
    public IHurtbox hurtBox;
    public IHitDetector hitDetector;

    public bool Validate()
    {
        // Can be changed to be more flexible 
        if(hurtBox != null)      
            if(hurtBox.Checkhit(this))           
                if(hurtBox.hurtResponder == null || hurtBox.hurtResponder.CheckHit(this))               
                    if (hitDetector.hitResponder == null || hitDetector.hitResponder.CheckHit(this))
                        return true;
        return false;
    }
}

public interface IHitResponder
{
    int Damage { get; }
    // Validates hit data
    public bool CheckHit(HitData hitData);
    // Respond to hit
    public void Response(HitData hitData);
}


public interface IHitDetector
{

    public IHitResponder hitResponder { get; set; }
    public void CheckHit();
}

public interface IHurtResponder
{
    public bool CheckHit(HitData hitData);
    
    public void Response(HitData hitData);
}

public interface IHurtbox
{
    public bool Active { get; }
    
    // Find owner: may have multiple hurtboxes
    public GameObject Owner { get; }

    // Determine where the hurtbox is
    public Transform Transform { get; }
    public IHurtResponder hurtResponder { get; set; }
    public bool Checkhit(HitData hitData);

}
