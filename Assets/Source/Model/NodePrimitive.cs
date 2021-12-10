using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive: MonoBehaviour {
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public bool Rotating = false;
    public float DegreeDirection = 1f;
    public float DegreeChange = 0.05f;
    public CatapultControll TheCatapultControl;

    private Transform OrigTransform;
    private Vector3 OrigPivot;
    private bool hasOrigInfoBeenStored = false;

    public bool IsGem = false;

	// Use this for initialization
	void Start () {
        DegreeChange = 0.4f;

        if (!IsGem)
        {
            this.TheCatapultControl = GameObject.Find("TheWorld")?.GetComponent<CatapultControll>();
            Debug.Assert(this.TheCatapultControl != null);
        }

        // adjust rotation direction for hanging capsule
        //if (Rotating && string.Equals(this.gameObject.name, "Capsule", System.StringComparison.OrdinalIgnoreCase))
        //{
        //    this.gameObject.transform.forward = this.gameObject.transform.up;
        //}
    }

    void Update()
    {
    }
	
  
	public void LoadShaderMatrix(ref Matrix4x4 parentNodeMatrix)
    {
        Matrix4x4 pivot = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invPivot = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs;

        if (!Rotating)
        {
            trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            //Debug.Log("Mesh Local Pos: " + transform.localPosition);
        }
        else
        {
            if (Rotating && string.Equals(this.gameObject.name, "Capsule", System.StringComparison.OrdinalIgnoreCase))
            {
                // the capsule begins rotated 90 degrees in order to stand on its head
                if (transform.localEulerAngles.z > 180)
                {
                    DegreeDirection *= -1f;  // change direction of rotation
                }
            }
            if (transform.localEulerAngles.z > 90 && transform.localEulerAngles.z < 270)
            {
                DegreeDirection *= -1f;  // change direction of rotation
            }

            Quaternion q = Quaternion.AngleAxis(DegreeDirection * DegreeChange, Vector3.forward);

            transform.localRotation = q * transform.localRotation;
            trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        }
        Matrix4x4 m = parentNodeMatrix * pivot * trs * invPivot;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);

        if (!IsGem)
        {
            GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
        }
    }
}