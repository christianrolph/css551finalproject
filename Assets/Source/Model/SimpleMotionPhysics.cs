using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMotionPhysics : MonoBehaviour
{
    public Vector3 GravitationPull = -Vector3.up;  // default is dropping downwards
    public Vector3 Acceleration = Vector3.zero;  // default is dropping downwards
    public Vector3 Velocity = Vector3.zero;
    float TotalTime;    // tracks total lifetime of this object
    float AliveTime;    // how long this object lives

    // Start is called before the first frame update
    void Start()
    {
        this.TotalTime = 0f;
        this.AliveTime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        this.TotalTime += Time.smoothDeltaTime;

        if (this.TotalTime >= this.AliveTime)
        {
            Destroy(gameObject);
        }

        Acceleration += GravitationPull * Time.smoothDeltaTime;        // change in acceleration
        Velocity += Acceleration * Time.smoothDeltaTime;               // change in Velocity
        transform.localPosition += Velocity * Time.smoothDeltaTime;    // change in position
    }
}
