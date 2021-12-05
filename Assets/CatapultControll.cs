using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultControll : MonoBehaviour
{
    public Vector2 MovementVector;
    public float ShotPowerAngle;
    public float AimAxisAngle;
    public bool isFiring;
    public float ElapsedTime;   // how much time since last frame
    public float TotalTime;     // a time counter

    public MockMainController mockController;

    public Transform ArmNode;

    public float MaxPulledBackCatapultArm;
    public float MinPulledBackCatapultArm;
    public float FireAnimationAngleDelta;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.ArmNode != null);
        Debug.Assert(this.mockController != null);
        this.ShotPowerAngle = 0; // initial rotation should be 0
        this.MaxPulledBackCatapultArm = -125f;
        this.MinPulledBackCatapultArm = 0f;
        this.FireAnimationAngleDelta = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        this.ElapsedTime = Time.deltaTime;

        // if firing, rotate powerangle
        if(this.isFiring) // every quarter sec
        {
            AnimateShot(FireAnimationAngleDelta);
        }
    }

    public void AnimateShot(float angleChange)
    {
        // ensure we don't overshoot
        if (Math.Abs(angleChange) > Math.Abs(this.ShotPowerAngle))
        {
            // we would overshoot, so just stop at 0
            SetShotPowerAngle(0);
        } 
        else
        {
            SetShotPowerAngle(this.ShotPowerAngle + angleChange);
        }
        

        if (this.ShotPowerAngle >= MinPulledBackCatapultArm)
        {
            // reached end of animation
            // reset slider
            this.ShotPowerAngle = 0; // ensure a return to known point
            this.mockController.ShotPowerSlider.SetSliderValue(0);
            
            this.isFiring = false;
        }
    }

    public void SetMovementVector(Vector2 vectorToSet)
    {
        this.MovementVector = vectorToSet;
    }

    public void SetShotPowerAngle(float angle)
    {
        Vector3 p = Vector3.zero;

        // if not in rotation, next two lines of work would be wasted
        float dx = angle - this.ShotPowerAngle;
        Quaternion q = Quaternion.AngleAxis(dx, Vector3.right);

        this.ArmNode.localRotation *= q;

        this.ShotPowerAngle = angle;
        //Debug.Log("SHOT POWER = " + ShotPowerAngle);
    }

    public void SetAimAxisAngle(float angle)
    {
        this.AimAxisAngle = angle;
        //Debug.Log("AIM AXIS ANGLE = " + AimAxisAngle);
    }

    public void Fire()
    {
        if (isFiring)
        {
            //return false;
        }
        else
        {
            this.isFiring = true;
            //return true;
        }
    }
}
