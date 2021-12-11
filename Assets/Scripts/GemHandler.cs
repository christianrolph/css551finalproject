using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemHandler : MonoBehaviour
{
    public GameObject GemPrefab;
    public int GemsDestroyedCount = 0;
    public TMP_Text ScoreEcho;
    AudioSource audioSource;

    List<GemCollider> gems = new List<GemCollider>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(GemPrefab != null);
        Debug.Assert(ScoreEcho != null);
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(GetComponent<AudioSource>() != null);
    }

    public void CreateGem()
    {
        GameObject gem = Instantiate(GemPrefab, new Vector3(0, 0.2f, -0.2f), Quaternion.identity);
        gems.Add(gem.GetComponent<GemCollider>());
    }
    public void CreateGem(Vector3 pos)
    {
        GameObject gem = Instantiate(GemPrefab, pos, Quaternion.identity);
        gems.Add(gem.GetComponent<GemCollider>());
    }

    public void GemHit(GemCollider gem, ProjectileBehavior projectile)
    {
        audioSource.Play();
        gems.Remove(gem);
        GemsDestroyedCount++;
        projectile.DestroyProjectile();
        ScoreEcho.text = "Score:\n" + GemsDestroyedCount;
    }
}
