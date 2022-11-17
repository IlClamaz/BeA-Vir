using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JizuBtn : MonoBehaviour
{
    private bool playerFlag;

    //[SerializeField] private CapsuleCollider reperti;
    //private CapsuleCollider cc;
    [SerializeField] private GameObject[] toDisappear;
    [SerializeField] private GameObject[] toAppear;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip btnClickClip;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var go in toAppear)
            if (!go.activeSelf) ChangeRenderingMode(go, true, true); // rende invisibili gli oggetti in toAppear del bottone entering
        playerFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (audioSource != null) audioSource.PlayOneShot(btnClickClip);
            foreach (var go in toAppear)
            {
                go.SetActive(true);
                StartCoroutine(FadingCoroutine(go, 0, 1, 0.2f));
            }

            foreach (var go in toDisappear)
            {
                StartCoroutine(FadingCoroutine(go, 1, 0, 0.2f));
            }

            //cc.enabled = false; //disabilita il proprio collider
            //reperti.enabled = true; //abilita il collider del tasto reperti
            playerFlag = false;
        }
    }

    private IEnumerator FadingCoroutine(GameObject subject, float start_value, float value_end, float duration)
    {
        ChangeRenderingMode(subject, true, false);
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

        if (value_end == 1) ChangeRenderingMode(subject, false, false); //rendo opaco l'oggetto che sta apparendo
        else subject.SetActive(false); // disattivo l'oggetto che sta sparendo
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    private void ChangeRenderingMode(GameObject subject, bool fade, bool start)
    {
        Material[] mats = subject.GetComponent<MeshRenderer>().materials;
        foreach (var mat in mats)
        {
            if (fade) MaterialExtensions.ToFadeMode(mat, start);
            else MaterialExtensions.ToOpaqueMode(mat);
        }
    }
}
