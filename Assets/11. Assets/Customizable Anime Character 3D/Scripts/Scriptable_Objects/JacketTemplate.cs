using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jacket", menuName = "Clothes/Jacket")]
public class JacketTemplate : ClothTemplate
{
   
    public Mesh Jacket;
    public variation[] ColorVar;
}
