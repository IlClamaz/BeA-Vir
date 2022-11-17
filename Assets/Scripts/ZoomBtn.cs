using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ZoomBtn : MonoBehaviour
{
    [SerializeField] private GameObject reperto;
    //[SerializeField] private AdditionalControls additionalControls;
    [SerializeField] private float scaleDelta;
    private Vector3 repertoPosition;
    private Vector3 repertoScale;
    private MeshRenderer piattaforma;
    //private FindReport findReport;
    //private TextMeshProUGUI[] reportContent;
    //private TextMeshProUGUI[] overlayContent;
    //private RawImage overlayImage;
    private bool playerFlag;
    // Start is called before the first frame update
    void Start()
    {
        if (reperto != null)
        {
            gameObject.transform.parent.gameObject.transform.parent.gameObject.TryGetComponent(out piattaforma); //prendo il nonno, cioè la piattaforma
            repertoPosition = reperto.transform.localPosition;
            repertoScale = reperto.transform.localScale;
            if (scaleDelta == 0) scaleDelta = 1.2f;
        }
        /* gameObject.transform.parent.gameObject.TryGetComponent(out findReport);
        if (findReport != null) reportContent = findReport.schedaReperto.GetComponentsInChildren<TextMeshProUGUI>();
        if (additionalControls != null)
        {
            overlayContent = additionalControls.cameraOverlayCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            overlayImage = additionalControls.cameraOverlayCanvas.transform.GetChild(1).GetComponent<RawImage>();
        } */
        playerFlag = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {

            if (reperto.transform.localScale == repertoScale)
            {
                reperto.transform.localScale = repertoScale * scaleDelta;
                if (piattaforma != null && piattaforma.enabled) piattaforma.enabled = false;
                /* if (findReport != null && additionalControls != null)
                {
                    findReport.schedaReperto.SetActive(false);
                    overlayImage.enabled = false;
                    overlayContent[0].text = "DATABASE REPORT";
                    overlayContent[1].text = reportContent[0].text + "\n" + reportContent[1].text + "\n" + reportContent[2].text;
                } */

            }
            else
            {
                Reset();
                /* if (findReport != null && additionalControls != null)
                {
                    findReport.schedaReperto.SetActive(true);
                } */
            }
            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
        //print("zoomenter");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
        //print("zoomexit");
    }

    private void Reset()
    {
        //print("resetZoom");
        reperto.transform.localPosition = repertoPosition;
        reperto.transform.localScale = repertoScale;
        if (piattaforma != null) piattaforma.enabled = true;
        /* if (additionalControls != null)
        {
            overlayImage.enabled = true;
            overlayContent[0].text = "";
            overlayContent[1].text = "";
        } */
    }
}
