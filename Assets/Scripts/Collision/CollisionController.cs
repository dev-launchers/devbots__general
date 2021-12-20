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
        // Which effect is determined first - hurtbox or hitdetector?
        // Can be changed to be more flexible 
        if(hurtBox != null)    // If the item does have a hurtbox 
        {
            Debug.Log("we hit a hurtbox TRUE: " + hurtBox.ToString());
            if (hurtBox.Checkhit(this))   // Check to see if this has a hurtbox to be hit
            {
                Debug.Log("the enemy checked it's hit TRUE: " + hurtBox.Checkhit(this));
                if (hurtBox.hurtResponder == null || hurtBox.hurtResponder.CheckHit(this))  // check if the hurtbox needs to compute anything for the parent
                {
                    Debug.Log("TRUE: " + hurtBox.hurtResponder + " OR " + hurtBox.hurtResponder.CheckHit(this));
                    if (hitDetector.hitResponder == null || hitDetector.hitResponder.CheckHit(this)) // Determine if the hitDetector needs to do anything to this hurtbox with the most recent data
                    {
                        return true;
                    }
                }   
            }   
        }
        Debug.Log("FALSE hurt box is null");
        return false;
    }
}

/// <summary>
/// HitResponders are interfaces on all objects that have effects onto other objects
/// when they collide with them. (Usually weapons)
/// </summary>
public interface IHitResponder
{
    // Data for hit responders is up for manipulation 

    int Damage { get; }
    // Method declaration for ALL things that hit - check to see if the attached game object 
    // hits something
    public bool CheckHit(HitData hitData);
    // Respond to hit method
    public void Response(HitData hitData);
}


public interface IHitDetector
{
    /// <summary>
    /// This is an interface attached to ALL game obejcts that need to determine if an obbject collided with this object
    /// this CheckHit() can be unique for each object.
    /// Create a script with BotPartName_HitBox and attach this script.
    /// </summary>
    public IHitResponder hitResponder { get; set; }
    public void CheckHit();
}

/// <summary>
/// This interface sends information to the parent bot object to determine whether or not computation
/// needs to be done to alter the response, (e.g. weight to resist knock back, slowed, shield, etc.)
/// </summary>
public interface IHurtResponder
{
    public bool CheckHit(HitData hitData);
    
    public void Response(HitData hitData);
}

/// <summary>
/// 
/// </summary>
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
