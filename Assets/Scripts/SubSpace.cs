using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SubSpace : MonoBehaviour
{
    [SerializeField] public bool changeLights;
    [SerializeField] private Material lightSkybox;
    [SerializeField] private Material blackSkybox;
    [SerializeField] private Light dirLight;
    [SerializeField] private GameObject jPConPrefetture;
    [SerializeField] private GameObject jPConPrefettureDark;
    //[SerializeField] private Texture whiteImage;
    [SerializeField] public GameObject canvas;
    [SerializeField] private AdditionalControls additionalControls;
    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private GameObject[] parentSightPoints;
    [SerializeField] private bool sightable;
    //[SerializeField] private CinemachineStoryboard cinestory;
    private Color lightColor = Color.white;
    private Color blackColor = new Color(0.85f, 0.85f, 0.85f, 1);


    void OnEnable()
    {
        additionalControls.cameraOverlayCanvas.SetActive(false);
        additionalControls.cameraOverlayCanvas = canvas;
        if (sightable) additionalControls.sightPoints = GameObject.FindGameObjectsWithTag("SightPoint");
        else additionalControls.sightPoints = null;
        if (changeLights)
        {
            RenderSettings.skybox = blackSkybox;
            dirLight.color = blackColor;
            jPConPrefettureDark.SetActive(true);
            jPConPrefetture.SetActive(false);
            DynamicGI.UpdateEnvironment();
        }
    }
    void OnDisable()
    {
        if (additionalControls != null && dirLight != null)
        {
            additionalControls.cameraOverlayCanvas.SetActive(false);
            additionalControls.cameraOverlayCanvas = parentCanvas;
            if (parentSightPoints != null) additionalControls.sightPoints = parentSightPoints;
        }
        if (dirLight != null && changeLights)
        {
            RenderSettings.skybox = lightSkybox;
            dirLight.color = lightColor;
            jPConPrefettureDark.SetActive(false);
            jPConPrefetture.SetActive(true);
            DynamicGI.UpdateEnvironment();
        }
    }

    /*  public IEnumerator FTBCoroutine(float value_end, float duration, Texture image)
     {
         cinestory.m_Image = image;
         float startAlpha = cinestory.m_Alpha;
         float elapsed = 0.0f;
         while (elapsed < duration)
         {
             cinestory.m_Alpha = Mathf.Lerp(startAlpha, value_end, elapsed / duration);
             elapsed += Time.deltaTime;
             yield return null;
         }

         cinestory.m_Alpha = value_end;
     } */
}
