using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [ExecuteInEditMode]
public class TheWorld : MonoBehaviour  {

    public SceneNode TheRoot;

    private void Start()
    {
    }

    private void Update()
    {
        //Matrix4x4 i = Matrix4x4.identity;
        Matrix4x4 i = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        TheRoot.CompositeXform(ref i);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}
