using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStick : MonoBehaviour
{
    public GameObject Handle;
    public GameObject Pivot;
    public GameObject EndPoint;
    public enum ControlType { catapultTranslation, aimAxis, shotPower}
    public ControlType ManipulationOutput = ControlType.aimAxis;

    Vector3 lastHandlePos;
    Transform orign;
    bool isLever;
    CatapultControll catapultControll;
    const int MAX_ANGLE = 50;
    const float JOY_STICK_BOUNDRY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        catapultControll = FindObjectOfType<CatapultControll>();
        Debug.Assert(catapultControll != null);
        Debug.Assert(Handle != null);
        Debug.Assert(Pivot != null);
        Debug.Assert(EndPoint != null);
        orign = Handle.transform;
        lastHandlePos = orign.localPosition;
        isLever = !(ManipulationOutput == ControlType.catapultTranslation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Handle.transform.localPosition != lastHandlePos)
        {
            lastHandlePos = Handle.transform.localPosition;
            Vector3 dir = lastHandlePos - Pivot.transform.localPosition;
            if (isLever)
            {
                dir.z = 0;
                float theta = Mathf.Acos(Vector3.Dot(dir.normalized, Vector3.up)) * Mathf.Rad2Deg;
                if (Mathf.Abs(theta) < MAX_ANGLE)
                {
                    Pivot.transform.up = dir;
                    EndPoint.transform.localPosition = (dir.normalized * 1.5f + Pivot.transform.localPosition);
                    EndPoint.transform.up = dir;

                    if (EndPoint.transform.localPosition.x < 0)
                    {
                        theta *= -1;
                    }

                    theta = Mathf.Clamp(theta, -MAX_ANGLE, MAX_ANGLE);
                    float outputAngle = theta + 50;
                    if (ManipulationOutput == ControlType.aimAxis)
                    {
                        calculateAimAxis(outputAngle);
                    }
                    else
                    {
                        calculateShotPower(outputAngle);
                    }
                }
            }
            else
            {

            }
        }
        if (ManipulationOutput == ControlType.catapultTranslation)
        {
            //do stuff
        }
    }

    public void ResetHandle()
    {
        Handle.transform.localPosition = EndPoint.transform.localPosition;
        lastHandlePos = Handle.transform.localPosition;
    }

    Vector3 checkBoundry(Vector3 v)
    {
        return v;
    }

    void calculateAimAxis(float aimAngle)
    {
        catapultControll.SetAimAxisAngle(aimAngle);
    }

    void calculateShotPower(float shotPower)
    {
        catapultControll.SetShotPowerAngle(shotPower);
    }
}
