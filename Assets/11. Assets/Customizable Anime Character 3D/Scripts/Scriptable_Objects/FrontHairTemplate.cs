using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Fornt Hair", menuName = "Clothes/Hair/Front Hair")]
public class FrontHairTemplate : ScriptableObject
{
    public Texture2D icon;
    public GameObject Hair;
    public variation[] ColorVar;

}
