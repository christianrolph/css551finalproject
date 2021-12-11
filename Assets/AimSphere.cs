using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAimSpherePosition(ref Matrix4x4 mCombinedParentXform)
    {
        // decompose and get each components
        this.transform.localPosition = mCombinedParentXform.GetColumn(3);
        Vector3 x = mCombinedParentXform.GetColumn(0);
        Vector3 y = mCombinedParentXform.GetColumn(1);
        Vector3 z = mCombinedParentXform.GetColumn(2);

        // Code for size
        Vector3 size = new Vector3(x.magnitude / 35f, y.magnitude / 35f, z.magnitude / 35f);
        this.transform.localScale = size;

        // Align rotation
        // WorldTransform.localRotation = Quaternion.LookRotation(z / size.z, y / size.y);
        // OR
        y.Normalize();
        z.Normalize();
        // First, align up
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, y)) * Mathf.Rad2Deg;
        Vector3 axis = Vector3.Cross(Vector3.up, y);
        this.transform.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align forward
        angle = Mathf.Acos(Vector3.Dot(this.transform.forward, z)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(this.transform.forward, z);
        this.transform.localRotation = Quaternion.AngleAxis(angle, axis) * this.transform.localRotation;

        // set the position to be directly above the scene node
        Vector3 currentPosition = this.transform.localPosition;
        this.transform.localPosition = currentPosition + (.018f * z.magnitude * transform.forward.normalized);
        this.transform.localPosition = this.transform.localPosition + (.210f * y.magnitude * transform.up.normalized);
    }
}
