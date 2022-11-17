using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private float m_timeOut;
    // Start is called before the first frame update
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    void OnEnable()
    {
        if (transform.parent.gameObject.activeSelf) Invoke("Deactivate", m_timeOut);
    }

    // Update is called once per frame
    private void Deactivate()
    {
        if (transform.parent.gameObject.activeSelf)
        {
            canvas.enabled = false;
            //print("ciao");
        }
    }

    private void ResetTitleUI()
    {
        CancelInvoke("Deactivate");
        canvas.enabled = true;
    }

    void OnDisable()
    {
        CancelInvoke("Deactivate");
        if (canvas.gameObject != null) canvas.enabled = true;
    }
}
