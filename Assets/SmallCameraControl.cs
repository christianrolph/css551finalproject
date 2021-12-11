using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCameraControl : MonoBehaviour
{
    public Camera TheCamera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.TheCamera != null);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraPostionToProjectile(Transform projectileTransform)
    {
        // offset the camera to the new position up and behind
        Vector3 offSetPosition = projectileTransform.localPosition;
        this.transform.localPosition = new Vector3(offSetPosition.x, offSetPosition.y + .082573f, offSetPosition.z - .328362f);

        // adjust aim
        this.TheCamera.transform.LookAt(projectileTransform);

    }

    public void SetCameraToPosition(Vector3 position)
    {
        this.transform.localPosition = position;
    }

    public void SetCameraToSceneNode(ref Matrix4x4 mCombinedParentXform)
    {
        // decompose and get each components
        this.transform.localPosition = mCombinedParentXform.GetColumn(3);
        Vector3 x = mCombinedParentXform.GetColumn(0);
        Vector3 y = mCombinedParentXform.GetColumn(1);
        Vector3 z = mCombinedParentXform.GetColumn(2);

        // Code for size
        Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        // this.transform.localScale = size;

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

        // set the camera's position
        Vector3 currentPosition = this.transform.localPosition;
        this.transform.localPosition = currentPosition + (-.45f * z.magnitude * transform.forward.normalized);
        this.transform.localPosition = this.transform.localPosition + (.15f * y.magnitude * transform.up.normalized);

        // look just above the scene node
        // this.TheCamera.transform.LookAt(transform.forward.normalized + transform.up.normalized);
        this.TheCamera.transform.LookAt(currentPosition + (.15f * y.magnitude * transform.up.normalized));

        // look up at a 15 degree angle
        //this.transform.localRotation = Quaternion.AngleAxis(-15, transform.right.normalized);
    }
}
