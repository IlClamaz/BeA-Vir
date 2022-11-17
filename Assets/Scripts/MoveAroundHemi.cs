using UnityEngine;

public class MoveAroundHemi : MonoBehaviour
{
    [SerializeField] private Transform main_camera;
    [SerializeField] private FadingTrigger ft;
    [SerializeField] private float verticalClampDelta;
    [SerializeField] private float distanceFromCamera;
    private float startPosition;

    void Start()
    {
        startPosition = transform.position.y;
    }

    void Update()
    {
        if (ft.rotated) //Only if player has landed
        {
            //IN ORDER TO FACE ALWAYS CAMERA ON THE RIGHT SIDE
            Vector3 difference = main_camera.position - transform.position;
            float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

            //IN ORDER TO FOLLOW ALWAYS CAMERA ON A LIMITED EMISPHERE
            if (-0.6f <= main_camera.transform.forward.y && main_camera.transform.forward.y <= 0.6f)
            {
                Vector3 positionNew = main_camera.transform.position + main_camera.transform.forward * distanceFromCamera;
                positionNew.y = Mathf.Clamp(positionNew.y, startPosition - verticalClampDelta, startPosition + verticalClampDelta);
                transform.position = positionNew;
            }
        }
    }

}