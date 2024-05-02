using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public PostProcessVolume postProcessingVolume; 
    [Range(1f, 20f)]
    public float bloomIntensity = 1f; 
    Vector2 _intensityMinMax;
    private Bloom bloomLayer;

    public AudioVisualization _audioVis;

    void Start()
    {
       
        bloomLayer = postProcessingVolume.profile.GetSetting<Bloom>();

        _intensityMinMax = new Vector2(1f, 10f);
    }

    void Update()
    {
        
        if (bloomLayer != null)
        {
            //bloomLayer.intensity.value = bloomIntensity;
            bloomLayer.intensity.value = Mathf.Lerp(_intensityMinMax.x, _intensityMinMax.y, _audioVis._AmplitudeBuffer);
        }

    }
}

