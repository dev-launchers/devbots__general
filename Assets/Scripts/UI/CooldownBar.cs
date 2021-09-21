using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [Tooltip("Used to pick which slot position tis cooldown bar belongs to")]
    [SerializeField] private SlotPosition slotPosition;
    [Tooltip("The slider for the cooldown bar")]
    [SerializeField] Slider slider; 
    //The botcontroller this cooldown bar belongs to
    private BotController botController;
    //The botpart that this cooldown bar is going to be using
    BotPart botPart;


    // Start is called before the first frame update
    void Start()
    {
        //get the botcontroller located in HealthAndCDHoolder script
        botController = transform.parent.GetComponentInParent<HealthAndCDHolder>().GetBotController();
        //get the botpart
        botPart = botController.slots.GetSlotBotPart(slotPosition);
        
    }

    // Update is called once per frame
    void Update()
    {
        ///Set slider maxvalue to botpart cooldown time
        if (slider.maxValue != botPart.GetCoolDown())
        {
            slider.maxValue = botPart.GetCoolDown();
        }
          //Update the value of the slider
            slider.value = botPart.GetCoolDown() - botPart.GetCoolDownTimer();
        
    }
}
