using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCustomizationSystem
{
    public class Accessory : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject armature;
        public SkinnedMeshRenderer skin;
    }

    [System.Serializable]
    public class AccessoryPrefab
    {
        public enum BoneList
        {
            Head, Neck, Chest, Stomach, ShoulderL, ShoulderR, UpperArmL, UpperArmR, LowerArmL, LowerArmR, HandL, HandR, ThumbBaseL, ThumbTipL, ThumbBaseR, ThumbTipR,
            IndexFingerBaseL, IndexFingerTipL, IndexFingerBaseR, IndexFingerTipR,
            MiddleFingerBaseL, MiddleFingerTipL, MiddleFingerBaseR, MiddleFingerTipR,
            RingFingerBaseL, RingFingerTipL, RingFingerBaseR, RingFingerTipR,
            PinkyFingerBaseL, PinkyFingerTipL, PinkyFingerBaseR, PinkyFingerTipR,
            HipL, HipR, LegL, LegR, KneeL, KneeR, FootL, FootR, ToeL, ToeR
        };
        public BoneList BoneParent = BoneList.Head;
        // Start is called before the first frame update
        public AccessoriesTemplate Accessory;
        [HideInInspector] public int AltIndex = 0;
        [HideInInspector] public GameObject AccessoryObject;
        [HideInInspector] public GameObject Parent;
    }
}
