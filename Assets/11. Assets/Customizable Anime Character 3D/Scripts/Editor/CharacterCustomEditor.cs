
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.AnimatedValues;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CharacterCustomizationSystem;

namespace CharacterCustomizationSystem
{
    [CustomEditor(typeof(CharacterCustomization))]
    public class CharacterCustomEditor : Editor
    {
        AnimBool m_ShowExtraFields;
        AnimBool m_ShowExtraShoe;
        SerializedProperty Accessories;
        bool AdvanceMaterial = false;
        bool showEyeBack = false;
        bool showEyeBall = false;
        bool showEyeBrow = false;
        bool showShoe = false;
        int spacebetween = 10;
        int SmallSpace = 5;
        int BigSpace = 20;
        void OnEnable()
        {
            m_ShowExtraFields = new AnimBool(false);
            m_ShowExtraFields.valueChanged.AddListener(Repaint);
            m_ShowExtraShoe = new AnimBool(false);
            m_ShowExtraShoe.valueChanged.AddListener(Repaint);
        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
        public override void OnInspectorGUI()
        {
            GUI.contentColor = Color.white;
            GUIStyle advstyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            GUIStyle PartHeader = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                margin = new RectOffset(20, 0, 0, 0),
                fixedWidth = 150
            };
            GUIStyle CardStyle = new GUIStyle();

            CardStyle.normal.background = MakeTex(600, 1, new Color(153.0f / 255, 212.0f / 255, 233.0f / 255, 0.2f));
            CardStyle.padding = new RectOffset(0, 0, 5, 10);

            GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button)
            {

                fontSize = 12,
                margin = new RectOffset(5, 5, 0, 0),
                fixedWidth = 120,
                fixedHeight = 25,


            };
            GUIStyle PopUpStyle = new GUIStyle(EditorStyles.popup)
            {

                fixedHeight = 20,


            };

            CardStyle.margin = new RectOffset(0, 0, 10, 10);

            GUIStyle TitleStyle = new GUIStyle();
            TitleStyle.normal.background = MakeTex(600, 1, new Color(0f, 0f, 0f, 0.2f));
            TitleStyle.fontSize = 20;
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.normal.textColor = Color.white;
            TitleStyle.padding = new RectOffset(20, 0, 0, 0);
            //base.
            //OnInspectorGUI();
            serializedObject.Update();
            GUIStyle whitetext = new GUIStyle();
            whitetext.normal.textColor = Color.white;

            CharacterCustomization c = (CharacterCustomization)target;

            Accessories = serializedObject.FindProperty("accessories");
            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Note: When dropping the character prefab into the scene.");
            GUILayout.Label("Remember to unpack the prefab before use.");
            GUILayout.Label("If not the customization will give error regarding the hair.");
            GUILayout.Label("Once the error appear the current prefab in the scene is unusable.");
            GUILayout.Label("a new prefab has to be dropped into the scene.");
            GUILayout.EndVertical();
            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Material", TitleStyle);
            GUILayout.Space(spacebetween);
            GUILayout.BeginHorizontal();

