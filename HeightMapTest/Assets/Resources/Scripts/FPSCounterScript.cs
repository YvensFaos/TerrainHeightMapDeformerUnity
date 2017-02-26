using UnityEngine;
using UnityEngine.UI;

public class FPSCounterScript : MonoBehaviour
{
    public Text FPSText;

    private void Start()
    {
        if (FPSText == null)
        {
            Debug.LogError("No text attached to this script. Disable it.");
            enabled = false;
        }
    }

    private void Update()
    {
        FPSText.text = Mathf.Ceil(1.0f / Time.deltaTime).ToString();
    }
}