using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public CatapultControll TheCatapultControl = null;
    public SmallCameraControl SmallCamera = null;

    private void Awake()
    {
        this.TheCatapultControl = GameObject.FindObjectOfType<CatapultControll>();
        Debug.Assert(this.TheCatapultControl != null);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // works similar to a constructor
    public static ProjectileBehavior InstantiateProjectile(ref Matrix4x4 mCombinedParentXform, float fireAngle, SmallCameraControl smallCamera, float projectileAliveTime)
    {
        GameObject projectile = Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;
        projectile.GetComponent<Renderer>().material.color = Color.green;

        // decompose and get each components
        projectile.transform.localPosition = mCombinedParentXform.GetColumn(3);
        Vector3 x = mCombinedParentXform.GetColumn(0);
        Vector3 y = mCombinedParentXform.GetColumn(1);
        Vector3 z = mCombinedParentXform.GetColumn(2);

        // Code for size
        //Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        //this.transform.localScale = size;

        // Align rotation
        // WorldTransform.localRotation = Quaternion.LookRotation(z / size.z, y / size.y);
        // OR
        y.Normalize();
        z.Normalize();
        // First, align up
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, y)) * Mathf.Rad2Deg;
        Vector3 axis = Vector3.Cross(Vector3.up, y);
        projectile.transform.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align forward
        angle = Mathf.Acos(Vector3.Dot(projectile.transform.forward, z)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(projectile.transform.forward, z);
        projectile.transform.localRotation = Quaternion.AngleAxis(angle, axis) * projectile.transform.localRotation;

        // set in the launch pod
        Vector3 currentPosition = projectile.transform.localPosition;
        projectile.transform.localPosition = currentPosition + (.0199f * projectile.transform.forward.normalized);
        projectile.transform.localPosition = projectile.transform.localPosition + (.1084f * projectile.transform.up.normalized);

        ProjectileBehavior projBehav = projectile.GetComponent<ProjectileBehavior>();

        // attach camera if it's available
        if (smallCamera != null)
        {
            projBehav.SmallCamera = smallCamera;
        }

        // launch
        projBehav.InstantiateLaunchPhysics(fireAngle, projectileAliveTime);

        return projBehav;
    }

    public void setProjectileLocation(ref Matrix4x4 mCombinedParentXform)
    {
        // decompose and get each components
        this.transform.localPosition = mCombinedParentXform.GetColumn(3);
        Vector3 x = mCombinedParentXform.GetColumn(0);
        Vector3 y = mCombinedParentXform.GetColumn(1);
        Vector3 z = mCombinedParentXform.GetColumn(2);

        // Code for size
        Vector3 size = new Vector3(x.magnitude / 20, y.magnitude / 20, z.magnitude / 20);
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

        // set in the launch pod
        Vector3 currentPosition = this.transform.localPosition;
        this.transform.localPosition = currentPosition + (.0199f * transform.forward.normalized);
        this.transform.localPosition = this.transform.localPosition + (.1084f * transform.up.normalized);
    }

    public void InstantiateLaunchPhysics(float fireAngle, float projectileAliveTime)
    {
        float size = this.TheCatapultControl.transform.localScale.y / 9f;  // use this as the "strenth" of the launcher

        // attach physics to this game object
        SimpleMotionPhysics s = this.gameObject.AddComponent<SimpleMotionPhysics>();
        s.AliveTime = projectileAliveTime;
        s.transform.localPosition = this.transform.localPosition;
        s.Velocity = size * Math.Abs(fireAngle) * (transform.up + transform.forward).normalized; // should be a 45 degree angle
        // s.Velocity = size * (transform.up + transform.forward).normalized; // should be a 45 degree angle
        s.Acceleration = Vector3.zero;  // Initial acceleration follow the current up
        s.GravitationPull = this.TheCatapultControl.GravitationPull * Vector3.up;

        Debug.Log($"Launched at FireAngle: {fireAngle}");
    }

    public void DestroyProjectile()
    {
        // return the small camera back to catapult
        if (this.SmallCamera != null)
        {
            this.TheCatapultControl.SetSmallCamera(this.SmallCamera);
        }

        Destroy(gameObject);
    }
}
