using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureResultText : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(SetActiveFalse), 0.5f);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
