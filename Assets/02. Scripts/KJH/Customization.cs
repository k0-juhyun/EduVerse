using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    public GameObject FrontHair;
    private SkinnedMeshRenderer curHairRenderer;

    [Header("¾Õ¸Ó¸®")]
    public Mesh[] frontHairMeshes;
    private int curFrontHairIdx;

    private void Awake()
    {
        curHairRenderer = FrontHair.GetComponent<SkinnedMeshRenderer>();
        curFrontHairIdx = 0;
    }
    
    private void Customize(ref int curIdx, Mesh[] allIdx,ref SkinnedMeshRenderer curRenderer)
    {
        if(curIdx == allIdx.Length -1)
        {
            curIdx = 0;
        }
        else
        {
            curIdx++;
        }

        if (curRenderer != null && curIdx >= 0 && curIdx < allIdx.Length)
        {
            curRenderer.sharedMesh = allIdx[curIdx];
        }
    }

    public void SwitchRight()
    {
        Customize(ref curFrontHairIdx, frontHairMeshes, ref curHairRenderer);
    }
}