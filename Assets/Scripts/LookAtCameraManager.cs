using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraManager : MonoBehaviour
{
    [SerializeField] private Transform main_camera;
    [SerializeField] private GameObject[] doors;

    // Use this for initialization
    void Start()
    {
        //doors = GameObject.FindGameObjectsWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        foreach (var door in doors)
        {
            Vector3 difference = main_camera.position - door.transform.position;
            float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
            door.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
        }
    }
}
