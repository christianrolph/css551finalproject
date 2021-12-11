using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;

    protected Vector3 origNodeOrigin;
    protected Transform origTransform;
    bool hasOrigInfoBeenStored = false;
    public TheWorld World;


    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public AxisFrameControl AxisFrame = null;       // the one axis frame control for this scene node
    public CatapultControll TheCatapultControl = null;
    public ProjectileBehavior Projectile = null;    // the projectile


    public bool IsGem = false;

	// Use this for initialization
	protected void Start () {
        InitializeSceneNode();

        if (!IsGem)
        {
            this.World = GameObject.FindObjectOfType<TheWorld>();
            this.TheCatapultControl = GameObject.FindObjectOfType<CatapultControll>();
            Debug.Assert(this.TheCatapultControl != null);
            Debug.Assert(this.World != null);
        }

	}
	
	// Update is called once per frame
	void Update () {
	}

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        // determine combined transform
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        this.mCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }
        
        // disseminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            //Debug.Log("CombindedXform: x" + mCombinedParentXform[12] + " y" + mCombinedParentXform[13] + " z" + mCombinedParentXform[14]);
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }

        // disseminate to axis frame
        if (!IsGem && this.AxisFrame != null)
        {
            this.AxisFrame.setAxisFrame(ref mCombinedParentXform);
        }

        // disseminate to the Scene Hierarchy Projectile
        if (!IsGem && this.Projectile != null)
        {
            this.Projectile.setProjectileLocation(ref mCombinedParentXform);
        }

        // determine creating a new projectile
        // only ArmNode does this, and it performs it one time
        if (string.Equals(this.gameObject.name, "ArmNode", System.StringComparison.OrdinalIgnoreCase) &&
            this.TheCatapultControl.CreateNewProjectile == true)
        {
            // launch the projectile
            ProjectileBehavior launchingProjectile = ProjectileBehavior.InstantiateProjectile(ref mCombinedParentXform, TheCatapultControl.LastFiredPositionAngle, TheCatapultControl.SmallCamera);
            
            // creation is complete
            this.TheCatapultControl.CreateNewProjectile = false;

            // set bool to pull the arm back
            this.TheCatapultControl.isPullingBackArm = true;
        }

        // set the camera if available
        if(string.Equals(this.gameObject.name, "RotationNode", System.StringComparison.OrdinalIgnoreCase) &&
           this.TheCatapultControl.SmallCamera != null)
        {
            this.TheCatapultControl.SmallCamera.SetCameraToSceneNode(ref mCombinedParentXform);
        }
    }
}