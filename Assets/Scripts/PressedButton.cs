using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressedButton : MonoBehaviour
{
    [SerializeField] GameObject pressedButton;
    [SerializeField] GameObject target;
    [SerializeField] float animationDuration;
    [SerializeField] float deltaScale;
    [SerializeField] bool oneTime;
    private MeshRenderer meshRenderer;
    private bool playerFlag;
    private bool cooldown;
    private bool oneTimePressed;
    private Vector3 oldScale;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        oldScale = transform.localScale;
        playerFlag = false;
        cooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (!oneTime)
            {
                if (pressedButton.activeSelf)
                {
                    StartCoroutine(ScaleCoro(pressedButton, animationDuration, pressedButton.transform.localScale.z + deltaScale, result =>
                    {
                        StartCoroutine(ScaleCoro(pressedButton, animationDuration, pressedButton.transform.localScale.z - deltaScale, result =>
                        {
                            pressedButton.SetActive(false);
                            target.SetActive(true);
                            meshRenderer.enabled = true;
                        }));
                    }));
                }
                else
                {
                    StartCoroutine(ScaleCoro(gameObject, animationDuration, transform.localScale.z + deltaScale, result =>
                    {
                        StartCoroutine(ScaleCoro(gameObject, animationDuration, transform.localScale.z - deltaScale, result =>
                        {
                            pressedButton.SetActive(true);
                            target.SetActive(false);
                            meshRenderer.enabled = false;
                        }));
                    }));
                }
            }
            else
            {
                StartCoroutine(ScaleCoro(gameObject, animationDuration, transform.localScale.z + deltaScale, result =>
                    {
                        StartCoroutine(ScaleCoro(gameObject, animationDuration, transform.localScale.z - deltaScale, result =>
                        {
                            pressedButton.SetActive(true);
                            target.SetActive(false);
                            meshRenderer.enabled = false;
                        }));
                    }));
                oneTimePressed = true;
            }

            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !cooldown && !oneTimePressed)
        {
            playerFlag = true;
            cooldown = true;
            Invoke("ResetCooldown", animationDuration + 1f);
        }
    }

    private IEnumerator ScaleCoro(GameObject subject, float duration, float end_value, System.Action<int> callback)
    {
        float elapsed = 0.0f;
        float oldSize = subject.transform.localScale.z;

        Vector3 currentSize = new Vector3();
        currentSize.x = subject.transform.localScale.x;
        currentSize.y = subject.transform.localScale.y;
        while (elapsed < duration)
        {
            currentSize.z = Mathf.Lerp(oldSize, end_value, elapsed / duration);
            subject.transform.localScale = currentSize;
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentSize.z = end_value;
        subject.transform.localScale = currentSize;
        callback(1);
    }

    private void Reset()
    {
        pressedButton.SetActive(false);
        target.SetActive(true);
        meshRenderer.enabled = true;
        transform.localScale = oldScale;
        pressedButton.transform.localScale = oldScale;
    }

    private void ResetCooldown()
    {
        cooldown = false;
    }
}
