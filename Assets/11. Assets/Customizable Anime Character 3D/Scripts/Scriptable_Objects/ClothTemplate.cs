using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ClothTemplate : ScriptableObject
    {
        public Texture2D icon;
    }
    [System.Serializable]
    public class variation
    {
        public Mesh items;
        public string name;
    }
