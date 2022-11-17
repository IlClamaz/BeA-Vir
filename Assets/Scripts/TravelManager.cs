using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class TravelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] areas;
    [SerializeField] private GameObject[] lightBeams;
    [SerializeField] private GameObject[] overlaysCanvas;
    [SerializeField] private CinemachineStoryboard cinestory;
    [SerializeField] private Transform cam;
    [SerializeField] private Material lightSkybox;
    [SerializeField] private Material blackSkybox;
    [SerializeField] private Light dirLight;
    [SerializeField] private GameObject JPConPrefetture;
    [SerializeField] private GameObject JPConPrefettureDark;
    [SerializeField] public Texture blackImage;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip throughGateClip;
    private CharacterController characterController;
    private FirstPersonController firstPersonController;
    private AdditionalControls additionalControls;
    private bool teleFlag;
    private string[] telePointName;
    private Quaternion[] lightBeamsRotations;
    private Vector3[] lightBeamsScales;
    private GameObject currentArea;
    private GameObject dest_area;
    private SubSpace subSpace;
    private GameObject targetPoint;
    private GameObject[] targetPointsA;
    public GameObject sightPoint;
    private FadingTrigger ft;
    private Color darkColor;
    private GameObject telepoint;
    void Awake()
    {
        ft = GetComponent<FadingTrigger>();
        firstPersonController = GetComponent<FirstPersonController>();
        characterController = GetComponent<CharacterController>();
        additionalControls = GetComponent<AdditionalControls>();
        telePointName = new string[] { "" };
        teleFlag = false;
        darkColor = new Color(0.85f, 0.85f, 0.85f, 1);
        lightBeamsRotations = new Quaternion[lightBeams.Length];
        lightBeamsScales = new Vector3[lightBeams.Length];
        for (int i = 0; i < lightBeams.Length; i++)
        {
            lightBeamsRotations[i] = lightBeams[i].transform.rotation;
            lightBeamsScales[i] = lightBeams[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (teleFlag)
        {
            StartCoroutine(FTBCoroutine(1, 1f, blackImage, false));
            teleFlag = false;
        }
    }

    public IEnumerator FTBCoroutine(float value_end, float duration, Texture image, bool sight)
    {
        //DISABLE GRAVITY AND CONTROLS
        firstPersonController.enabled = false;
        characterController.enabled = false;

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
        if (!sight)
        {
            if (telePointName.Length == 1 && value_end == 1) TeleportTo("A"); //IF CALLED PRESSING TRIANGLE, TELEPORT HOME
            else if (telePointName.Length != 1 && value_end == 1) TeleportTo(telePointName[1]); //WHEN THE SCREEN IS BLACK, TELEPORT
        }
        else
        {
            if (value_end == 0)
            {
                firstPersonController.enabled = true;
                characterController.enabled = true;
            }
            else if (sightPoint != null) SightSeeing();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TelePoint")
        {
            teleFlag = true;
            telepoint = other.gameObject;
            telePointName = telepoint.name.Split("_");
            //print(telePointName[1]);
        }
    }

    private void TeleportTo(string dest)
    {
        //GET NEW AREA, RESET OLD ONE E SET THE NEW ONE ACTIVE
        audioSource.PlayOneShot(throughGateClip);
        telePointName = new string[] { "" };
        currentArea = GameObject.FindGameObjectWithTag("Area");
        foreach (var area in areas) if (area.name.Equals(dest)) dest_area = area;

        currentArea.BroadcastMessage("Reset", SendMessageOptions.DontRequireReceiver);
        currentArea.BroadcastMessage("ResetTitleUI", SendMessageOptions.DontRequireReceiver);
        currentArea.BroadcastMessage("MinaReset", SendMessageOptions.DontRequireReceiver);
        currentArea.BroadcastMessage("ResetTrincee", SendMessageOptions.DontRequireReceiver);

        currentArea.SetActive(false);
        dest_area.SetActive(true);

        //TELEPORT TO 150MT HEIGHT
        targetPoint = GameObject.FindGameObjectWithTag("TargetPoint");
        if (dest_area.name != "A")
        {
            //telepoint.GetComponent<MeshRenderer>().enabled = false;
            transform.position = new Vector3(targetPoint.transform.position.x, 150f, targetPoint.transform.position.z);
            transform.rotation = new Quaternion(0.707106829f, 0, 0, 0.707106829f); //90,0,0
            cam.rotation = new Quaternion(0.707106829f, 0, 0, 0.707106829f);
        }
        else
        {
            targetPointsA = GameObject.FindGameObjectsWithTag("TargetPoint");
            targetPoint = targetPointsA[Random.Range(0, targetPointsA.Length)];
            transform.position = new Vector3(targetPoint.transform.position.x, 10f, targetPoint.transform.position.z);
            transform.rotation = Quaternion.Euler(90, targetPoint.transform.rotation.eulerAngles.y, 0);
            cam.rotation = Quaternion.Euler(90, targetPoint.transform.rotation.eulerAngles.y, 0);
        }

        //ROTATE CAMERA IN ORDER TO VIEW FROM TOP TO BOTTOM
        firstPersonController._cinemachineTargetPitch = 0;

        //ROTATE LIGHTBEAMS ACCORDING TO AREA
        ManageLightBeams();

        //CHECK IF LIGHTS SHOULD BE SWITCHED OFF
        subSpace = GameObject.FindObjectOfType<SubSpace>();
        if (dest_area.name == "B1" || dest_area.name == "B2" || dest_area.name == "C" || dest_area.name == "E" || dest_area.name == "F" ||
        dest_area.name == "B3" || dest_area.name == "D" && subSpace != null && subSpace.changeLights)
            ChangeLights(true);
        else
            ChangeLights(false);

        //UPDATE CURRENT CANVAS TO SHOW
        additionalControls.cameraOverlayCanvas.SetActive(false);
        if (subSpace == null)
        {
            foreach (var canvas in overlaysCanvas)
            {
                var name = canvas.name.Split('_');
                if (name[1].Equals(dest_area.name)) additionalControls.cameraOverlayCanvas = canvas;
            }
        }
        else additionalControls.cameraOverlayCanvas = subSpace.canvas;

        //FADE TO BLACK
        StartCoroutine(FTBCoroutine(0, 1f, blackImage, false));

        //STARTS AGAIN THE FALLING ROUTINE 
        ft.StartFromSky();
    }

    private void SightSeeing()
    {
        transform.position = new Vector3(sightPoint.transform.position.x, transform.position.y, sightPoint.transform.position.z);
        transform.rotation = Quaternion.Euler(0, sightPoint.transform.rotation.eulerAngles.y, 0);
        cam.rotation = Quaternion.Euler(0, sightPoint.transform.rotation.eulerAngles.y, 0);
        firstPersonController._cinemachineTargetPitch = 0;
        currentArea = GameObject.FindGameObjectWithTag("Area");
        currentArea.BroadcastMessage("Reset", SendMessageOptions.DontRequireReceiver);
        StartCoroutine(FTBCoroutine(0, 0.3f, blackImage, true));
    }

    public void ChangeLights(bool dark)
    {
        Material skybox;
        Color lightColor;
        if (dark)
        {
            skybox = blackSkybox;
            lightColor = darkColor;
            JPConPrefettureDark.SetActive(true);
            JPConPrefetture.SetActive(false);
        }
        else
        {
            skybox = lightSkybox;
            lightColor = Color.white;
            JPConPrefettureDark.SetActive(false);
            JPConPrefetture.SetActive(true);
        }
        RenderSettings.skybox = skybox;
        dirLight.color = lightColor;
        DynamicGI.UpdateEnvironment();
    }

    private void ManageLightBeams()
    {
        for (int i = 0; i < lightBeams.Length; i++)
        {
            if (dest_area.name == "A")
            {
                lightBeams[i].SetActive(true);
                lightBeams[i].transform.rotation = lightBeamsRotations[i];
                lightBeams[i].transform.localScale = lightBeamsScales[i];
            }
            else
            {
                if (lightBeams[i].name.Contains(dest_area.name))
                {
                    lightBeams[i].SetActive(true);
                    lightBeams[i].transform.rotation = Quaternion.identity;
                    lightBeams[i].transform.localScale = Vector3.one;
                }
                else lightBeams[i].SetActive(false);
            }
        }
    }
}