            EditorGUI.indentLevel++;
            GUILayout.Label("Material", PartHeader);
            c.MaterialIndex = EditorGUILayout.Popup(c.MaterialIndex, c.Materials, PopUpStyle);
            if (GUILayout.Button("Apply Mat", ButtonStyle))
            {
                c.ChangeMaterial();
            }
            
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel += 2;
            AdvanceMaterial = EditorGUILayout.Foldout(AdvanceMaterial, "Material List");
            if (AdvanceMaterial)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("URP_Body"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("URP_Face"));
                GUILayout.Space(SmallSpace);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BuiltIn_Body"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BuiltIn_Face"));
                GUILayout.Space(SmallSpace);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HFRP_Body"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HDRP_Face"));

            }
            EditorGUI.indentLevel -= 2;
            EditorGUI.indentLevel--;
            GUILayout.Space(spacebetween);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Hair", TitleStyle);
            GUILayout.Space(spacebetween);
            GUILayout.BeginHorizontal();

            EditorGUI.indentLevel++;
            GUILayout.Label("Back Hair", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BackHair"), GUIContent.none);
            if (GUILayout.Button("Apply Hair", ButtonStyle))
            {
                c.ApplyChange("HairBack");
            }
            GUILayout.EndHorizontal();
            if (c.BackHair != null && c.BackHair.ColorVar != null && c.BackHair.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.BackHair.ColorVar.Length];
                for (int j = 0; j < c.BackHair.ColorVar.Length; j++)
                {
                    altcolors[j] = c.BackHair.ColorVar[j].name;
                }
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;

                GUILayout.BeginHorizontal();
                c.BackHairColorIndex = EditorGUILayout.Popup(c.BackHairColorIndex, altcolors, PopUpStyle);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("HairBack");
                    
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel -= Screen.width / 60;
            }
            GUILayout.Space(spacebetween);
            // EditorGUILayout.PropertyField(serializedObject.FindProperty("Fronthairs"));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Front Hair", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontHair"), GUIContent.none);

            if (GUILayout.Button("Apply Hair", ButtonStyle))
            {
                c.ApplyChange("HairFront");
            }
            GUILayout.EndHorizontal();
            if (c.FrontHair != null && c.FrontHair.ColorVar != null && c.FrontHair.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.FrontHair.ColorVar.Length];
                for (int j = 0; j < c.FrontHair.ColorVar.Length; j++)
                {
                    altcolors[j] = c.FrontHair.ColorVar[j].name;
                }
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                GUILayout.BeginHorizontal();
                c.FrontHairColorIndex = EditorGUILayout.Popup(c.FrontHairColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("HairFront");
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel -= Screen.width / 60;
            }

            GUILayout.Space(BigSpace);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Side Hair", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SideHair"), GUIContent.none);

            if (GUILayout.Button("Apply Hair", ButtonStyle))
            {
                c.ApplyChange("HairSide");
            }
            GUILayout.EndHorizontal();
            if (c.SideHair != null && c.SideHair.ColorVar != null && c.SideHair.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.SideHair.ColorVar.Length];
                for (int j = 0; j < c.SideHair.ColorVar.Length; j++)
                {
                    altcolors[j] = c.SideHair.ColorVar[j].name;
                }
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                GUILayout.BeginHorizontal();
                c.SideHairColorIndex = EditorGUILayout.Popup(c.SideHairColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("HairSide");
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel -= Screen.width / 60;

            }
            EditorGUI.indentLevel--;
            GUILayout.Space(spacebetween);
            GUILayout.EndVertical();



            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Eyes", TitleStyle);
            GUILayout.Space(spacebetween);
            EditorGUI.indentLevel++;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Eye Back", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EyeBack"), GUIContent.none);
            if (GUILayout.Button("Apply EyeBack", ButtonStyle))
            {
                c.ApplyChange("EyeBack");
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel += 2;
            showEyeBack = EditorGUILayout.Foldout(showEyeBack, "Different EyeBack");
            if (showEyeBack)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AdvEyeBack"));
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LEyeBack"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("REyeBack"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel -= 2;
            GUILayout.Space(SmallSpace);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Eye Ball", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EyeBall"), GUIContent.none);
            if (GUILayout.Button("Apply EyeBall", ButtonStyle))
            {
                c.ApplyChange("EyeBall");
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel += 2;
            showEyeBall = EditorGUILayout.Foldout(showEyeBall, "Different Eyeball");
            if (showEyeBall)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AdvEyeBall"));
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LEyeBall"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("REyeBall"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel -= 2;
            GUILayout.Space(SmallSpace);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Eye Brow", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EyeBrow"), GUIContent.none);
            if (GUILayout.Button("Apply EyeBrow", ButtonStyle))
            {
                c.ApplyChange("EyeBrow");
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel += 2;
            showEyeBrow = EditorGUILayout.Foldout(showEyeBrow, "Different Eyebrow");
            if (showEyeBrow)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AdvEyeBrow"));
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LEyeBrow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("REyeBrow"));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel -= 2;
            EditorGUI.indentLevel--;
            GUILayout.Space(SmallSpace);
            GUILayout.EndVertical();


            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Other Body Part", TitleStyle);
            GUILayout.Space(spacebetween);
            EditorGUI.indentLevel++;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mouth", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Mouth"), GUIContent.none);
            if (GUILayout.Button("Apply Mouth", ButtonStyle))
            {
                c.ApplyChange("Mouth");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(SmallSpace);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Skin Color", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Skin"), GUIContent.none);
            if (GUILayout.Button("Apply Skin Color", ButtonStyle))
            {
                c.ApplyChange("Skin");
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();


            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Clothes", TitleStyle);
            GUILayout.Space(spacebetween);


            EditorGUI.indentLevel++;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Shirt", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shirt"), GUIContent.none);
            if (GUILayout.Button("Apply Shirt", ButtonStyle))
            {
                c.ApplyChange("Shirt");
            }
            GUILayout.EndHorizontal();
            if (c.shirt != null && c.shirt.ColorVar != null && c.shirt.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.shirt.ColorVar.Length];
                for (int j = 0; j < c.shirt.ColorVar.Length; j++)
                {
                    altcolors[j] = c.shirt.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.ShirtColorIndex = EditorGUILayout.Popup(c.ShirtColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("Shirt");
                }
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();




            }
            GUILayout.Space(spacebetween);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Jacket 1", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Jacket"), GUIContent.none);
            if (GUILayout.Button("Apply Jacket", ButtonStyle))
            {
                c.ApplyChange("Jacket");
            }
            GUILayout.EndHorizontal();
            if (c.Jacket != null && c.Jacket.ColorVar.Length > 1)
            {
                string[] altcolors = new string[c.Jacket.ColorVar.Length];
                for (int j = 0; j < c.Jacket.ColorVar.Length; j++)
                {
                    altcolors[j] = c.Jacket.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.JacketColorIndex = EditorGUILayout.Popup(c.JacketColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("Jacket");
                }
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(spacebetween);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Jacket 2", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Jacket_2"), GUIContent.none);
            if (GUILayout.Button("Apply Jacket", ButtonStyle))
            {
                c.ApplyChange("Jacket2");
            }
            GUILayout.EndHorizontal();
            if (c.Jacket_2 != null && c.Jacket_2.ColorVar.Length > 1)
            {
                string[] altcolors = new string[c.Jacket_2.ColorVar.Length];
                for (int j = 0; j < c.Jacket_2.ColorVar.Length; j++)
                {
                    altcolors[j] = c.Jacket_2.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.Jacket2ColorIndex = EditorGUILayout.Popup(c.Jacket2ColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("Jacket2");
                }
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(spacebetween);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Jacket 3", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Jacket_3"), GUIContent.none);
            if (GUILayout.Button("Apply Jacket", ButtonStyle))
            {
                c.ApplyChange("Jacket3");
            }
            GUILayout.EndHorizontal();
            if (c.Jacket_3 != null && c.Jacket_3.ColorVar.Length > 1)
            {
                string[] altcolors = new string[c.Jacket_3.ColorVar.Length];
                for (int j = 0; j < c.Jacket_3.ColorVar.Length; j++)
                {
                    altcolors[j] = c.Jacket_3.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.Jacket3ColorIndex = EditorGUILayout.Popup(c.Jacket3ColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("Jacket3");
                }
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(spacebetween);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Pants", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Pants"), GUIContent.none);
            if (GUILayout.Button("Apply Pants", ButtonStyle))
            {
                c.ApplyChange("Pants");
            }
            GUILayout.EndHorizontal();
            if (c.Pants != null && c.Pants.ColorVar != null && c.Pants.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.Pants.ColorVar.Length];
                for (int j = 0; j < c.Pants.ColorVar.Length; j++)
                {
                    altcolors[j] = c.Pants.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.PantsColorIndex = EditorGUILayout.Popup(c.PantsColorIndex, altcolors);
                if (GUILayout.Button("Change Color", ButtonStyle))
                {
                    c.ChangeColor("Pants");
                }
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();

            }
            GUILayout.Space(spacebetween);


            GUILayout.BeginHorizontal();
            GUILayout.Label("Shoes", PartHeader);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Shoe"), GUIContent.none);


            GUILayout.EndHorizontal();
            if (c.Shoe != null && c.Shoe.ColorVar != null && c.Shoe.ColorVar.Length > 1)
            {

                string[] altcolors = new string[c.Shoe.ColorVar.Length];
                for (int j = 0; j < c.Shoe.ColorVar.Length; j++)
                {
                    altcolors[j] = c.Shoe.ColorVar[j].name;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(SmallSpace);
                EditorGUI.indentLevel += Screen.width / 60;
                c.ShoeColorIndex = EditorGUILayout.Popup(c.ShoeColorIndex, altcolors);
                EditorGUI.indentLevel -= Screen.width / 60;
                GUILayout.EndHorizontal();

            }
            EditorGUI.indentLevel += 2;
            showShoe = EditorGUILayout.Foldout(showShoe, "Different Shoes");
            if (showShoe)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AdvanceShoe"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LShoe"));
                if (c.LShoe != null && c.LShoe.ColorVar != null && c.LShoe.ColorVar.Length > 1)
                {

                    string[] altcolors = new string[c.LShoe.ColorVar.Length];
                    for (int j = 0; j < c.LShoe.ColorVar.Length; j++)
                    {
                        altcolors[j] = c.LShoe.ColorVar[j].name;
                    }

                    GUILayout.BeginHorizontal();
                    c.LShoeColorIndex = EditorGUILayout.Popup(c.LShoeColorIndex, altcolors);

                    GUILayout.EndHorizontal();

                }
                GUILayout.Space(SmallSpace);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("RShoe"));
                if (c.RShoe != null && c.RShoe.ColorVar != null && c.RShoe.ColorVar.Length > 1)
                {

                    string[] altcolors = new string[c.RShoe.ColorVar.Length];
                    for (int j = 0; j < c.RShoe.ColorVar.Length; j++)
                    {
                        altcolors[j] = c.RShoe.ColorVar[j].name;
                    }

                    GUILayout.BeginHorizontal();
                    c.RShoeColorIndex = EditorGUILayout.Popup(c.RShoeColorIndex, altcolors);

                    GUILayout.EndHorizontal();

                }
                GUILayout.Space(spacebetween);
            }



            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Change Color"))
            {
                c.ChangeColor("Shoes");
            }
            if (GUILayout.Button("Apply Shoe"))
            {
                c.ApplyChange("Shoes");

            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel -= 2;
            GUILayout.EndVertical();

            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Accessories", TitleStyle);
            GUILayout.Space(spacebetween);



            EditorGUI.indentLevel++;
            EditorGUILayout.Space();



            for (int i = 0; i < Accessories.arraySize; i++)
            {
                GUILayout.BeginVertical(CardStyle);
                GUILayout.BeginHorizontal();
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("accessories"));

                EditorGUILayout.PropertyField(Accessories.GetArrayElementAtIndex(i));
                GUILayout.EndHorizontal();
                if (c.accessories[i].Accessory != null)
                {
                    if (c.accessories[i].Accessory.ColorVar.Length > 0)
                    {
                        string[] altcolors = new string[c.accessories[i].Accessory.ColorVar.Length];
                        for (int j = 0; j < c.accessories[i].Accessory.ColorVar.Length; j++)
                        {
                            altcolors[j] = c.accessories[i].Accessory.ColorVar[j].name;
                        }

                        GUILayout.BeginHorizontal();
                        c.accessories[i].AltIndex = EditorGUILayout.Popup(c.accessories[i].AltIndex, altcolors);
                        if (GUILayout.Button("Change Color"))
                        {
                            c.ChangeAccessoryColor(i);
                        }
                        GUILayout.EndHorizontal();
                    }
                }



                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply Accessory"))
                {
                    c.ApplyAccessory(i);
                }
                if (GUILayout.Button("Change Parent"))
                {
                    c.ChangeParent(i);
                }
                if (GUILayout.Button("Remove"))
                {
                    c.RemoveAccessories(i);
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                EditorGUILayout.Space(SmallSpace);
            }
            if (GUILayout.Button("Add Accessories"))
            {
                c.AddAccesories();
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();


            GUILayout.BeginVertical(CardStyle);
            GUILayout.Label("Hide Items", TitleStyle);
            GUILayout.Space(spacebetween);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Hide"), GUIContent.none);
            if (GUILayout.Button("Apply hide", ButtonStyle))
            {
                c.HideMesh();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


            EditorGUILayout.Space();


            m_ShowExtraFields.target = EditorGUILayout.ToggleLeft("Advance", m_ShowExtraFields.target, advstyle);

            if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFields.faded))
            {
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Body Parts Reference");
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Body", advstyle);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("headRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("neckRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chestRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stomachRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lowerbodyRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_legRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_legRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_footRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_footRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_shoulderRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_shoulderRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_armRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_armRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_handRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_handRef"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Face", advstyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_eyeRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_eyeRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_eyebrowRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_eyebrowRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_eyebackRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_eyebackRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mouthRef"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Clothes", advstyle);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shirtRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_sleeve_shirtRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_sleeve_shirtRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pantsRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jacketRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jacket2Ref"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jacket3Ref"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("L_shoeRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("R_shoeRef"));


                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;

                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Armature Reference");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_Root_Ref"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("armheadRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("armneckRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("armchestRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("armstomachRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_HipRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_HipRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_UpperlegRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_UpperlegRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_LowerlegRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_LowerlegRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_footRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_footRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_toeRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_toeRef"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_ShoulderRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_ShoulderRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_UpperArmRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_UpperArmRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_LowerArmRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_LowerArmRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_HandRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_HandRef"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_BaseThumbRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_TipThumbRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_BaseIndexFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_TipIndexFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_BaseMiddleFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_TipMiddleFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_BaseRingFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_TipRingFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_BasePinkyFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_L_TipPinkyFingerRef"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_BaseThumbRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_TipThumbRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_BaseIndexFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_TipIndexFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_BaseMiddleFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_TipMiddleFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_BaseRingFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_TipRingFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_BasePinkyFingerRef"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("arm_R_TipPinkyFingerRef"));

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFadeGroup();
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(c);
                EditorSceneManager.MarkSceneDirty(c.gameObject.scene);
            }
        }


    }
}

