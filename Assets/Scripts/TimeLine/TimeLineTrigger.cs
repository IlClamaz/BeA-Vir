using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineTrigger : MonoBehaviour
{
    private bool playerFlag;
    private TimeLineManager triggerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerFlag = false;
        triggerManager = GameObject.FindGameObjectWithTag("TriggerManager").GetComponent<TimeLineManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            triggerManager.triggerEntered(gameObject.name);
            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }
}
