using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisDisplayButtonScript : MonoBehaviour
{
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(this.buttonText != null);
    }
    
    public void ToggleButtonText()
    {
        if (string.Equals(
            this.buttonText.text,
            "AxisDisplay: Off",
            System.StringComparison.OrdinalIgnoreCase))
        {
            this.buttonText.text = "AxisDisplay: On";
        }
        else
        {
            this.buttonText.text = "AxisDisplay: Off";
        }
    }
}
