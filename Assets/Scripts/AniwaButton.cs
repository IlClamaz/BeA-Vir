using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AniwaButton : MonoBehaviour
{
    private bool playerFlag;
    private int switchBtn;
    [SerializeField] private GameObject aniwaPanel;
    [SerializeField] private float flipDuration;
    [SerializeField] private float positionDelta;
    private bool rotated;
    // Start is called before the first frame update
    void Start()
    {
        playerFlag = false;
        rotated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFlag)
        {
            if (!rotated)
            {
                StartCoroutine(PositionCoro(flipDuration, aniwaPanel.transform.position.y + positionDelta, result =>
                {
                    StartCoroutine(RotationCoro(flipDuration, -90, result => { }));
                    rotated = true;
                }));
            }
            else
            {
                StartCoroutine(RotationCoro(flipDuration, 0, result =>
                {
                    StartCoroutine(PositionCoro(flipDuration, aniwaPanel.transform.position.y - positionDelta, result => { }));
                    rotated = false;
                }));
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
        playerFlag = false;
    }

    private IEnumerator PositionCoro(float duration, float end_value, Action<int> callback)
    {
        float elapsed = 0.0f;
        float oldPos = aniwaPanel.transform.position.y;
        Vector3 currentPos = new Vector3();
        currentPos.x = aniwaPanel.transform.position.x;
        currentPos.z = aniwaPanel.transform.position.z;
        while (elapsed < duration)
        {
            currentPos.y = Mathf.Lerp(oldPos, end_value, elapsed / duration);
            aniwaPanel.transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentPos.y = end_value;
        aniwaPanel.transform.position = currentPos;
        callback(1);
    }

    private IEnumerator RotationCoro(float duration, float end_value, Action<int> callback)
    {
        float delta = Time.deltaTime;
        Quaternion old_rotation = aniwaPanel.transform.rotation;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            aniwaPanel.transform.rotation = Quaternion.Slerp(old_rotation, Quaternion.Euler(end_value, 0, 0), elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        aniwaPanel.transform.rotation = Quaternion.Euler(end_value, 0, 0);
        callback(1);
    }
}
