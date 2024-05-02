using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class test : MonoBehaviour
{
    public AudioVisualization _audioVis;
    private float[] _prevFreqBandHighest = new float[8];
    public float _freqSum;

    public MouseAttractor _mouseAttractor;
    public LineRenderer lineRendererPrefab; // Assign your LineRenderer prefab in the Inspector

    public Color[] rainbowColors = new Color[8];

    private float currentRadius = 0.1f;
    private float angleStep = 10f;
    private float currentAngle = 0f;

    private int count = 0;
    private List<LineRenderer> instantiatedLines = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        rainbowColors[0] = Color.red;
        rainbowColors[1] = new Color(1.0f, 0.5f, 0.0f); // Orange
        rainbowColors[2] = Color.yellow;
        rainbowColors[3] = Color.green;
        rainbowColors[4] = Color.cyan;
        rainbowColors[5] = Color.blue;
        rainbowColors[6] = new Color(0.5f, 0.0f, 1.0f); // Indigo
        rainbowColors[7] = new Color(0.7f, 0.0f, 0.7f); // Violet
    }

    // Update is called once per frame
    void Update()
    {
        // Check for changes in the highest frequency band
        for (int i = 0; i < 8; i++)
        {
            if (_audioVis._freqBandHighest[i] > _prevFreqBandHighest[i] || Mathf.Abs(_audioVis._freqBand[i] - _audioVis._freqBandHighest[i]) < 0.5f)
            {
                // Calculate position using polar coordinates
                float x = currentRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad) + 25f;
                float y = currentRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad) + 25f;
                Vector3 newPosition = new Vector3(x, y, 25f);

                // Update angle for next line
                currentAngle += angleStep;
                currentRadius += 0.05f;

                // Instantiate a new LineRenderer
                LineRenderer newLine = Instantiate(lineRendererPrefab, newPosition, Quaternion.identity);
                newLine.positionCount = 2; // Set the number of vertices to 2 for a line
                newLine.SetPosition(0, newPosition);
                newLine.SetPosition(1, newPosition); // Set both positions initially to the same point

                instantiatedLines.Add(newLine);
                count++;

                // Add the new line's position to the list
                _mouseAttractor.originalPositions.Add(newPosition);
            }
            // Update previous highest frequency band values
            _prevFreqBandHighest[i] = _audioVis._freqBandHighest[i];

        }

        // Set colors of instantiated lines based on frequency band index
        foreach (LineRenderer line in instantiatedLines)
        {
            int lineIndex = count % 8;
            line.material.color = rainbowColors[lineIndex];
        }

        _freqSum = 0;
        foreach (float i in _audioVis._freqBandHighest)
        {
            _freqSum += i;
        }
    }
}
