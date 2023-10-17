using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Back Hair", menuName = "Clothes/Hair/Back Hair")]
    public class BackHairTemplate : ScriptableObject
    {
        public Texture2D icon;
        public GameObject Hair;
        public variation[] ColorVar;

    }

