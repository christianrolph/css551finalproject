using System; // for assert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for GUI elements: Button, Toggle

public partial class MainController : MonoBehaviour {

    // reference to all UI elements in the Canvas
    public Camera MainCamera = null;
    public TheWorld TheWorld = null;
    public Vector3 lastMousePosition = Vector3.zero;
    public Vector3 currentMousePosition = Vector3.zero;

    // Use this for initialization
    void Start() {
        Debug.Assert(MainCamera != null);
        Debug.Assert(TheWorld != null);
    }

    // Update is called once per frame
    void Update() {
    }
}