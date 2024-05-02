using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScale : MonoBehaviour
{
    public AudioVisualization _audioVis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!float.IsNaN(_audioVis._AmplitudeBuffer))
        {
            transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(10f, 10f, 10f), _audioVis._AmplitudeBuffer);
            Debug.Log(transform.localScale);
        }
    }
}
