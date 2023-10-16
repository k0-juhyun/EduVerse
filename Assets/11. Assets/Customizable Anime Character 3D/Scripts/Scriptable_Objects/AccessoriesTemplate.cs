using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Accesories", menuName = "Clothes/Accessories")]
public class AccessoriesTemplate : ScriptableObject
{
    // Start is called before the first frame update
    public Texture2D icon;
    public GameObject Accessory;
    public variation[] ColorVar;
}

