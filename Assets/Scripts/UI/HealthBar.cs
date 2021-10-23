using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public partial class HealthBar : MonoBehaviour
{
    [Tooltip("Slider for healthbar")]
    [SerializeField] private Slider healthSlider;

    [Tooltip("Slider for red bar underneath main healthbar")]
    [SerializeField] private Slider underBar;

    [Tooltip("Text that shows current health amount")]
    [SerializeField] private Text healthText;

    [Tooltip("Delay time for underbarbar and text")]
    [SerializeField] float sliderTextDelay = 0.5f;

    [Tooltip("Time it takes for sliders to change from one value to the next")]
    [SerializeField] float sliderValueChangeTime = 0.25f;

    [Tooltip("The Healthbar changes to red when health perccentage is lower than this amount")]
    [SerializeField] float spriteChangePercentage = 0.25f;

    [Header("Sprites for healthbar")]
    [SerializeField] Sprite redBar;
    [SerializeField] Sprite greenBar;

    [Header("Test with mmouse clicks to see if healthbars work")]
    [SerializeField] bool debug = false;

    //image renderer located on main healthbar slider needed to change sprite dor health bar slider
    private Image imageRend;

    //The currrent value that the healthbar has
    private float healthBarValue = 1f;

    //Scripts used to make the slider values change overtime rather thann straight away
    SmoothSlider healthBarSmoothSlider;
    SmoothSlider underBarSmoothSlider;


    //Get the current value of the healthbar
    public float GetHealthBarValue()
    {
        return healthBarValue;
    }
    //Set a new value for the healthbar
    public void SetSliderValue(float value)
    {
        healthBarValue = value;
        //Start update sliders coroutine which changes the value of each slider
        StartCoroutine(UpdateSliders(value));
    }


    // Start is called before the first frame update
    void Start()
    {
        //Set the both sliders value to the starting health
        healthSlider.value = GetHealthBarValue();
        underBar.value = GetHealthBarValue();
        //Create new Smoothsliders for each slider
        healthBarSmoothSlider = new SmoothSlider(healthSlider, sliderValueChangeTime);
        underBarSmoothSlider = new SmoothSlider(underBar, sliderValueChangeTime);
        //get the image renderer for the healthbar slider and assign it to imageRend
        imageRend = healthSlider.GetComponentInChildren<Image>();


    }

    // Update is called once per frame
    void Update()
    {
        //If deebbug is true left mouse click takes health and right click resets health to starting health
        if (debug)
        {

            if (Input.GetMouseButtonDown(0))
            {
                SetSliderValue(GetHealthBarValue() - 0.25f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetSliderValue(1f);

            }
        }


        //check to see if healthbar value is under percentage 
        if (healthBarValue <= spriteChangePercentage && imageRend.sprite != redBar)
        {
            //change healthbar sprite to red bar
            imageRend.sprite = redBar;
        }
        else if (healthBarValue > spriteChangePercentage && imageRend.sprite != greenBar)
        {
            //change healthbar sprite to green bar
            imageRend.sprite = greenBar;
        }
        //Update both sliders usiing smoothslider class
        healthBarSmoothSlider.SliderUpdate();
        underBarSmoothSlider.SliderUpdate();
    }


    //method used to change both sliders values and text with the underbar and text values changing at a delay
    IEnumerator UpdateSliders(float currentHP)
    {
        //set value of main health bar
        healthBarSmoothSlider.SetSliderValue(currentHP);
        //Delay used to change underbar and healthbar text
        yield return new WaitForSeconds(sliderTextDelay);
        //set value of under bar
        underBarSmoothSlider.SetSliderValue(currentHP);
        //set the value of the health text
        healthText.text = $"{currentHP * 100}/100";
    }

}
