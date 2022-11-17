using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDisappear : MonoBehaviour
{
    private bool playerFlag;
    [SerializeField] private GameObject[] toDisappear;
    [SerializeField] private GameObject[] toAppear;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip btnClickClip;
    // Start is called before the first frame update
    void Start()
    {
        playerFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (audioSource != null) audioSource.PlayOneShot(btnClickClip);
            foreach (var go in toDisappear)
            {
                if (go.activeSelf) go.SetActive(false);
            }

            foreach (var go in toAppear)
            {
                if (!go.activeSelf) go.SetActive(true);
            }

            playerFlag = false;
        }
    }

    private IEnumerator FadingCoroutine(GameObject subject, float start_value, float value_end, float duration)
    {
        ChangeRenderingMode(subject, true);
        float elapsed = 0.0f;
        Color currentColor = new Color();
        while (elapsed < duration)
        {
            Material[] mats = subject.GetComponent<MeshRenderer>().materials;
            foreach (var mat in mats)
            {
                float currentAlpha = Mathf.Lerp(start_value, value_end, elapsed / duration);
                currentColor.r = mat.color.r;
                currentColor.g = mat.color.g;
                currentColor.b = mat.color.b;
                currentColor.a = currentAlpha;
                mat.SetColor("_Color", currentColor);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Material[] materials = subject.GetComponent<MeshRenderer>().materials;
        foreach (var mat in materials)
        {
            currentColor.r = mat.color.r;
            currentColor.g = mat.color.g;
            currentColor.b = mat.color.b;
            currentColor.a = value_end;
            mat.SetColor("_Color", currentColor);
        }

        if (value_end == 1) ChangeRenderingMode(subject, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    private void ChangeRenderingMode(GameObject subject, bool fade)
    {
        Material[] mats = subject.GetComponent<MeshRenderer>().materials;
        foreach (var mat in mats)
        {
            if (fade) MaterialExtensions.ToFadeMode(mat, false);
            else MaterialExtensions.ToOpaqueMode(mat);
        }
    }
}
