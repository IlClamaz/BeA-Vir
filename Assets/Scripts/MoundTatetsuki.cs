using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoundTatetsuki : MonoBehaviour
{
    private bool playerFlag;
    private bool cooldown;
    private int switchBtn;
    private Vector3 oldScale;
    private Vector3 oldPosition;
    [SerializeField] private GameObject mound;
    [SerializeField] private float positionDelta;
    [SerializeField] private float scaleDelta;
    [SerializeField] private float animDuration;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip btnClickClip;
    // Start is called before the first frame update
    void Start()
    {
        oldScale = mound.transform.localScale;
        oldPosition = mound.transform.position;
        playerFlag = false;
        cooldown = false;
        switchBtn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (audioSource != null) audioSource.PlayOneShot(btnClickClip);
            playerFlag = false;
            if (switchBtn == 1)
            {
                mound.SetActive(true);
                StartCoroutine(PositionCoro(animDuration, mound.transform.position.y + positionDelta));
            }
            else StartCoroutine(ScaleCoro(animDuration, mound.transform.localScale.y - scaleDelta));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !cooldown)
        {
            playerFlag = true;
            cooldown = true;
            Invoke("ResetCooldown", animDuration + 1f);
            if (switchBtn == 0) switchBtn = 1;
            else switchBtn = 0;
        }
    }

    private IEnumerator PositionCoro(float duration, float end_value)
    {
        float elapsed = 0.0f;
        float oldPos = mound.transform.position.y;
        Vector3 currentPos = new Vector3();
        currentPos.x = mound.transform.position.x;
        currentPos.z = mound.transform.position.z;
        while (elapsed < duration)
        {
            currentPos.y = Mathf.Lerp(oldPos, end_value, elapsed / duration);
            mound.transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentPos.y = end_value;
        mound.transform.position = currentPos;

        if (switchBtn == 1) StartCoroutine(ScaleCoro(1f, mound.transform.localScale.y + scaleDelta)); //continuo animazione
        else mound.SetActive(false);
    }

    private IEnumerator ScaleCoro(float duration, float end_value)
    {
        float elapsed = 0.0f;
        float oldSize = mound.transform.localScale.x;
        Vector3 currentSize = new Vector3();
        while (elapsed < duration)
        {
            currentSize.x = currentSize.y = currentSize.z = Mathf.Lerp(oldSize, end_value, elapsed / duration);
            mound.transform.localScale = currentSize;
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentSize.x = currentSize.y = currentSize.z = end_value;
        mound.transform.localScale = currentSize;

        if (switchBtn == 0) StartCoroutine(PositionCoro(1f, mound.transform.position.y - positionDelta)); //continuo animazione
    }

    void ResetCooldown()
    {
        cooldown = false;
    }

    private void Reset()
    {
        mound.transform.position = oldPosition;
        mound.transform.localScale = oldScale;
        switchBtn = 0;
        mound.SetActive(false);
    }
}
