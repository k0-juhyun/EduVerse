using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public CharacterMovement cMovement;
    public CharacterInteraction cInteraction;
    public CameraSetting cameraSetting;

    private void Awake()
    {
        cMovement = GetComponentInChildren<CharacterMovement>();
        cInteraction = GetComponentInChildren<CharacterInteraction>();
        cameraSetting = GetComponentInChildren<CameraSetting>();
    }
}
