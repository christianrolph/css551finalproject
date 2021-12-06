using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockMainController : MonoBehaviour
{
    public SliderWithEcho ShotPowerSlider;
    public SliderWithEcho AimAxisSlider;
    public CatapultControll TheCatapultControl;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.ShotPowerSlider != null);
        Debug.Assert(this.AimAxisSlider != null);

        ShotPowerSlider.InitSliderRange(
            TheCatapultControl.MaxPulledBackCatapultArm,
            TheCatapultControl.MinPulledBackCatapultArm,
            TheCatapultControl.InitialCatapultArmPosition);

        AimAxisSlider.InitSliderRange(
            TheCatapultControl.MaxAimAxisRotateLeft,
            TheCatapultControl.MaxAimAxisRotateRight,
            TheCatapultControl.InitialPositionAimAxis);
        //TheCatapultControl.MaxAimAxisRotateLeft,
        //    180f,
        //    0f);

        ShotPowerSlider.SetSliderListener(TheCatapultControl.SetShotPowerAngle);
        AimAxisSlider.SetSliderListener(TheCatapultControl.SetAimAxisAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
