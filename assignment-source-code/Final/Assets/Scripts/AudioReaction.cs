//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReaction : MonoBehaviour
{
    public AudioVisualization _audioVis;
    private float[] _prevFreqBandHighest = new float[8];
    public float _freqSum;

    public MouseAttractor _mouseAttractor;
    public GameObject prefabToInstantiate; 

    public Color[] rainbowColors = new Color[8];

    private float currentRadius = 1f;
    private float angleStep = 10f;
    private float currentAngle = 0f;

    public int count = 0;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public GameObject field;
    public GameObject system;
    public static Color colorIndex;
    public AudioSource audioSource;

    public float scaleSpeed = 0.0001f;
    bool flag = false;
    public static bool end = false;

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
        // if highest is changed or close to highest
        for (int i = 0; i < 8; i++)
        {
            if (_audioVis._freqBandHighest[i] > _prevFreqBandHighest[i] || Mathf.Abs(_audioVis._freqBand[i] - _audioVis._freqBandHighest[i]) < 1f)
            {
            
                float x = currentRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad)+25f+Random.Range(0f,5f);
                float y = currentRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad)+25f+ Random.Range(0f, 5f);
                Vector3 newPosition = new Vector3(x, y, 25f);

                
                currentAngle += angleStep;
                //currentRadius += 0.05f;

                if (currentAngle > 360f)
                {
                    currentRadius += 1f;
                    currentAngle = 0f;
                }

                //instaniate new
                GameObject newObj = Instantiate(prefabToInstantiate, newPosition, Quaternion.identity);
                newObj.GetComponent<MeshRenderer>().material.SetColor("_Color", rainbowColors[i]);
                newObj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", rainbowColors[i]);
                newObj.name = "band"+Random.Range(0,7);

                instantiatedObjects.Add(newObj);
                count++;

                // add the new to the list
                _mouseAttractor.originalPositions.Add(newObj.transform.position);
            }
            // update highest
            _prevFreqBandHighest[i] = _audioVis._freqBandHighest[i];

        }
        // current time stamp
        float currentTime = Time.time;
        float audioLength = audioSource.clip.length;
        float remainingTime = audioLength - currentTime;


            foreach (GameObject obj in instantiatedObjects)
            {
                int objIndex = int.Parse(obj.name.Replace("band", ""));
                if (objIndex == count % 8)
                {
                    colorIndex = rainbowColors[objIndex];
                    obj.GetComponent<MeshRenderer>().enabled = true;
                    obj.GetComponent<MeshRenderer>().material.SetColor("_Color", rainbowColors[objIndex]);
                    obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", rainbowColors[objIndex]);
                    //float scale = Mathf.Lerp(0.1f, 1f, _audioVis._audioBandBuffer[objIndex]);
                    //obj.transform.localScale= new Vector3(scale, scale, scale);
                }
                else if (flag)
                {
                    //obj.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                    //obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
                    //obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }

                if (remainingTime < 6)
                {
                    obj.transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
                    obj.transform.localScale = Vector3.Max(obj.transform.localScale, Vector3.zero);

                    system.transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
                    system.transform.localScale = Vector3.Max(system.transform.localScale, Vector3.zero);

                end = true;
                }
            }

        _freqSum = 0;
         foreach (float i in _audioVis._freqBandHighest)
        {
            _freqSum += i;
        }
        if (_freqSum>=101f)
        {
            field.SetActive(true);
        }
        if (_freqSum >= 40f)
        {
            flag = true;
            system.SetActive(true);
        }
    }
}