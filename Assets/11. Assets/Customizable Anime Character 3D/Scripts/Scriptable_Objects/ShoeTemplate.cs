using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shoes", menuName = "Clothes/Shoes")]
public class ShoeTemplate : ScriptableObject
{
    public Texture2D icon;
    public Mesh L_Shoe;
    public Mesh R_Shoe;
    public ShoeVariation[] ColorVar;
}
[System.Serializable]
public class ShoeVariation
{

    public string name;
    public Mesh L_Shoe;
    public Mesh R_Shoe;
}
