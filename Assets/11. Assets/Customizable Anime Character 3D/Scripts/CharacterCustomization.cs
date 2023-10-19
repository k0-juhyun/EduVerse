
using UnityEngine;

using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CharacterCustomizationSystem{

[System.Serializable]
public class CharacterCustomization : MonoBehaviour
{
        // Start is called before the first frame update
        Material body ;
        Material Face ;
        public Material URP_Body;
        public Material URP_Face;

        public Material BuiltIn_Body;
        public Material BuiltIn_Face;

        public Material HFRP_Body;
        public Material HDRP_Face;

        public string[] Materials = new string[] {"URP","Built-In","HDRP" };
        public int MaterialIndex = 0;

        public GameObject[] Hide;

    public GameObject[] Accessories;

    //Ref
    #region Bodypart reference

    public GameObject shirtRef;
    public GameObject L_sleeve_shirtRef;
    public GameObject R_sleeve_shirtRef;

    public GameObject pantsRef;

    public GameObject jacketRef;

    public GameObject jacket2Ref;
    public GameObject jacket3Ref;

    public GameObject L_shoeRef;
    public GameObject R_shoeRef;

    public GameObject headRef;
    public GameObject neckRef;
    public GameObject chestRef;
    public GameObject stomachRef;
    public GameObject lowerbodyRef;
    public GameObject L_legRef;
    public GameObject R_legRef;
    public GameObject L_footRef;
    public GameObject R_footRef;
    public GameObject L_shoulderRef;
    public GameObject R_shoulderRef;
    public GameObject L_armRef;
    public GameObject R_armRef;
    public GameObject L_handRef;
    public GameObject R_handRef;


    public GameObject L_eyeRef;
    public GameObject R_eyeRef;
    public GameObject L_eyebrowRef;
    public GameObject R_eyebrowRef;
    public GameObject L_eyebackRef;
    public GameObject R_eyebackRef;

    public GameObject mouthRef;

    public GameObject arm_Root_Ref;
    public GameObject armheadRef;
    public GameObject armneckRef;
    public GameObject armchestRef;
    public GameObject armstomachRef;

    public GameObject arm_L_HipRef;
    public GameObject arm_R_HipRef;
    public GameObject arm_L_UpperlegRef;
    public GameObject arm_R_UpperlegRef;
    public GameObject arm_L_LowerlegRef;
    public GameObject arm_R_LowerlegRef;
    public GameObject arm_L_footRef;
    public GameObject arm_R_footRef;
    public GameObject arm_L_toeRef;
    public GameObject arm_R_toeRef;


    public GameObject arm_L_ShoulderRef;
    public GameObject arm_R_ShoulderRef;
    public GameObject arm_L_UpperArmRef;
    public GameObject arm_R_UpperArmRef;
    public GameObject arm_L_LowerArmRef;
    public GameObject arm_R_LowerArmRef;
    public GameObject arm_L_HandRef;
    public GameObject arm_R_HandRef;

    public GameObject arm_L_BaseThumbRef;
    public GameObject arm_L_TipThumbRef;
    public GameObject arm_L_BaseIndexFingerRef;
    public GameObject arm_L_TipIndexFingerRef;
    public GameObject arm_L_BaseMiddleFingerRef;
    public GameObject arm_L_TipMiddleFingerRef;
    public GameObject arm_L_BaseRingFingerRef;
    public GameObject arm_L_TipRingFingerRef;
    public GameObject arm_L_BasePinkyFingerRef;
    public GameObject arm_L_TipPinkyFingerRef;

    public GameObject arm_R_BaseThumbRef;
    public GameObject arm_R_TipThumbRef;
    public GameObject arm_R_BaseIndexFingerRef;
    public GameObject arm_R_TipIndexFingerRef;
    public GameObject arm_R_BaseMiddleFingerRef;
    public GameObject arm_R_TipMiddleFingerRef;
    public GameObject arm_R_BaseRingFingerRef;
    public GameObject arm_R_TipRingFingerRef;
    public GameObject arm_R_BasePinkyFingerRef;
    public GameObject arm_R_TipPinkyFingerRef;
    #endregion


    public SkinTemplate Skin;
    public ShirtTemplate shirt;

    public JacketTemplate Jacket;
    public JacketTemplate Jacket_2;
    public JacketTemplate Jacket_3;

    public PantsTemplate Pants;

    public ShoeTemplate Shoe;
    public ShoeTemplate LShoe;
    public ShoeTemplate RShoe;

    public EyeTemplate EyeBack;
    public EyeTemplate LEyeBack;
    public EyeTemplate REyeBack;

    public EyeBallsTemplate EyeBall;
    public EyeBallsTemplate LEyeBall;
    public EyeBallsTemplate REyeBall;

    public EyeBrowTemplate EyeBrow;
    public EyeBrowTemplate LEyeBrow;
    public EyeBrowTemplate REyeBrow;

    public BackHairTemplate BackHair;
    public FrontHairTemplate FrontHair;
    public SideHairTemplate SideHair;

    public MouthTemplate Mouth;

    public bool AdvanceShoe;
    public bool AdvEyeBack;
    public bool AdvEyeBall;
    public bool AdvEyeBrow;

    public List<AccessoryPrefab> accessories = new List<AccessoryPrefab>();

    #region Color index
    public int BackHairColorIndex = 0;
    public int FrontHairColorIndex = 0;
    public int SideHairColorIndex = 0;
    public int ShirtColorIndex = 0;
    public int PantsColorIndex = 0;
    public int JacketColorIndex = 0;
    public int Jacket2ColorIndex = 0;
    public int Jacket3ColorIndex = 0;
    public int ShoeColorIndex = 0;
    public int LShoeColorIndex = 0;
    public int RShoeColorIndex = 0;
    #endregion

    public GameObject Backhairs;
    public GameObject Fronthairs;
    public GameObject Sidehairs;
    // GameObject[] hairs=new GameObject[1];
    //public GameObject HairArmatureHolder;
    private void OnValidate()
    {







    }
    public void ChangeMaterial()
        {
            body=URP_Body;
            Face=URP_Face;
            if (MaterialIndex == 0)
            {
                body = URP_Body;
                Face = URP_Face;
            }
            else if (MaterialIndex == 1)
            {
                body = BuiltIn_Body;
                Face = BuiltIn_Face;
            }
            else if (MaterialIndex == 2)
            {
                body = HFRP_Body;
                Face = HDRP_Face;
            }
            GameObject temp = (GameObject)Backhairs;
            if (temp != null)
            {
                temp.GetComponent<Accessory>().skin.material = body;
            }
            temp = (GameObject)Fronthairs;
            if (temp != null)
            {
                temp.GetComponent<Accessory>().skin.material = body;
            }
            temp = (GameObject)Sidehairs;
            if (temp != null)
            {
                temp.GetComponent<Accessory>().skin.material = body;
            }
            shirtRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().material = body;
            pantsRef.GetComponent<SkinnedMeshRenderer>().material = body;
            jacketRef.GetComponent<SkinnedMeshRenderer>().material = body;
            jacket2Ref.GetComponent<SkinnedMeshRenderer>().material = body;
            jacket3Ref.GetComponent<SkinnedMeshRenderer>().material = body;
            L_shoeRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_shoeRef.GetComponent<SkinnedMeshRenderer>().material = body;
            headRef.GetComponent<SkinnedMeshRenderer>().material = body;
            neckRef.GetComponent<SkinnedMeshRenderer>().material = body;
            chestRef.GetComponent<SkinnedMeshRenderer>().material = body;
            stomachRef.GetComponent<SkinnedMeshRenderer>().material = body;
            lowerbodyRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_legRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_legRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_footRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_footRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_shoulderRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_shoulderRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_armRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_armRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_handRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_handRef.GetComponent<SkinnedMeshRenderer>().material = body;
            L_eyebrowRef.GetComponent<SkinnedMeshRenderer>().material = body;
            R_eyebrowRef.GetComponent<SkinnedMeshRenderer>().material = body;
            for (int x = 0; x < accessories.Count; x++)
            {
                if (accessories[x].AccessoryObject != null)
                {
                    accessories[x].AccessoryObject.GetComponentInChildren<SkinnedMeshRenderer>().material = body;
                }
                
            }


            L_eyebackRef.GetComponent<SkinnedMeshRenderer>().material = Face;
            R_eyebackRef.GetComponent<SkinnedMeshRenderer>().material = Face;
            L_eyeRef.GetComponent<SkinnedMeshRenderer>().material = Face;
            R_eyeRef.GetComponent<SkinnedMeshRenderer>().material = Face;
            mouthRef.GetComponent<SkinnedMeshRenderer>().material = Face;
        }
    public void HideMesh()
    {
        for (int i = 0; i < Hide.Length; i++)
        {
            if (Hide[i] != null)
            {
                Hide[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }

        }
    }
   
        
    public void AddAccesories()
    {
        accessories.Add(new AccessoryPrefab());
    }
    public void RemoveAccessories(int x)
    {
        GameObject temp = (GameObject)accessories[x].AccessoryObject;
        accessories.RemoveAt(x);
        //hairs.RemoveAt(0);
        if (temp != null)
        {
            if (temp.GetComponent<Accessory>().armature.gameObject != null)
            {
                DestroyImmediate(temp.GetComponent<Accessory>().armature.gameObject);
            }

            DestroyImmediate(temp);
        }



    }
    public void ChangeParent(int x)
    {
        if (accessories[x].AccessoryObject == null)
        {
            Debug.LogError("Please Apply Accessories First");
        }
        else
        {
            #region Look For Parent
            if (accessories[x].BoneParent == AccessoryPrefab.BoneList.Head)
            {
                accessories[x].Parent = armheadRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.Neck)
            {

                accessories[x].Parent = armneckRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.Chest)
            {

                accessories[x].Parent = armchestRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.Stomach)
            {

                accessories[x].Parent = armstomachRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ShoulderL)
            {

                accessories[x].Parent = arm_L_ShoulderRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ShoulderR)
            {

                accessories[x].Parent = arm_R_ShoulderRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.UpperArmL)
            {

                accessories[x].Parent = arm_L_UpperArmRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.UpperArmR)
            {

                accessories[x].Parent = arm_R_UpperArmRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.LowerArmL)
            {

                accessories[x].Parent = arm_L_LowerArmRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.LowerArmR)
            {

                accessories[x].Parent = arm_R_LowerArmRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.HandL)
            {

                accessories[x].Parent = arm_L_HandRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.HandR)
            {

                accessories[x].Parent = arm_R_HandRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ThumbBaseL)
            {

                accessories[x].Parent = arm_L_BaseThumbRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ThumbTipL)
            {

                accessories[x].Parent = arm_L_TipThumbRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ThumbBaseR)
            {

                accessories[x].Parent = arm_R_BaseThumbRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ThumbTipR)
            {

                accessories[x].Parent = arm_R_TipThumbRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.IndexFingerBaseL)
            {

                accessories[x].Parent = arm_L_BaseIndexFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.IndexFingerTipL)
            {

                accessories[x].Parent = arm_L_TipIndexFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.IndexFingerBaseR)
            {

                accessories[x].Parent = arm_R_BaseIndexFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.IndexFingerTipR)
            {

                accessories[x].Parent = arm_R_TipIndexFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.MiddleFingerBaseL)
            {

                accessories[x].Parent = arm_L_BaseMiddleFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.MiddleFingerTipL)
            {

                accessories[x].Parent = arm_L_TipMiddleFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.MiddleFingerBaseR)
            {

                accessories[x].Parent = arm_R_BaseMiddleFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.MiddleFingerTipR)
            {

                accessories[x].Parent = arm_R_TipMiddleFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.RingFingerBaseL)
            {

                accessories[x].Parent = arm_L_BaseRingFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.RingFingerTipL)
            {

                accessories[x].Parent = arm_L_TipRingFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.RingFingerBaseR)
            {

                accessories[x].Parent = arm_R_BaseRingFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.RingFingerTipR)
            {

                accessories[x].Parent = arm_R_TipRingFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.PinkyFingerBaseL)
            {

                accessories[x].Parent = arm_L_BasePinkyFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.PinkyFingerTipL)
            {

                accessories[x].Parent = arm_L_TipPinkyFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.PinkyFingerBaseR)
            {

                accessories[x].Parent = arm_R_BasePinkyFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.PinkyFingerTipR)
            {

                accessories[x].Parent = arm_R_TipPinkyFingerRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.HipL)
            {

                accessories[x].Parent = arm_L_HipRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.HipR)
            {

                accessories[x].Parent = arm_R_HipRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.LegL)
            {

                accessories[x].Parent = arm_L_UpperlegRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.LegR)
            {

                accessories[x].Parent = arm_R_UpperlegRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.KneeL)
            {

                accessories[x].Parent = arm_L_LowerlegRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.KneeR)
            {

                accessories[x].Parent = arm_R_LowerlegRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.FootL)
            {

                accessories[x].Parent = arm_L_footRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.FootR)
            {

                accessories[x].Parent = arm_R_footRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ToeL)
            {

                accessories[x].Parent = arm_L_toeRef;
            }
            else if (accessories[x].BoneParent == AccessoryPrefab.BoneList.ToeR)
            {

                accessories[x].Parent = arm_R_toeRef;
            }
            #endregion

            accessories[x].AccessoryObject.GetComponent<Accessory>().armature.transform.SetParent(accessories[x].Parent.transform);
        }

    }
    public void ChangeColor(string ClothesID)
    {
        if (ClothesID == "HairBack")
        {
            GameObject temp = (GameObject)Backhairs;
            temp.GetComponent<Accessory>().skin.sharedMesh = BackHair.ColorVar[BackHairColorIndex].items;
        }
        else if (ClothesID == "HairFront")
        {
            GameObject temp = (GameObject)Fronthairs;
            temp.GetComponent<Accessory>().skin.sharedMesh = FrontHair.ColorVar[FrontHairColorIndex].items;
        }
        else if (ClothesID == "HairSide")
        {
            GameObject temp = (GameObject)Sidehairs;
            temp.GetComponent<Accessory>().skin.sharedMesh = SideHair.ColorVar[SideHairColorIndex].items;
        }
        else if (ClothesID == "Shirt")
        {
            shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.ColorVar[ShirtColorIndex].shirt;
            L_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.ColorVar[ShirtColorIndex].shirt_L_Sleeve;
            R_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.ColorVar[ShirtColorIndex].shirt_R_Sleeve;
        }
        else if (ClothesID == "Head")
        {

        }
        else if (ClothesID == "Pants")
        {
            pantsRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Pants.ColorVar[PantsColorIndex].items;
        }
        else if (ClothesID == "Jacket")
        {
            jacketRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket.ColorVar[JacketColorIndex].items;

        }
        else if (ClothesID == "Jacket2")
        {
            jacket2Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket_2.ColorVar[Jacket2ColorIndex].items;

        }
        else if (ClothesID == "Jacket3")
        {
            jacket3Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket_3.ColorVar[Jacket3ColorIndex].items;

        }
        else if (ClothesID == "Shoes")
        {
            if (!AdvanceShoe)
            {
                if (Shoe != null)
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Shoe.ColorVar[ShoeColorIndex].L_Shoe;
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Shoe.ColorVar[ShoeColorIndex].R_Shoe;

                }
                else
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }
            else
            {
                if (LShoe != null)
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = LShoe.ColorVar[LShoeColorIndex].L_Shoe;

                }
                else
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
                if (RShoe != null)
                {
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = RShoe.ColorVar[RShoeColorIndex].R_Shoe;

                }
                else
                {
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }



        }
        else if (ClothesID == "Body")
        {

        }
    }
    public void ChangeAccessoryColor(int x)
    {
        if (accessories[x].AccessoryObject == null)
        {
            ApplyAccessory(x);
        }
        accessories[x].AccessoryObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = accessories[x].Accessory.ColorVar[accessories[x].AltIndex].items;
    }
    public void ApplyAccessory(int x)
    {
            ChangeMaterial();
        if (accessories.Count != 0 && accessories[x].AccessoryObject != null)
        {
            GameObject temp = (GameObject)accessories[x].AccessoryObject;
            //hairs.RemoveAt(0);
            DestroyImmediate(temp.GetComponent<Accessory>().armature.gameObject);
            DestroyImmediate(temp);
        }
        if (accessories[x] != null)
        {
            GameObject SceneAcc;
            GameObject SceneAccArmature;

            SceneAcc = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)accessories[0].Accessory.Accessory);
            SceneAcc.transform.SetParent(this.gameObject.transform);
            SceneAcc.transform.localPosition = Vector3.zero;
            PrefabUtility.UnpackPrefabInstance(SceneAcc, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            SceneAccArmature = SceneAcc.GetComponent<Accessory>().armature;
            SceneAccArmature.transform.SetParent(armheadRef.transform);
            SceneAcc.GetComponent<Accessory>().skin.GetComponent<SkinnedMeshRenderer>().material=body;
            accessories[x].AccessoryObject = SceneAcc;
            accessories[x].AltIndex = 0;
            if (accessories[x].Accessory.ColorVar.Length > 0)
            {
                ChangeAccessoryColor(x);
            }
        }
    }
    public void ApplyChange(string ClothesID)
    {
            ChangeMaterial();
            if (ClothesID == "HairBack")
        {
            BackHairColorIndex = 0;
            if (Backhairs != null)
            {
                GameObject temp = (GameObject)Backhairs;
                Backhairs = null;
                DestroyImmediate(temp.GetComponent<Accessory>().armature.gameObject);
                DestroyImmediate(temp.GetComponent<Accessory>().gameObject);
            }
            if (BackHair != null)
            {
                GameObject SceneHair;
                GameObject SceneHairArmature;

                SceneHair = (GameObject)PrefabUtility.InstantiatePrefab(BackHair.Hair);
                SceneHair.transform.SetParent(this.gameObject.transform);
                SceneHair.transform.localPosition = Vector3.zero;
                PrefabUtility.UnpackPrefabInstance(SceneHair, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                SceneHairArmature = SceneHair.GetComponent<Accessory>().armature;
                SceneHairArmature.transform.SetParent(armheadRef.transform);
                    SceneHair.GetComponent<Accessory>().skin.GetComponent<SkinnedMeshRenderer>().material = body;
                    SceneHair.GetComponent<Accessory>().skin.sharedMesh = BackHair.ColorVar[0].items;
                Backhairs = SceneHair;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this.gameObject);

            }
        }
        else if (ClothesID == "HairFront")
        {
            FrontHairColorIndex = 0;
            if (Fronthairs != null)
            {
                GameObject temp = (GameObject)Fronthairs;
                Fronthairs = null;
                DestroyImmediate(temp.GetComponent<Accessory>().armature.gameObject);
                DestroyImmediate(temp.GetComponent<Accessory>().gameObject);
            }
            if (FrontHair != null)
            {
                GameObject SceneHair;
                GameObject SceneHairArmature;

                SceneHair = (GameObject)PrefabUtility.InstantiatePrefab(FrontHair.Hair);
                SceneHair.transform.SetParent(this.gameObject.transform);
                SceneHair.transform.localPosition = Vector3.zero;
                PrefabUtility.UnpackPrefabInstance(SceneHair, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                SceneHairArmature = SceneHair.GetComponent<Accessory>().armature;
                SceneHairArmature.transform.SetParent(armheadRef.transform);
                    SceneHair.GetComponent<Accessory>().skin.GetComponent<SkinnedMeshRenderer>().material = body;
                    SceneHair.GetComponent<Accessory>().skin.sharedMesh = FrontHair.ColorVar[0].items;
                Fronthairs = SceneHair;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this.gameObject);
            }
        }
        else if (ClothesID == "HairSide")
        {
            SideHairColorIndex = 0;
            if (Sidehairs != null)
            {
                GameObject temp = (GameObject)Sidehairs;
                Sidehairs = null;
                DestroyImmediate(temp.GetComponent<Accessory>().armature.gameObject);
                DestroyImmediate(temp.GetComponent<Accessory>().gameObject);
            }
            if (SideHair != null)
            {
                GameObject SceneHair;
                GameObject SceneHairArmature;

                SceneHair = (GameObject)PrefabUtility.InstantiatePrefab(SideHair.Hair);
                SceneHair.transform.SetParent(this.gameObject.transform);
                SceneHair.transform.localPosition = Vector3.zero;
                PrefabUtility.UnpackPrefabInstance(SceneHair, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                SceneHairArmature = SceneHair.GetComponent<Accessory>().armature;
                SceneHairArmature.transform.SetParent(armheadRef.transform);
                    SceneHair.GetComponent<Accessory>().skin.GetComponent<SkinnedMeshRenderer>().material = body;
                    SceneHair.GetComponent<Accessory>().skin.sharedMesh = SideHair.ColorVar[0].items;
                Sidehairs = SceneHair;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this.gameObject);
            }
        }
        else if (ClothesID == "Shirt")
        {
            if (shirt != null)
            {
                shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.shirt;
                L_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.shirt_L_Sleeve;
                R_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = shirt.shirt_R_Sleeve;
                ShirtColorIndex = 0;
            }
            else
            {
                shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_sleeve_shirtRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }

        else if (ClothesID == "Skin")
        {
            if (Skin != null)
            {
                headRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.Head;
                neckRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.Neck;
                chestRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.Chest;
                stomachRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.Stomach;
                lowerbodyRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.LowerBody;
                L_legRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.L_Leg;
                R_legRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.R_Leg;
                L_footRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.L_Foot;
                R_footRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.R_Foot;
                L_shoulderRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.L_Shoulder;
                R_shoulderRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.R_Shoulder;
                L_armRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.L_Arm;
                R_armRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.R_Arm;
                L_handRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.L_Hand;
                R_handRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Skin.R_Hand;

            }
            else
            {
                headRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                neckRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                chestRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                stomachRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                lowerbodyRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_legRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_legRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_footRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_footRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_shoulderRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_shoulderRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_armRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_armRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                L_handRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                R_handRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

            }
        }
        else if (ClothesID == "Pants")
        {
            if (Pants != null)
            {
                pantsRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Pants.Pants;
                PantsColorIndex = 0;

            }
            else
            {
                pantsRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

            }
        }
        else if (ClothesID == "Jacket")
        {
            if (Jacket != null)
            {
                jacketRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket.Jacket;
                JacketColorIndex = 0;

            }
            else
            {
                jacketRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

            }
        }
        else if (ClothesID == "Jacket2")
        {
            if (Jacket_2 != null)
            {
                jacket2Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket_2.Jacket;
                Jacket2ColorIndex = 0;
            }
            else
            {
                jacket2Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

            }
        }
        else if (ClothesID == "Jacket3")
        {
            if (Jacket_3 != null)
            {
                jacket3Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = Jacket_3.Jacket;
                Jacket3ColorIndex = 0;
            }
            else
            {
                jacket3Ref.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

            }
        }
        else if (ClothesID == "Shoes")
        {

            if (!AdvanceShoe)
            {
                ShoeColorIndex = 0;
                if (Shoe != null)
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Shoe.L_Shoe;
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Shoe.R_Shoe;

                }
                else
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }
            else
            {
                LShoeColorIndex = 0;
                RShoeColorIndex = 0;
                if (LShoe != null)
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = LShoe.L_Shoe;

                }
                else
                {
                    L_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
                if (RShoe != null)
                {
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = RShoe.R_Shoe;

                }
                else
                {
                    R_shoeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }

        }
        else if (ClothesID == "EyeBack")
        {

            if (!AdvEyeBack)
            {

                if (EyeBack != null)
                {
                    L_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBack.Leye;
                    R_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBack.Reye;

                }
                else
                {
                    L_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    R_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }
            else
            {
                if (LEyeBack != null)
                {
                    L_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = LEyeBack.Leye;

                }
                else
                {
                    L_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
                if (REyeBack != null)
                {
                    R_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = REyeBack.Reye;

                }
                else
                {
                    R_eyebackRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }

        }
        else if (ClothesID == "EyeBall")
        {

            if (!AdvEyeBall)
            {

                if (EyeBall != null)
                {
                    L_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBall.Leye;
                    R_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBall.Reye;

                }
                else
                {
                    L_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    R_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }
            else
            {
                if (LEyeBall != null)
                {
                    L_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = LEyeBall.Leye;

                }
                else
                {
                    L_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
                if (REyeBall != null)
                {
                    R_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = REyeBall.Reye;

                }
                else
                {
                    R_eyeRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }

        }
        else if (ClothesID == "EyeBrow")
        {

            if (!AdvEyeBrow)
            {

                if (EyeBrow != null)
                {
                    L_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBrow.Leye;
                    R_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = EyeBrow.Reye;

                }
                else
                {
                    L_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                    R_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }
            else
            {
                if (LEyeBrow != null)
                {
                    L_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = LEyeBrow.Leye;

                }
                else
                {
                    L_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
                if (REyeBrow != null)
                {
                    R_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = REyeBrow.Reye;

                }
                else
                {
                    R_eyebrowRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

                }
            }

        }
        else if (ClothesID == "Mouth")
        {
            if (Mouth != null)
            {
                mouthRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = Mouth.Mouth;


            }
            else
            {
                mouthRef.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;


            }
        }

        HideMesh();


        //sceneHair.transform.SetParent(HairHolder.transform);
        // PrefabUtility.UnpackPrefabInstance(sceneHair,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
        //sceneHair.GetComponent<Hair>().armature.transform.SetParent(HairArmatureHolder.transform);
    }
}
}
