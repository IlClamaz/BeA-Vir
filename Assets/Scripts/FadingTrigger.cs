using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class FadingTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject startCanvas;
    private GameObject terrainsToBeEnabled;
    private GameObject[] ffObjs;
    private GameObject[] sfObjs;
    private GameObject[] tfObjs;
    private GameObject[] ringObjs;
    private GameObject[] glassObjs;
    private GameObject[] findReportsObjs;
    private GameObject[] treesObjs;
    private bool ffObjsVisibile;
    private bool sfObjsVisibile;
    private bool terrainVisibile;
    public bool rotated;
    public bool rotatedPlayer;
    private bool isLanding;
    private bool terrainFlag;

    [SerializeField] private float fadingDuration;
    private PlayerInput playerInput;
    private FirstPersonController controller;
    private CharacterController cc;
    private AdditionalControls additionalControls;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<FirstPersonController>();
        cc = GetComponent<CharacterController>();
        additionalControls = GetComponent<AdditionalControls>();
        StartFromSky();
    }

    // Update is called once per frame
    void Update()
    {
        //print(transform.position);
        if (terrainsToBeEnabled != null && transform.position.y < 120 && !terrainVisibile)
        {
            if (terrainsToBeEnabled != null && !terrainsToBeEnabled.activeSelf) terrainsToBeEnabled.SetActive(true); //if terrain, enable
            terrainVisibile = true;
            var lightBeam = GameObject.FindGameObjectWithTag("LightBeam");
            lightBeam.SetActive(false);
        }

        if (transform.position.y < 100 && !sfObjsVisibile)
        {
            //print("coru1");
            if (ringObjs != null) StartCoroutine(FadingCoroutine(ringObjs, 0, 0.4f, fadingDuration));
            if (treesObjs != null) StartCoroutine(FadingTreesCoroutine(treesObjs, 0, 1, fadingDuration));
            StartCoroutine(FadingCoroutine(sfObjs, 0, 1f, fadingDuration));
            sfObjsVisibile = true;
            logo.SetActive(false);
        }

        if (transform.position.y < 50 && !ffObjsVisibile)
        {
            //print("coru2");
            StartCoroutine(FadingCoroutine(ffObjs, 0, 1f, fadingDuration));
            ffObjsVisibile = true;
        }

        if (transform.position.y <= 8 && !isLanding)
        {
            //print("coru3");
            controller.enabled = true;
            cc.enabled = true;
            controller.Gravity = -9.8f;
            controller.GroundLayers = LayerMask.GetMask("Terrain");
            controller.Grounded = true;
            isLanding = true;
        }

        if (controller.Grounded && !rotated)
        {
            StartCoroutine(RotationCoroutine(0.5f));
            rotated = true;
            terrainFlag = false;
        }

    }

    private IEnumerator FadingCoroutine(GameObject[] objs, float start_value, float value_end, float duration)
    {
        foreach (var glass in glassObjs) glass.SetActive(true); //now glass are visibile and can be increased opacity

        float elapsed = 0.0f;
        float opacityShift = 0.0f;
        Color currentColor = new Color();
        while (elapsed < duration)
        {
            foreach (var go in objs)
            {
                Material[] mats = go.GetComponent<MeshRenderer>().materials;
                foreach (var mat in mats)
                {
                    if (mat.shader.name != "Standard")
                    {
                        if (!go.activeSelf) go.SetActive(true);
                        opacityShift = Mathf.Lerp(start_value, value_end, elapsed / duration);
                        mat.SetFloat("_FrostIntensity", opacityShift);
                    }
                    else
                    {
                        float currentAlpha = Mathf.Lerp(start_value, value_end, elapsed / duration);
                        currentColor.r = mat.color.r;
                        currentColor.g = mat.color.g;
                        currentColor.b = mat.color.b;
                        currentColor.a = currentAlpha;
                        mat.SetColor("_Color", currentColor);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadingTreesCoroutine(GameObject[] objs, float start_value, float value_end, float duration)
    {
        float elapsed = 0.0f;
        Vector2 currentSize = new Vector2();
        Color currentColor = new Color();
        while (elapsed < duration)
        {
            foreach (var go in objs)
            {
                Material[] mats = go.GetComponent<MeshRenderer>().materials;
                foreach (var mat in mats)
                {
                    if (mat.shader.name != "Standard")
                    {
                        float currentShift = Mathf.Lerp(start_value, value_end, elapsed / duration);
                        currentSize.x = currentSize.y = currentShift;
                        mat.mainTextureScale = currentSize;
                    }
                    else
                    {
                        float currentAlpha = Mathf.Lerp(start_value, value_end, elapsed / duration);
                        currentColor.r = mat.color.r;
                        currentColor.g = mat.color.g;
                        currentColor.b = mat.color.b;
                        currentColor.a = currentAlpha;
                        mat.SetColor("_Color", currentColor);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var go in objs) go.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On; //Enable shadows only for trees
    }

    private IEnumerator RotationCoroutine(float duration)
    {
        //print("ruoto");
        ChangeRenderingMode(false);
        float delta = Time.deltaTime;
        Quaternion old_rotation = transform.rotation;
        Quaternion end_value = rotatedPlayer ? Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) : Quaternion.identity;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(old_rotation, end_value, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end_value;
        playerInput.enabled = true;
    }

    private IEnumerator FallingCoroutine(float duration, float value_end)
    {
        float delta = Time.deltaTime;
        float old_position = transform.position.y;
        Vector3 currentPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            currentPos.y = Mathf.Lerp(old_position, value_end, elapsed / duration);
            transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentPos.y = value_end;
        transform.position = currentPos;
    }

    private void ChangeRenderingMode(bool fade)
    {
        var temp = new GameObject[ffObjs.Length + sfObjs.Length];
        ffObjs.CopyTo(temp, 0);
        sfObjs.CopyTo(temp, ffObjs.Length);
        foreach (var go in temp)
        {
            Material[] mats = go.GetComponent<MeshRenderer>().materials;
            foreach (var mat in mats)
            {
                if (fade)
                {
                    MaterialExtensions.ToFadeMode(mat, true);
                }
                else
                {
                    MaterialExtensions.ToOpaqueMode(mat);
                }
            }
        }
    }

    public void StartFromSky()
    {
        //UNCOMMENT TO START FROM A SINGLE AREA 
        /*
        GameObject targetPoint = GameObject.FindGameObjectWithTag("TargetPoint");
        GameObject currentArea = GameObject.FindGameObjectWithTag("Area");
        if (currentArea.name != "A") transform.position = new Vector3(targetPoint.transform.position.x, 150f, targetPoint.transform.position.z);
        else transform.position = new Vector3(targetPoint.transform.position.x, 700f, targetPoint.transform.position.z);
        */

        ffObjs = GameObject.FindGameObjectsWithTag("FirstFloor");
        sfObjs = GameObject.FindGameObjectsWithTag("SecondFloor");

        terrainsToBeEnabled = GameObject.FindGameObjectWithTag("Terrain");
        ringObjs = GameObject.FindGameObjectsWithTag("Ring"); //Transparent objs with a specified alpha val
        glassObjs = GameObject.FindGameObjectsWithTag("Glass"); //Parents or glass shader objs
        findReportsObjs = GameObject.FindGameObjectsWithTag("FindReport"); //Reports objs

        additionalControls.sightPoints = GameObject.FindGameObjectsWithTag("SightPoint");

        foreach (var glass in glassObjs) glass.SetActive(false);
        foreach (var report in findReportsObjs) report.SetActive(false);

        treesObjs = GameObject.FindGameObjectsWithTag("Tree");
        logo.SetActive(true);

        if (terrainsToBeEnabled != null) terrainsToBeEnabled.SetActive(false);

        ChangeRenderingMode(true); //Solo a firstfloor and secondfloor e standard shader
        setInvisibleMat(ringObjs, false);
        setInvisibleMat(treesObjs, true);

        playerInput.enabled = false;
        controller.enabled = false;
        cc.enabled = false;

        controller.Grounded = false;
        controller.GroundLayers = LayerMask.GetMask("Default");
        ffObjsVisibile = false;
        sfObjsVisibile = false;
        terrainVisibile = false;
        rotated = false;
        rotatedPlayer = false;
        isLanding = false;
        terrainFlag = false;

        switch (transform.position.y)
        {
            case 700: //First time
                //additionalControls.cameraOverlayCanvas = startCanvas;
                StartCoroutine(FallingCoroutine(20f, 8f));
                break;
            case 10: //Every time in A except first time
                StartCoroutine(FallingCoroutine(1f, 8f));
                rotatedPlayer = true;
                break;
            default: //Every other area
                StartCoroutine(FallingCoroutine(8f, 8f));
                break;

        }
    }

    private void setInvisibleMat(GameObject[] objs, bool trees)
    {
        if (!trees)
        {
            foreach (var obj in objs)
            {
                Material[] mats = obj.GetComponent<MeshRenderer>().materials;
                foreach (var mat in mats)
                {
                    if (mat.shader.name == "Standard")
                    {
                        Color currentCol = mat.color;
                        mat.color = new Color(currentCol.r, currentCol.g, currentCol.b, 0);
                    }
                }

            }
        }
        else
        {
            foreach (var obj in objs)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                Material[] mats = mr.materials;
                foreach (var mat in mats)
                {
                    if (mat.shader.name == "Standard")
                    {
                        Color currentCol = mat.color;
                        mat.color = new Color(currentCol.r, currentCol.g, currentCol.b, 0);
                    }
                    else
                    {
                        mat.mainTextureScale = new Vector2(0, 0);
                    }
                }
            }
        }
    }
}
