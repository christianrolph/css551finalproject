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

    // for predictive aim
    public List<GameObject> PredictiveAimSpheres;
    public AimSphere LaunchAimSphere;    // where projectiles will be launched from for predictive aim
    public int NumOfAimPoints;
    public float ProjectileAliveTime;

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
    TheWorld world;

    // for the camera
    public SmallCameraControl SmallCamera;

    private void Awake()
    {
        world = FindObjectOfType<TheWorld>();
        this.LaunchAimSphere = FindObjectOfType<AimSphere>();
        if (!TEST_MODE)
        {
            Debug.Assert(this.ArmNode != null);
            Debug.Assert(this.mockController != null);
            Debug.Assert(this.LaunchAimSphere != null);
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

        Debug.Assert(this.SmallCamera != null);

        this.NumOfAimPoints = 100;
        this.ProjectileAliveTime = 1.15f;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.ElapsedTime = Time.deltaTime;

        // update predictive aim
        SetPredictiveAim(this.ShotPowerAngle);

        // if firing, rotate powerangle
        if (this.isFiring) // every quarter sec
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
        //Vector3 currentPostion =  this.BaseNode.transform.localPosition;
        Vector3 currentPostion = this.world.transform.localPosition;
        Vector3 nextPosition = currentPostion + (this.ElapsedTime * speed * direction);

        // move to next direction
        //this.BaseNode.transform.position = nextPosition;
        this.world.transform.position = nextPosition;
    }

    public void SetSmallCamera(SmallCameraControl smallCamera)
    {
        this.SmallCamera = smallCamera;
    }
    public void SetPredictiveAim(float fireAngle)
    {
        // clear any existing aim spheres
        foreach (GameObject aimSphere in this.PredictiveAimSpheres)
        {
            GameObject.Destroy(aimSphere);
        }

        // disable the aim sphere
        this.LaunchAimSphere.gameObject.SetActive(false);

        // only do this if we're not animating
        if (isFiring == false && isPullingBackArm == false && ShotPowerAngle != 0)
        {
            // get the time offset
            float timeOffset = this.ProjectileAliveTime / this.NumOfAimPoints;

            // set initial variables
            float size = transform.localScale.y / 10f;
            Vector3 gravitationPull = this.GravitationPull * Vector3.up;    // default is dropping downwards
            Vector3 acceleration = Vector3.zero;                            // default is dropping downwards
            Vector3 velocity = size * Math.Abs(fireAngle) * (this.LaunchAimSphere.transform.up + this.LaunchAimSphere.transform.forward).normalized;
            Vector3 aimPosition = this.LaunchAimSphere.transform.localPosition;

            // create new ones, skip the first (AimSphere counts as first)
            for (int aimPointNumber = 1; aimPointNumber <= this.NumOfAimPoints; aimPointNumber++)
            {
                // calculate it's position, aimPointNum times time
                // simulates the passage of time
                acceleration += gravitationPull * (aimPointNumber * timeOffset);    // change in acceleration
                velocity += acceleration * (aimPointNumber * timeOffset);           // change in velocity
                aimPosition += velocity * (aimPointNumber * timeOffset);
                Vector3 newAimPointPosition = aimPosition;

                // instantiate, and set scale, position
                // add to list of all aim spheres
                GameObject aimPointGameObj = Instantiate(Resources.Load("Prefabs/PredictiveAimPoint")) as GameObject;
                aimPointGameObj.transform.localScale = this.LaunchAimSphere.transform.localScale;
                aimPointGameObj.transform.localPosition = newAimPointPosition;
                AimSphere sphere = aimPointGameObj.GetComponent<AimSphere>();
                this.PredictiveAimSpheres.Add(sphere.gameObject);
            }

            // reenable the aim sphere
            this.LaunchAimSphere.gameObject.SetActive(true);
        }
    }
}
