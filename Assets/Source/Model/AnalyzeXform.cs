using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeXform : MonoBehaviour
{
    public Transform AxisFrameTransform = null;

    void Awake()
    {
        FindAndSetAxisFrameTransform();
    }

    // Use this for initialization
    void Start()
    {
        Debug.Assert(AxisFrameTransform != null);
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 myTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 concatMatrix = myTRS;

        // now decompose and get each components
        AxisFrameTransform.localPosition = concatMatrix.GetColumn(3);
        Vector3 x = concatMatrix.GetColumn(0);
        Vector3 y = concatMatrix.GetColumn(1);
        Vector3 z = concatMatrix.GetColumn(2);
        Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        AxisFrameTransform.localScale = size;

        // Align rotation
        // WorldTransform.localRotation = Quaternion.LookRotation(z / size.z, y / size.y);
        // OR
        y.Normalize();
        z.Normalize();
        // First, align up
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, y)) * Mathf.Rad2Deg;
        Vector3 axis = Vector3.Cross(Vector3.up, y);
        AxisFrameTransform.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align forward
        angle = Mathf.Acos(Vector3.Dot(AxisFrameTransform.forward, z)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(AxisFrameTransform.forward, z);
        AxisFrameTransform.localRotation = Quaternion.AngleAxis(angle, axis) * AxisFrameTransform.localRotation;
    }

    public void FindAndSetAxisFrameTransform()
    {
        this.AxisFrameTransform = GameObject.Find("AxisFrame").transform;
        Debug.Assert(AxisFrameTransform != null);
    }
}
