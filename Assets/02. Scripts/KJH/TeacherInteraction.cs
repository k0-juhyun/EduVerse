using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherInteraction : MonoBehaviour
{
    public Button Btn;

    public bool enableButton;

    private void OnTriggerStay(Collider other)
    {
        Btn.gameObject.SetActive(enableButton);
    }
}
