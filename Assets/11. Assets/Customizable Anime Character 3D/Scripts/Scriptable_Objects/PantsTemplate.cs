using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pants", menuName = "Clothes/Pants")]
public class PantsTemplate : ClothTemplate
{
   
    public Mesh Pants;

    public variation[] ColorVar;
}
