using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using StarterAssets;
using UnityEngine.InputSystem.Utilities;
using System;
using UnityEngine.SceneManagement;


public class AdditionalControls : MonoBehaviour
{
    private FirstPersonController ctrlScript;
    private CharacterController characterController;
    private StarterAssetsInputs inputs;
    private TravelManager travelManager;
    private GameObject currentArea;
    [SerializeField] private InputActionReference cameraOverlay;
    [SerializeField] private InputActionReference goBackToHome;
    [SerializeField] private InputActionReference prevSightSeeing;
    [SerializeField] private InputActionReference nextSightSeeing;
    [SerializeField] private InputActionReference resetGameLX;
    [SerializeField] private InputActionReference resetGameRX;
    [SerializeField] private GameObject screenSaverOverlayCanvas;
    [SerializeField] private int IdleTimeSetting;
    public GameObject cameraOverlayCanvas;
    public GameObject[] sightPoints;
    private bool sightSeeingMode;
    private int sightIndex;
    private Vector2 zero;
    private bool cooldown;
    private float LastIdleTime;
    private IDisposable buttonPressListener;
    private Canvas currentCanvasTitle;
    private GameObject canvasTitle;

    void Awake()
    {

        LastIdleTime = Time.time;
        buttonPressListener = InputSystem.onAnyButtonPress.Call(button =>
        {
            //print(button);
            LastIdleTime = Time.time;
        });
    }

    void Start()
    {
        cooldown = false;
        zero = Vector2.zero;
        travelManager = GetComponent<TravelManager>();
        ctrlScript = GetComponent<FirstPersonController>();
        characterController = GetComponent<CharacterController>();
        inputs = GetComponent<StarterAssetsInputs>();
        sightSeeingMode = false;
        sightIndex = 0;

        cameraOverlay.action.performed += CameraOverlay;
        resetGameLX.action.performed += ResetGame;
        resetGameRX.action.performed += ResetGame;
        goBackToHome.action.performed += GoBackToHome;
        prevSightSeeing.action.performed += StartSightSeeing;
        nextSightSeeing.action.performed += StartSightSeeing;

        //Invoke("StartScreenSaver", 10f);
        //InputSystem.onAnyButtonPress.Call(button => StopScreenSaver());
    }

    void Update()
    {
        Fly();
        if (inputs.move != zero) sightSeeingMode = false;
        if (inputs.move != zero || inputs.look != zero) LastIdleTime = Time.time;
        if (IdleCheck())
        {
            if (!screenSaverOverlayCanvas.activeSelf) screenSaverOverlayCanvas.SetActive(true);
        }
        else
        {
            if (screenSaverOverlayCanvas.activeSelf)
            {
                screenSaverOverlayCanvas.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //if (inputs.move != zero || inputs.look != zero) StopScreenSaver();
    }

    private void Fly()
    {
        if (inputs.jump)
        {
            ctrlScript._verticalVelocity = Mathf.Sqrt(ctrlScript.JumpHeight * -15f * ctrlScript.Gravity);
            ctrlScript.Gravity = -1;
        }
    }

    private void CameraOverlay(InputAction.CallbackContext ctx)
    {
        canvasTitle = GameObject.FindGameObjectWithTag("UITitle");
        if (canvasTitle != null) currentCanvasTitle = canvasTitle.GetComponent<Canvas>();
        if (canvasTitle == null || !currentCanvasTitle.enabled)
        {
            if (cameraOverlay.action.ReadValue<float>() == 1)
            {
                if (!cameraOverlayCanvas.activeSelf)
                {
                    cameraOverlayCanvas.SetActive(true);
                    Invoke("CloseOverlay", 50f);
                }
                else
                {
                    cameraOverlayCanvas.SetActive(false);
                    if (!cooldown) CancelInvoke("CloseOverlay");
                }
            }
        }

        /* if (cameraCredits.action.ReadValue<float>() == 1)
        {
            if (!creditsOverlayCanvas.activeSelf) creditsOverlayCanvas.SetActive(true);
            else creditsOverlayCanvas.SetActive(false);
        } */
    }

    private void GoBackToHome(InputAction.CallbackContext ctx)
    {
        if (goBackToHome.action.ReadValue<float>() == 1 && !cooldown)
        {
            cooldown = true;
            Invoke("CooldownReset", 2f);
            StartCoroutine(travelManager.FTBCoroutine(1, 1f, travelManager.blackImage, false));
        }
    }

    private void ResetGame(InputAction.CallbackContext ctx)
    {
        if (resetGameLX.action.ReadValue<float>() > 0 && resetGameRX.action.ReadValue<float>() > 0 && !cooldown)
        {
            cooldown = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void StartSightSeeing(InputAction.CallbackContext ctx)
    {
        //print(sightIndex + " " + sightPoints.Length + " " + sightSeeingMode);
        if (sightPoints != null && sightPoints.Length > 0 && !cooldown)
        {
            cooldown = true;
            Invoke("CooldownReset", 1f);
            ctrlScript.Gravity = -15;
            if ((prevSightSeeing.action.ReadValue<float>() > 0 || nextSightSeeing.action.ReadValue<float>() > 0) && !sightSeeingMode)
            {
                sightIndex = 0;
                travelManager.sightPoint = sightPoints[sightIndex];
                StartCoroutine(travelManager.FTBCoroutine(1, 0.3f, travelManager.blackImage, true));
                sightSeeingMode = true;
                //print("START " + sightIndex + " " + sightPoints.Length + " " + sightSeeingMode);
            }
            else if (prevSightSeeing.action.ReadValue<float>() > 0 && sightSeeingMode)
            {
                if (sightIndex == 0) sightIndex = sightPoints.Length;
                sightIndex--;
                travelManager.sightPoint = sightPoints[sightIndex];
                StartCoroutine(travelManager.FTBCoroutine(1, 0.3f, travelManager.blackImage, true));
                //print("PREV " + sightIndex + " " + sightPoints.Length + " " + sightSeeingMode);
            }
            else if (nextSightSeeing.action.ReadValue<float>() > 0 && sightSeeingMode)
            {
                sightIndex++;
                if (sightIndex == sightPoints.Length) sightIndex = 0;
                travelManager.sightPoint = sightPoints[sightIndex];
                StartCoroutine(travelManager.FTBCoroutine(1, 0.3f, travelManager.blackImage, true));
                //print("NEXT " + sightIndex + " " + sightPoints.Length + " " + sightSeeingMode);
            }
        }
        else
        {
            print("No sightspoint");
        }

    }

    public void CloseOverlay()
    {
        cameraOverlayCanvas.SetActive(false);
    }

    private void CooldownReset()
    {
        cooldown = false;
    }

    private bool IdleCheck()
    {
        return Time.time - LastIdleTime > IdleTimeSetting;
    }

    void OnDestroy()
    {
        buttonPressListener.Dispose();
    }

    /*     private void StartScreenSaver()
        {
            if (!screenSaverOverlayCanvas.activeSelf)
                screenSaverOverlayCanvas.SetActive(true);

        }

        private void StopScreenSaver()
        {
            print("STOP");
            if (screenSaverOverlayCanvas.activeSelf) screenSaverOverlayCanvas.SetActive(false);
        } */

}