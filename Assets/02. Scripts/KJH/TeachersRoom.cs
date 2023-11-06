using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachersRoom : MonoBehaviour
{
    public Button Btn;

    public bool enableButton;

    private CharacterInteraction characterInteraction;

    public GameObject DigitalEditCanvas;

    // ������ ������ ���� ��.
    public void DigitalEdit()
    {
        if (!DigitalEditCanvas.activeSelf)
            DigitalEditCanvas.SetActive(true);
        else
        {
            DigitalEditCanvas.SetActive(false);

            // ����ī�޶� �� ���� 
            Camera.main.transform.localPosition = new Vector3(0, 6.5f, -8);
            Camera.main.transform.localRotation = Quaternion.Euler(20, 0, 0);
            Camera.main.GetComponentInParent<CameraSetting>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.transform.parent.name == "Character(Clone)")
        {
            enableButton = true;
            Btn.gameObject.SetActive(enableButton);
            Btn.gameObject.transform.forward = Camera.main.transform.forward;
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
