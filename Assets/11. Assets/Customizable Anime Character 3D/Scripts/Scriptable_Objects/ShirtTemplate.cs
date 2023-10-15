using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Shirt",menuName ="Clothes/Shirt")]
public class ShirtTemplate : ClothTemplate
{
    
    public Mesh shirt;
    public Mesh shirt_L_Sleeve;
    public Mesh shirt_R_Sleeve;

    public ShirtVariation[] ColorVar;
}
[System.Serializable]
public class ShirtVariation
{

    public string name;
    public Mesh shirt;
    public Mesh shirt_L_Sleeve;
    public Mesh shirt_R_Sleeve;
}
