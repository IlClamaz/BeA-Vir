using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindReport : MonoBehaviour
{
    private bool playerFlag;
    private bool playerClose;
    [SerializeField] public GameObject schedaReperto;
    [SerializeField] private GameObject reperto;
    [SerializeField] private bool still;
    private WaitForSeconds waitForSeconds;
    private BrownianMovement piattaforma;
    private Vector3 repertoPosition;
    private Vector3 repertoScale;
    private Quaternion repertoRotation;
    private float scaleDelta;
    private float rotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        if (reperto != null)
        {
            repertoPosition = reperto.transform.localPosition;
            repertoScale = reperto.transform.localScale;
            repertoRotation = reperto.transform.rotation;
            //scaleDelta = 1.2f;
            rotationVelocity = 10f;
            gameObject.transform.parent.gameObject.TryGetComponent(out piattaforma);
        }
        playerFlag = false;
        playerClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (!schedaReperto.activeSelf)
            {
                schedaReperto.SetActive(true);
                playerClose = true;
                if (!still) StartCoroutine(RotateCoru());
                if (piattaforma != null) piattaforma.enabled = false;
            }
            else
            {
                Reset();
            }
            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //print("DBENTER");
        if (other.tag == "Player") playerFlag = true;
    }

    void OnTriggerExit(Collider other)
    {
        //print("DBEXIT");
        if (other.tag == "Player") playerFlag = true;
    }

    private IEnumerator RotateCoru()
    {
        if (reperto != null)
        {
            while (playerClose)
            {
                reperto.transform.RotateAround(reperto.transform.position, Vector3.up, Time.deltaTime * rotationVelocity); //piatto rotante
                reperto.transform.localPosition = repertoPosition;
                yield return null;
            }
        }
    }

    private void Reset()
    {
        //print("dbreset");
        schedaReperto.SetActive(false);
        playerClose = false;
        if (!still) StopCoroutine(RotateCoru());
        if (reperto != null)
        {
            reperto.transform.localPosition = repertoPosition;
            //reperto.transform.localScale = repertoScale;
            reperto.transform.rotation = repertoRotation;
        }
        if (piattaforma != null) piattaforma.enabled = true;
    }

    private void ResetTrincee()
    {
        if (schedaReperto.name == "TumuloPerZonaTrincee") schedaReperto.SetActive(true);
    }
}
