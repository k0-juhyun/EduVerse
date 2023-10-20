using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviour
{
    // 의자에 앉아있는지 여부
    public bool isFull;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("dd");
        }
    }
}
