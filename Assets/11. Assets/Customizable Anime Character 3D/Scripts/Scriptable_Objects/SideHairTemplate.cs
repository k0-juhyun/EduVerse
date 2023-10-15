using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Side Hair", menuName = "Clothes/Hair/Side Hair")]
public class SideHairTemplate : ScriptableObject
{
    public Texture2D icon;
    public GameObject Hair;
    public variation[] ColorVar;

}
