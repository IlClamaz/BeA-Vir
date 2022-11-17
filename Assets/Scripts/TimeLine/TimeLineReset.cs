using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineReset : MonoBehaviour
{
    private bool playerFlag;
    [SerializeField] private GameObject[] toAppear;
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
            foreach (var go in toAppear)
            {
                if (!go.activeSelf) go.SetActive(true);
            }

            playerFlag = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }
}
