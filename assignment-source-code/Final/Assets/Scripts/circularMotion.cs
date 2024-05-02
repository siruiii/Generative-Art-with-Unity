using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularMovement : MonoBehaviour
{
    public float radius = 2f;
    public float speed = 1f;
    public float angle = 0f; // starting angle
    public AudioVisualization _audioVis;
    public bool useColor;
    public float y;
    public float scaleSpeed = 1f;

    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

    }
    private void Update()
    {

        // Set the start color
        if (useColor)
        {
            var mainModule = GetComponent<ParticleSystem>().main;
            mainModule.startColor = AudioReaction.colorIndex;
        }

        float x = Mathf.Cos(angle) * radius + 25f;
        float z = Mathf.Sin(angle) * radius + 25f;

        transform.position = new Vector3(x, y, z);

        if (!float.IsNaN(_audioVis._AmplitudeBuffer))
        {
            speed = Mathf.Lerp(1f, 5f, _audioVis._AmplitudeBuffer);
            radius = Mathf.Lerp(2f, 15f, _audioVis._AmplitudeBuffer);
        }
        else
        {
            speed = 1f; // Default speed if _audioVis._AmplitudeBuffer is NaN
        }

        angle += speed * Time.deltaTime;

        if (AudioReaction.end)
        {
            // Gradually scale down the object
            transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);

            // Ensure the scale doesn't go below zero
            transform.localScale = Vector3.Max(transform.localScale, Vector3.zero);
        }
    }
}
