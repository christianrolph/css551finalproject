using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollider : MonoBehaviour
{
    public SceneNode GemNode;
    public GameObject GemGlow;

    Vector3 lastPos;
    bool isGrabbed = false;
    GemHandler gemHandler;

    // Start is called before the first frame update
    void Start()
    {
        gemHandler = FindObjectOfType<GemHandler>();
        lastPos = transform.localPosition;
        Debug.Assert(GemGlow != null);
        GemGlow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos != transform.localPosition)
        {
            lastPos = transform.localPosition;
            GemNode.transform.position = lastPos;
        }
        if (!isGrabbed)
        {
            //Quaternion rotation = rotateGem();
            //transform.localRotation *= rotation;
        }
        Matrix4x4 matrix = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        GemNode.CompositeXform(ref matrix);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            gemHit();
        }
    }

    public void GemGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
    }

    public void Glow(bool turnOn)
    {
        GemGlow.SetActive(turnOn);
    }

    Quaternion rotateGem()
    {
        Quaternion rotation = Quaternion.AngleAxis(-20 * Time.deltaTime, Vector3.up);
        return rotation;
    }

    void gemHit()
    {
        gemHandler.GemHit(this);
        GameObject.Destroy(gameObject);
    }
}