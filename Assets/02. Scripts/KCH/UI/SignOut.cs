using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SignOut : MonoBehaviour
{

    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => SignOutFun());
    }

    public void SignOutFun()
    {
        GameManager.Instance.SingOut();
    }
}
