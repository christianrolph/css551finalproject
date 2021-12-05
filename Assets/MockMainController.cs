using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockMainController : MonoBehaviour
{
    public SliderWithEcho ShotPowerSlider;
    public CatapultControll TheCatapultControl;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.ShotPowerSlider != null);
        ShotPowerSlider.InitSliderRange(TheCatapultControl.MaxPulledBackCatapultArm, TheCatapultControl.MinPulledBackCatapultArm, 0);    // slider is initially on 0
        ShotPowerSlider.SetSliderListener(TheCatapultControl.SetShotPowerAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
