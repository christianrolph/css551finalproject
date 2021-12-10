using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultControll : MonoBehaviour
{
    public Vector2 MovementVector;
    public float ShotPowerAngle;
    public float LastFiredPositionAngle;
    public float AimAxisAngle;
    public bool isFiring = false;               // for firing animation
    public bool isPullingBackArm = false;       // for pull back arm animation
    public float ElapsedTime;   // how much time since last frame
    public float TotalTime;     // a time counter

    public MockMainController mockController;

    public Transform ArmNode;
    public Transform AimAxisNode;
    public Transform BaseNode;

    public bool CreateNewProjectile;

    public float MaxPulledBackCatapultArm;
    public float MinPulledBackCatapultArm;
    public float InitialCatapultArmPosition;
    
    public float FireAnimationAngleDelta;
    public float PullBackAnimationAngleDelta;

    public float MaxAimAxisRotateLeft;
    public float MaxAimAxisRotateRight;
    public float InitialPositionAimAxis;

    // for launch physics
    public float LaunchSpeedScale = 125f;
    public float GravitationPull = -50f;

    public bool TEST_MODE = false;

    private void Awake()
    {
        if (!TEST_MODE)
        {
            Debug.Assert(this.ArmNode != null);
            Debug.Assert(this.mockController != null);
        }
        this.ShotPowerAngle = 0; // initial rotation should be 0
        this.MaxPulledBackCatapultArm = -125f;
        this.MinPulledBackCatapultArm = 0f;
        this.InitialCatapultArmPosition = 0f;
        this.FireAnimationAngleDelta = 7f;
        this.PullBackAnimationAngleDelta = -1f;

        this.MaxAimAxisRotateLeft = -180f;
        this.MaxAimAxisRotateRight = 180f;
        this.InitialPositionAimAxis = 0f;

        this.MovementVector = Vector2.zero;

        this.CreateNewProjectile = false;   // initially not launching a projectile
    }
    // Start is called before the first frame update
    void Start()
    {
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
        else if (this.isPullingBackArm)
        {
            AnimatePullBackArm(PullBackAnimationAngleDelta);
        }

        // move catapult
        MoveToNextPosition();
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
            this.ShotPowerAngle = 0; // go to known position
            this.mockController.ShotPowerSlider.SetSliderValue(0);
            
            this.isFiring = false;

            // instantiate and fire new projectile
            this.CreateNewProjectile = true;
        }
    }

    public void AnimatePullBackArm(float angleChange)
    {
        // ensure we don't overshoot
        //if (Math.Abs(angleChange + this.ShotPowerAngle) > Math.Abs(this.LastFiredPositionAngle))
        if (Math.Abs(this.ShotPowerAngle + angleChange) > Math.Abs(this.LastFiredPositionAngle))
        {
            // we would overshoot, so just stop at 0
            SetShotPowerAngle(this.LastFiredPositionAngle);
        }
        else
        {
            SetShotPowerAngle(this.ShotPowerAngle + angleChange);
        }


        if (this.ShotPowerAngle <= this.LastFiredPositionAngle)
        {
            // reached end of animation
            // reset slider
            SetShotPowerAngle(this.LastFiredPositionAngle); // go to known position
            this.mockController.ShotPowerSlider.SetSliderValue(this.ShotPowerAngle);

            this.isPullingBackArm = false;
        }
    }

    public void PullArmToLastFiringPosition()
    {
        // pull catapult arm back to firing position
        SetShotPowerAngle(this.LastFiredPositionAngle);
        this.mockController.ShotPowerSlider.SetSliderValue(this.ShotPowerAngle);    // angle should have been updated by SetShotPowerAngle
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

        if (!TEST_MODE)
        {
            this.ArmNode.localRotation *= q;
        }

        this.ShotPowerAngle = angle;
        //Debug.Log("SHOT POWER = " + ShotPowerAngle);
    }

    public void SetAimAxisAngle(float angle)
    {
        Vector3 p = Vector3.zero;

        // if not in rotation, next two lines of work would be wasted
        float dy = angle - this.AimAxisAngle;
        Quaternion q = Quaternion.AngleAxis(dy, Vector3.up);

        this.AimAxisNode.localRotation *= q;

        this.AimAxisAngle = angle;
        //Debug.Log("AIM AXIS ANGLE = " + AimAxisAngle);
    }

    public void Fire()
    {
        // if we're currently in animation
        if (isFiring || isPullingBackArm)
        {
            // do nothing
        }
        else
        {
            if (!TEST_MODE)
            {
                this.LastFiredPositionAngle = this.ShotPowerAngle;
                this.isFiring = true;
            }
        }
    }

    public void MoveToNextPosition()
    {
        // get speed and direction
        float speed = this.MovementVector.magnitude;
        Vector3 adjustIntoV3 = new Vector3(this.MovementVector.y, 0, (-1) * this.MovementVector.x);  // z direction originally going backwards before -1
        Vector3 direction = adjustIntoV3.normalized;

        // calculate next direction
        Vector3 currentPostion =  this.BaseNode.transform.localPosition;
        Vector3 nextPosition = currentPostion + (this.ElapsedTime * speed * direction);

        // move to next direction
        this.BaseNode.transform.position = nextPosition;
    }
}
