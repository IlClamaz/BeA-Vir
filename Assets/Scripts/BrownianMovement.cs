using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownianMovement : MonoBehaviour
{
    [SerializeField] private float move_delta_x;
    [SerializeField] private float move_delta_y;
    [SerializeField] private float move_delta_z;
    [SerializeField] private float move_duration;
    private bool directionUp = true;
    private bool called = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!called)
        {
            //print("ciao");
            move_delta_x = move_delta_x + Random.Range(-0.01f, 0.01f);
            move_delta_y = move_delta_y + Random.Range(-0.01f, 0.01f);
            move_delta_z = move_delta_z + Random.Range(-0.01f, 0.01f);
            StartCoroutine(BrownianCoroutine());
            called = true;
        }
    }

    private IEnumerator BrownianCoroutine()
    {
        float end_value_x = 0.0f;
        float end_value_y = 0.0f;
        float end_value_z = 0.0f;

        Vector3 currentPos = transform.position;
        float old_val_x = currentPos.x;
        float old_val_y = currentPos.y;
        float old_val_z = currentPos.z;

        if (directionUp)
        {
            end_value_x = old_val_x + move_delta_x;
            end_value_y = old_val_y + move_delta_y;
            end_value_z = old_val_z + move_delta_z;
        }
        else
        {
            end_value_x = old_val_x - move_delta_x;
            end_value_y = old_val_y - move_delta_y;
            end_value_z = old_val_z - move_delta_z;
        }

        float elapsed = 0.0f;
        while (elapsed < move_duration)
        {
            currentPos.x = Mathf.Lerp(old_val_x, end_value_x, elapsed / move_duration);
            currentPos.y = Mathf.Lerp(old_val_y, end_value_y, elapsed / move_duration);
            currentPos.z = Mathf.Lerp(old_val_z, end_value_z, elapsed / move_duration);
            transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentPos.x = end_value_x;
        currentPos.y = end_value_y;
        currentPos.z = end_value_z;
        transform.position = currentPos;
        directionUp = !directionUp;
        called = false;
    }
}
