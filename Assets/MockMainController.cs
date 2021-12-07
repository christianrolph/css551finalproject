using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockMainController : MonoBehaviour
{
    public SliderWithEcho ShotPowerSlider;
    public SliderWithEcho AimAxisSlider;
    public CatapultControll TheCatapultControl;

    // these objects are for mocking and dev only,
    // they're not for prod
    public AxisFrameControl AxisFrame;
    public TestCube TheTestCube;
    public bool Debugging;

    // Start is called before the first frame update
    void Start()
    {
        Debugging = false;
        Debug.Assert(this.ShotPowerSlider != null);
        Debug.Assert(this.AimAxisSlider != null);
        Debug.Assert(this.AxisFrame != null);
        Debug.Assert(this.TheTestCube != null);
        if (!Debugging)
        {
            this.AxisFrame.gameObject.SetActive(false);
            this.TheTestCube.gameObject.SetActive(false);
        }

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
