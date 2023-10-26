using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherInteraction : MonoBehaviour
{
    public Button Btn;

    public bool enableButton;

    private CharacterInteraction characterInteraction;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.transform.parent.name == "Character(Clone)")
        {
            enableButton = true;
            Btn.gameObject.SetActive(enableButton);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.transform.parent.name == "Character(Clone)")
        {
            enableButton = false;
            Btn.gameObject.SetActive(enableButton);
        }
    }
}
