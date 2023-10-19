using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviour
{
    // 의자에 앉아있는지 여부
    public bool isSit;

    // 콜라이더에 들어왔는지 확인여부
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("dd");
        }
    }
}
