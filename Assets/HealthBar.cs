using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider underBar;
    [SerializeField] private Text healthText;
    [SerializeField] float sliderTextDelay = 0.5f;
    [SerializeField] BotController botController;
    private float testValue = 1f;    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = testValue; //botController.GetHP;
        underBar.value = testValue; //botController.GetHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            testValue -= 0.25f;
            StartCoroutine(UpdateSlider(testValue));
        }
    }
    public IEnumerator UpdateSlider(float currentHP)
    {       //When player takes damage the player HP slider value is set to the current health of the players bot
        float getHP = currentHP;
        healthSlider.value = getHP;
        yield return new WaitForSeconds(sliderTextDelay);
        underBar.value = getHP;
        healthText.text = $"{currentHP * 100}/100";
    }


}
