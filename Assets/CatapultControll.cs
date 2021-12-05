using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultControll : MonoBehaviour
{
    public Vector2 MovementVector;
    public float ShotPowerAngle;
    public float AimAxisAngle;
    public bool isFiring;

    public float MaxPulledBackCatapultArm = -125;
    public float MinPulledBackCatapultArm = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovementVector(Vector2 vectorToSet)
    {
        this.MovementVector = vectorToSet;
    }

    public void SetShotPowerAngle(float angle)
    {
        this.ShotPowerAngle = angle;
        //Debug.Log("SHOT POWER = " + ShotPowerAngle);
    }

    public void SetAimAxisAngle(float angle)
    {
        this.AimAxisAngle = angle;
        //Debug.Log("AIM AXIS ANGLE = " + AimAxisAngle);
    }

    public bool Fire()
    {
        if (isFiring)
        {
            return false;
        }
        else
        {
            // do some fire stuff
            return true;
        }
    }
}
