using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinamikataSwap : MonoBehaviour
{
    private bool playerFlag;
    private bool cooldown;
    [SerializeField] private GameObject mound;
    [SerializeField] private GameObject city;
    [SerializeField] private float positionDelta;
    [SerializeField] private float swapDuration;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip btnClickClip;
    [SerializeField] private GameObject titleNow;
    [SerializeField] private GameObject titleArch;
    [SerializeField] private AdditionalControls additionalControls;
    private Vector3 oldMoundPosition;
    private Vector3 oldCityPosition;
    private Canvas canvasTitleNow;
    private Canvas canvasTitleArch;
    // Start is called before the first frame update
    void Start()
    {
        canvasTitleNow = titleNow.GetComponent<Canvas>();
        canvasTitleArch = titleArch.GetComponent<Canvas>();
        oldMoundPosition = mound.transform.position;
        oldCityPosition = city.transform.position;
        playerFlag = false;
        cooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (audioSource != null) audioSource.PlayOneShot(btnClickClip);
            playerFlag = false;
            if (city.activeSelf)
            {
                StartCoroutine(PositionCoro(city, swapDuration, city.transform.position.y - positionDelta, result =>
                {
                    city.SetActive(false);
                    mound.SetActive(true);
                    StartCoroutine(PositionCoro(mound, swapDuration, mound.transform.position.y + positionDelta, result =>
                    {
                        additionalControls.CloseOverlay();
                        titleNow.SetActive(false);
                        titleArch.SetActive(true);
                        canvasTitleArch.enabled = true;
                    }));
                }));
            }
            else
            {
                StartCoroutine(PositionCoro(mound, swapDuration, mound.transform.position.y - positionDelta, result =>
                {
                    city.SetActive(true);
                    mound.SetActive(false);
                    StartCoroutine(PositionCoro(city, swapDuration, city.transform.position.y + positionDelta, result =>
                    {
                        additionalControls.CloseOverlay();
                        titleNow.SetActive(true);
                        canvasTitleNow.enabled = true;
                        titleArch.SetActive(false);
                    }));
                }));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !cooldown)
        {
            playerFlag = true;
            cooldown = true;
            Invoke("ResetCooldown", swapDuration + 1f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        playerFlag = false;
    }

    private IEnumerator PositionCoro(GameObject subject, float duration, float end_value, System.Action<int> callback)
    {
        float elapsed = 0.0f;
        float oldPos = subject.transform.position.y;
        Vector3 currentPos = new Vector3();
        currentPos.x = subject.transform.position.x;
        currentPos.z = subject.transform.position.z;
        while (elapsed < duration)
        {
            currentPos.y = Mathf.Lerp(oldPos, end_value, elapsed / duration);
            subject.transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentPos.y = end_value;
        subject.transform.position = currentPos;
        callback(1);
    }

    void ResetCooldown()
    {
        cooldown = false;
    }

    private void Reset()
    {
        city.transform.position = oldCityPosition;
        mound.transform.position = oldMoundPosition;
        city.SetActive(true);
        mound.SetActive(false);
        titleNow.SetActive(false);
        canvasTitleNow.enabled = true;
        titleArch.SetActive(false);
        canvasTitleArch.enabled = true;
    }

    private void MinaReset()
    {
        titleNow.SetActive(true);
        canvasTitleNow.enabled = true;
        titleArch.SetActive(false);
        canvasTitleArch.enabled = true;
    }
}

