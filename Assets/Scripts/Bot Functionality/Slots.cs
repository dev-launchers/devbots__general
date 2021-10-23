using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is where the bots botpart slots are located 
/// </summary>
[System.Serializable]
public class Slots
{
    [Header("These are the game objects used as slots on the bot where the bot's botpart gameobject's belong")]
    public GameObject top;
    public GameObject side;
    public GameObject bottom;
    public GameObject back;
    
    //List of all the slots
    private List<GameObject> slots;


    public Slots()
    {
        SetSlots();
    }
    //Initiate slots list by adding all slots to list
    private void SetSlots()
    {
        slots = new List<GameObject>();
        slots.Add(top);
        slots.Add(side);
        slots.Add(bottom);
        slots.Add(back);
    }

 
    //Get the slot gameobject by using the slotpositioon enum
    private GameObject GetSlot(SlotPosition slotPosition)
    {
        GameObject slot = default(GameObject);
        //match slotposition to correct slot
        switch (slotPosition)
        {
            case SlotPosition.Top:
                slot = top;
                break;
            case SlotPosition.Side:
                slot = side;
                break;
            case SlotPosition.Bottom:
                slot = bottom;
                break;
            case SlotPosition.Back:
                slot = back;
                break;
            default:
                break;
        }
        //return correct gamebject located at slot
        return slot;
    }

    //check to see if botpart is in slot position
    public bool IsBotPartInSlot(SlotPosition slotPosition)
    {
        //get slot using slot position
        var slot = GetSlot(slotPosition);
        return slot.GetComponent<BotPart>();
    }

    //Set a new botpart in the botpart slot 
    public void SetSlotBotPart(SlotPosition slotPosition, GameObject botPartGameObject)
    {
        //get slot using slot position
        var slot = GetSlot(slotPosition);
        //check if any other gameobjects are located on this slot
        if (slot.transform.childCount > 0)
        {
            //destroy gameobject on this slot
            Object.Destroy(GetSlotBotPartGameObject(slot));
        }
        //Instantiate botpart gameobject and parent it to this slot
        Object.Instantiate(botPartGameObject, slot.transform.position, slot.transform.rotation, slot.transform);

    }

    //Get the botpart gameobject located at this slot
    private GameObject GetSlotBotPartGameObject(GameObject slot)
    {
        return slot.transform.GetChild(0).gameObject;
    }

    //Get the  botpart located at this slot
    public BotPart GetSlotBotPart(SlotPosition slotPosition)
    {
        GameObject slot = GetSlot(slotPosition);
        return slot.GetComponentInChildren<BotPart>();
    }

}
