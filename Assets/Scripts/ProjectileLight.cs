using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLight : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("LightPosition", transform.localPosition);
    }
}
