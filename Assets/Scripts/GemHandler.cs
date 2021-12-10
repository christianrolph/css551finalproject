using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemHandler : MonoBehaviour
{
    public GameObject GemPrefab;
    public int GemsDestroyedCount = 0;

    List<GemCollider> gems = new List<GemCollider>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(GemPrefab != null);   
    }

    public void CreateGem()
    {
        GameObject gem = Instantiate(GemPrefab, new Vector3(0, 0.2f, -0.2f), Quaternion.identity);
        gems.Add(gem.GetComponent<GemCollider>());
    }

    public void GemHit(GemCollider gem)
    {
        gems.Remove(gem);
        GemsDestroyedCount++;
    }
}
