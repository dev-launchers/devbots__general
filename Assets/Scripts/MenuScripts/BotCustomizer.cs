using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCustomizer : MonoBehaviour
{

    public GameObject targetBot;
    public GameObject targetSlot;

    public List<GameObject> options = new List<GameObject>();

    private int currentOption = 0;

    public void NextOption() {
        currentOption++;
        if (currentOption >= options.Count) {
            currentOption = 0;
        }
        UpdateSlot();
    }

    public void PrevOption() {
        currentOption--;
        if (currentOption < 0) {
            currentOption = options.Count-1;
        }
        UpdateSlot();
    }

    public void UpdateSlot() {
        //clunky?
        GameObject newTarget = Instantiate(options[currentOption], targetSlot.transform.position, targetSlot.transform.rotation, targetBot.transform);
        Destroy(targetSlot);
        targetSlot = newTarget;
    }
}
