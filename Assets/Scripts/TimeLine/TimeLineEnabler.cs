using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineEnabler : MonoBehaviour
{
    private bool playerFlag;
    [SerializeField] private GameObject timeLine;
    [SerializeField] private GameObject timeLineWall;

    void OnEnable()
    {
        playerFlag = false;
        timeLine.SetActive(false);
        timeLineWall.transform.localPosition = new Vector3(timeLineWall.transform.localPosition.x, 3.3f, timeLineWall.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (!timeLine.activeSelf)
            {
                timeLine.SetActive(true);
                StartCoroutine(PositionCoro(0.05f, 7.22f));
            }
            else
            {
                StartCoroutine(PositionCoro(0.05f, 3.3f));
            }
            playerFlag = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") playerFlag = true;
    }

    private IEnumerator PositionCoro(float duration, float end_value)
    {
        float elapsed = 0.0f;
        float oldPos = timeLineWall.transform.localPosition.y;
        Vector3 currentPos = new Vector3();
        currentPos.x = timeLineWall.transform.localPosition.x;
        currentPos.z = timeLineWall.transform.localPosition.z;
        while (elapsed < duration)
        {
            currentPos.y = Mathf.Lerp(oldPos, end_value, elapsed / duration);
            timeLineWall.transform.localPosition = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentPos.y = end_value;
        timeLineWall.transform.localPosition = currentPos;
        if (end_value == 3.3f) timeLine.SetActive(false);
    }
}
