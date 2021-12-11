using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [ExecuteInEditMode]
public class TheWorld : MonoBehaviour  {

    public SceneNode TheRoot;
    public GameObject Glow;

    private void Start()
    {
        Glow.SetActive(false);
        GemHandler gemHandler = GetComponent<GemHandler>();
        gemHandler.CreateGem(new Vector3(-0.5f, 0.3f, 1.0f));
        gemHandler.CreateGem(new Vector3(0.0f, 0.3f, 1.0f));
        gemHandler.CreateGem(new Vector3(0.5f, 0.3f, 1.0f));
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

    public void ToggleGlow(bool isOn)
    {
        Glow.SetActive(isOn);
    }
}
