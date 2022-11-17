using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOverlay : MonoBehaviour
{
    [SerializeField] AdditionalControls additionalControls;
    [SerializeField] Texture overlay;
    [SerializeField] private bool destroy;
    [SerializeField] private Texture hall;
    private bool playerFlag;
    private RawImage rawImage;
    private Texture oldOverlay;
    // Start is called before the first frame update
    void Start()
    {
        //print("ciao");
        playerFlag = false;
        rawImage = additionalControls.cameraOverlayCanvas.transform.GetChild(1).GetComponent<RawImage>();
        oldOverlay = rawImage.texture;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            //print(oldOverlay.name + " " + rawImage.texture);
            if (rawImage.texture.name == "welcome" && hall != null)
            {
                rawImage = additionalControls.cameraOverlayCanvas.transform.GetChild(1).GetComponent<RawImage>();
                oldOverlay = rawImage.texture;
                //print("ciao");
            }
            if (rawImage.texture == oldOverlay)
            {
                rawImage.texture = overlay;
                //print("cambio overlay");
            }
            else rawImage.texture = oldOverlay;
            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    void Reset()
    {
        rawImage.texture = oldOverlay;
    }

    void OnDisable()
    {
        if (destroy && rawImage.texture != null)
        {
            rawImage.texture = oldOverlay;
        }
    }
}
