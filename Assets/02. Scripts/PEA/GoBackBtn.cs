using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }
}
