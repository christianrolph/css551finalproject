using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // works similar to a constructor
    public static ProjectileBehavior InstantiateProjectile(ref Matrix4x4 mCombinedParentXform)
    {
        GameObject projectile = Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;
        projectile.GetComponent<Renderer>().material.color = Color.blue;
        projectile.transform.position = mCombinedParentXform.GetColumn(3);

        return projectile.GetComponent<ProjectileBehavior>();
    }

    public void setProjectileLocation(ref Matrix4x4 mCombinedParentXform)
    {
        // decompose and get each components
        this.transform.localPosition = mCombinedParentXform.GetColumn(3);
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
        this.transform.localRotation = Quaternion.AngleAxis(angle, axis);
        // Now, align forward
        angle = Mathf.Acos(Vector3.Dot(this.transform.forward, z)) * Mathf.Rad2Deg;
        axis = Vector3.Cross(this.transform.forward, z);
        this.transform.localRotation = Quaternion.AngleAxis(angle, axis) * this.transform.localRotation;

        // set in the launch pod
        Vector3 currentPosition = this.transform.localPosition;
        this.transform.localPosition = currentPosition + (.0214f * transform.forward.normalized);
        this.transform.localPosition = this.transform.localPosition + (.1084f * transform.up.normalized);
    }
}
