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
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
    }

    // resets the entire scene
    public void ResetHierarchy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // resets the entire scene
    public void LoadExtraScene()
    {
        SceneManager.LoadScene("ExtraScene");
    }

    // resets the entire scene
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    // resets the entire scene
    public void LoadPeaceScene()
    {
        SceneManager.LoadScene("PeaceScene");
    }

    // resets the entire scene
    public void LoadRockScene()
    {
        SceneManager.LoadScene("RockScene");
    }

    // resets the entire scene
    public void LoadThumbsUpScene()
    {
        SceneManager.LoadScene("ThumbsUpScene");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
