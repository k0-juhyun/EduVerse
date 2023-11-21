using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassRoomHandler : MonoBehaviour
{
    public GameObject StudentDesk;
    public GameObject Floor;
    public GameObject FloorWithoutShadow;

    private void Start()
    {
        FloorWithoutShadow.SetActive(false);
    }
}
