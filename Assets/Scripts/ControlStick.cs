using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ControlStick : MonoBehaviour
{
    public RemoteController controller;
    public GameObject Handle;
    public GameObject Pivot;
    public GameObject EndPoint;
    public enum ControlType { catapultTranslation, aimAxis, shotPower}
    public ControlType ManipulationOutput = ControlType.aimAxis;
    public GameObject EndPointGlow;

    Vector3 lastHandlePos;
    Vector3 orign;
    bool isLever;
    const int MAX_ANGLE = 50;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(controller != null);
        Debug.Assert(Handle != null);
        Debug.Assert(Pivot != null);
        Debug.Assert(EndPoint != null);
        Debug.Assert(EndPointGlow != null);
        ToggleGlow(false);
        orign = Handle.transform.localPosition; //need to update this if controller moved?
        lastHandlePos = orign;
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
            }
            float theta = Mathf.Acos(Vector3.Dot(dir.normalized, Vector3.up)) * Mathf.Rad2Deg;
            if (Mathf.Abs(theta) <= MAX_ANGLE)
            {
                EndPoint.transform.localPosition = (dir.normalized * 1.5f + Pivot.transform.localPosition);
                Pivot.transform.up = EndPoint.transform.position - Pivot.transform.position;
                EndPoint.transform.up = Pivot.transform.up;
                if (EndPoint.transform.localPosition.x < 0)
                {
                    theta *= -1;
                }
                if (ManipulationOutput == ControlType.aimAxis)
                {
                    controller.CalculateAimAxis(theta);
                }
                else if (ManipulationOutput == ControlType.shotPower)
                {
                    controller.CalculateShotPower(-theta);
                }
                else
                {
                    controller.CalculateCatapultTransform(EndPoint.transform.localPosition, orign);
                }
            }
        }
    }

    public void ResetHandle()
    {
        Handle.transform.localPosition = EndPoint.transform.localPosition;
        lastHandlePos = Handle.transform.localPosition;
    }

    public void SnapBack()
    {
        EndPoint.transform.localPosition = orign;
        Pivot.transform.up = EndPoint.transform.position - Pivot.transform.position;
        EndPoint.transform.up = Pivot.transform.up;
        Handle.transform.localPosition = EndPoint.transform.localPosition;
    }

    public void ToggleGlow(bool turnOn)
    {
        EndPointGlow.SetActive(turnOn);
    }
}
