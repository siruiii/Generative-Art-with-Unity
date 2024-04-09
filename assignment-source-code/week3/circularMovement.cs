using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularMovement : MonoBehaviour
{
    public float radius = 2f;
    public float speed = 1f;
    private float angle = 0f; // starting angle

    private void Update()
    {
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, 0f);

        // update angle
        angle += speed * Time.deltaTime;
    }
}
