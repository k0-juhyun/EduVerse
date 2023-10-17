using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

namespace CharacterCustomizationSystem
{
    [CustomEditor(typeof(ClothTemplate))]
    [CanEditMultipleObjects]
    public class Preview : Editor
    {
        // Start is called before the first frame update
        private ClothTemplate s;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            s = (ClothTemplate)target;
            if (s.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(s.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        s.icon.width,
                        s.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(s.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(s.icon.width, s.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(s.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(ShirtTemplate))]
    [CanEditMultipleObjects]
    public class ShirtPreview : Editor
    {
        // Start is called before the first frame update
        private ShirtTemplate shirtprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            shirtprev = (ShirtTemplate)target;
            if (shirtprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        shirtprev.icon.width,
                        shirtprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(shirtprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(shirtprev.icon.width, shirtprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(shirtprev.icon), subAssets, width, height);
            }



        }
    }
    [CustomEditor(typeof(PantsTemplate))]
    [CanEditMultipleObjects]
    public class PantsPreview : Editor
    {
        // Start is called before the first frame update
        private PantsTemplate pantprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            pantprev = (PantsTemplate)target;
            if (pantprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        pantprev.icon.width,
                        pantprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(pantprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(pantprev.icon.width, pantprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(pantprev.icon), subAssets, width, height);
            }



        }
    }
    [CustomEditor(typeof(JacketTemplate))]
    [CanEditMultipleObjects]
    public class JacketPreview : Editor
    {
        // Start is called before the first frame update
        private JacketTemplate shirtprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            shirtprev = (JacketTemplate)target;
            if (shirtprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        shirtprev.icon.width,
                        shirtprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(shirtprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(shirtprev.icon.width, shirtprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(shirtprev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(ShoeTemplate))]
    [CanEditMultipleObjects]
    public class ShoePreview : Editor
    {
        // Start is called before the first frame update
        private ShoeTemplate shoeprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            shoeprev = (ShoeTemplate)target;
            if (shoeprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        shoeprev.icon.width,
                        shoeprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(shoeprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(shoeprev.icon.width, shoeprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(shoeprev.icon), subAssets, width, height);
            }



        }
    }
    [CustomEditor(typeof(FrontHairTemplate))]
    [CanEditMultipleObjects]
    public class FrontHairPreview : Editor
    {
        // Start is called before the first frame update
        private FrontHairTemplate FHprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            FHprev = (FrontHairTemplate)target;
            if (FHprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        FHprev.icon.width,
                        FHprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(FHprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(FHprev.icon.width, FHprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(FHprev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(BackHairTemplate))]
    [CanEditMultipleObjects]
    public class BackHairPreview : Editor
    {
        // Start is called before the first frame update
        private BackHairTemplate BHprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            BHprev = (BackHairTemplate)target;
            if (BHprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        BHprev.icon.width,
                        BHprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(BHprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(BHprev.icon.width, BHprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(BHprev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(SideHairTemplate))]
    [CanEditMultipleObjects]
    public class SideHairPreview : Editor
    {
        // Start is called before the first frame update
        private SideHairTemplate SHprev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            SHprev = (SideHairTemplate)target;
            if (SHprev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        SHprev.icon.width,
                        SHprev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(SHprev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(SHprev.icon.width, SHprev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(SHprev.icon), subAssets, width, height);
            }



        }
    }
    [CustomEditor(typeof(EyeTemplate))]
    [CanEditMultipleObjects]
    public class EyeBackPreview : Editor
    {
        // Start is called before the first frame update
        private EyeTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (EyeTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(EyeBallsTemplate))]
    [CanEditMultipleObjects]
    public class EyeBallPreview : Editor
    {
        // Start is called before the first frame update
        private EyeBallsTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (EyeBallsTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(EyeBrowTemplate))]
    [CanEditMultipleObjects]
    public class EyeBrowPreview : Editor
    {
        // Start is called before the first frame update
        private EyeBrowTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (EyeBrowTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(MouthTemplate))]
    [CanEditMultipleObjects]
    public class MouthPreview : Editor
    {
        // Start is called before the first frame update
        private MouthTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (MouthTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }
    [CustomEditor(typeof(AccessoriesTemplate))]
    [CanEditMultipleObjects]
    public class AccessoryPreview : Editor
    {
        // Start is called before the first frame update
        private AccessoriesTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (AccessoriesTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }

    [CustomEditor(typeof(SkinTemplate))]
    [CanEditMultipleObjects]
    public class SkinPreview : Editor
    {
        // Start is called before the first frame update
        private SkinTemplate prev;
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            prev = (SkinTemplate)target;
            if (prev.icon != null)
            {
                //Texture2D tex = new Texture2D(width, height);
                //EditorUtility.CopySerialized(shirtprev.icon, tex);
                RenderTexture renderTex = RenderTexture.GetTemporary(
                        prev.icon.width,
                        prev.icon.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                Graphics.Blit(prev.icon, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(prev.icon.width, prev.icon.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);
                return readableText;
            }
            else
            {
                return base.RenderStaticPreview(AssetDatabase.GetAssetPath(prev.icon), subAssets, width, height);
            }



        }
    }
}