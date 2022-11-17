using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtensions
{
    public static void ToOpaqueMode(this Material material)
    {
        if (material.shader.name == "Standard")
        {
            //Debug.Log("ToOpaqueMode");
            material.SetFloat("_Mode", 0);
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }

    }

    public static void ToFadeMode(this Material material, bool fadingFromStart)
    {
        switch (material.shader.name)
        {
            case "Standard":
                material.SetFloat("_Mode", 2);
                material.SetOverrideTag("RenderType", "Fade");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                if (fadingFromStart)
                {
                    Color currentCol = material.color;
                    material.color = new Color(currentCol.r, currentCol.g, currentCol.b, 0); //Only third floor visible
                }
                break;
            case "TreeCreatorLeavesFast":
                material.mainTextureScale = new Vector2(0, 0);
                break;
            case "FrostedGlass":
                material.SetFloat("_FrostIntensity", 0.0f);
                break;
        }
    }
}
