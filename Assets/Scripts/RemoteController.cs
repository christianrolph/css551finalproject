using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemoteController : MonoBehaviour
{
    public ControlStick AimAxis;
    public ControlStick CatapultTranslation;
    public ControlStick ShotPower;
    public TMP_Text AimEcho;
    public TMP_Text ShotEcho;

    CatapultControll catapultControll;
    const int MAX_ANGLE = 50;

    private void Start()
    {
        catapultControll = FindObjectOfType<CatapultControll>();
        Debug.Log(AimAxis != null);
        Debug.Log(CatapultTranslation != null);
        Debug.Log(ShotPower != null);
        Debug.Log(AimEcho != null);
        Debug.Log(ShotEcho != null);
        AimEcho.text = "Aim Axis:\n0" + (char)176;
        ShotEcho.text = "Shot Angle:\n0" + (char)176;
    }

    public void FireProjectile()
    {
        if (!catapultControll.isFiring)
        {
            ShotEcho.text = "Shot Angle:\n0" + (char)176;
            catapultControll.Fire();
            ShotPower.SnapBack();
        }
    }

    public void CalculateAimAxis(float aimAngle)
    {
        aimAngle *= catapultControll.MaxAimAxisRotateLeft / MAX_ANGLE;
        aimAngle = Mathf.Clamp(aimAngle, catapultControll.MaxAimAxisRotateLeft, catapultControll.MaxAimAxisRotateRight);
        if (aimAngle + 1 >= 180)
        {
            //this helps catch situations where the lever almost makes it to 180 but not quite
            aimAngle = 180;
        }
        else if (aimAngle - 1 <= -180)
        {
            aimAngle = -180;
        }
        AimEcho.text = "Aim Axis:\n" + (int)aimAngle + (char)176;
        catapultControll.SetAimAxisAngle(aimAngle);
    }

    public void CalculateShotPower(float shotPower)
    {
        shotPower += MAX_ANGLE;
        shotPower *= catapultControll.MaxPulledBackCatapultArm / (MAX_ANGLE * 2);
        shotPower = Mathf.Clamp(shotPower, catapultControll.MaxPulledBackCatapultArm, 0);
        ShotEcho.text = "Shot Angle:\n" + (int)-shotPower + (char)176;
        catapultControll.SetShotPowerAngle(shotPower);
    }

    public void CalculateCatapultTransform(Vector3 endPointPos, Vector3 origin)
    {
        Vector3 dir = endPointPos - origin;
        Vector2 v = new Vector2(dir.x, dir.z);
        catapultControll.SetMovementVector(v);
    }
}
