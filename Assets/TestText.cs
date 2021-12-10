using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour
{
    public Text TheText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.TheText != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SayHello()
    {
        Debug.Log("Hello was said");
        if (string.Equals(this.TheText.text, "Hello", System.StringComparison.OrdinalIgnoreCase))
        {
            this.TheText.text = "World!";
        }
        else
        {
            this.TheText.text = "Hello";
        }
    }
}
