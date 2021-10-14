using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// class used to move slider over time
/// </summary>
public class SmoothSlider
{
    //slider that will be used
    private Slider slider;
    //the target value that the slider is moved to
    private float target;
    //the velocity of the slider
    private float velocity = 0f;
    //the time it takes for the slider to change from one value to the next
    float sliderValueChangeTime;

    /// <summary>
    /// Create smooth slider class and set the slider and slider value change 
    /// </summary>
    /// <param name="_slider">Set the slider that this class will be using</param>
    /// <param name="_sliderValueChangeTime">the value of the time it takes for the slider to change from one vale to the next</param>
    public SmoothSlider(Slider _slider, float _sliderValueChangeTime)
    {

        slider = _slider;
        target = slider.value;
        sliderValueChangeTime = _sliderValueChangeTime;
    }
    // used to get the smooth damp value of the slider value
    private float SmoothDamp()
    {
        return Mathf.SmoothDamp(slider.value, target, ref velocity, sliderValueChangeTime);
    }
    //Used to update the current slider value in a monobehaviour update method
    public void SliderUpdate()
    {
        slider.value = SmoothDamp();
    }
    //used to set the current value of the slider
    public void SetSliderValue(float value)
    {
        target = value;
    }
}


