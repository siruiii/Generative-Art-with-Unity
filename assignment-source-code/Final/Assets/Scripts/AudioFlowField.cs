using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]
public class NewBehaviourScript : MonoBehaviour
{
    NoiseFlowField _noiseFlowField;
    public AudioVisualization _audioVis;
    [Header("Speed")]
    public bool _useSpeed;
    public Vector2 _moveSpeedMinMax, _rotateSpeedMinMax;
    [Header("Scale")]
    public bool _useScale;
    public Vector2 _scaleMinMax;
    [Header("Material")]
    public Material _material;
    private Material[] _audioMaterial;
    public bool _useColor1;
    public string _colorName1;
    public Gradient _gradient1;
    private Color[] _color1;
    [Range(0f, 1f)]
    public float _colorThreshold1;
    public float _colorMultiplier1;
    public bool _useColor2;
    public string _colorName2;
    public Gradient _gradient2;
    private Color[] _color2;
    [Range(0f, 1f)]
    public float _colorThreshold2;
    public float _colorMultiplier2;

    // Start is called before the first frame update
    void Start()
    {
        _noiseFlowField = GetComponent<NoiseFlowField>();
        _audioMaterial = new Material[8];
        _color1 = new Color[8];
        _color2 = new Color[8];
        for (int i = 0; i < 8; i++)
        {
            _color1[i] = (_gradient1.Evaluate(1f / 8f)) * i;
            _color2[i] = (_gradient2.Evaluate(1f / 8f)) * i;
            _audioMaterial[i] = new Material(_material);
        }
        int countBand = 0;
        for (int i = 0; i < _noiseFlowField._amountofParticles; i++)
        {
            int band = countBand % 8;
            _noiseFlowField._particleMeshRenderer[i].material = _audioMaterial[band];
            _noiseFlowField._particles[i]._audioBand = band;
            countBand++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_useSpeed)
        {
            _noiseFlowField._particleMoveSpeed = Mathf.Lerp(_moveSpeedMinMax.x, _moveSpeedMinMax.y, _audioVis._AmplitudeBuffer);
            _noiseFlowField._particleRotateSpeed = Mathf.Lerp(_rotateSpeedMinMax.x, _rotateSpeedMinMax.y, _audioVis._AmplitudeBuffer);
        }
        for (int i = 0; i < _noiseFlowField._amountofParticles; i++)
        {
            if (_useScale)
            {
                float scale = Mathf.Lerp(_scaleMinMax.x, _scaleMinMax.y, _audioVis._audioBandBuffer[_noiseFlowField._particles[i]._audioBand]);
                _noiseFlowField._particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (_useColor1)
            {
                if (_audioVis._audioBandBuffer[i] > _colorThreshold1)
                {
                    _audioMaterial[i].SetColor(_colorName1, _color1[i] * _audioVis._audioBandBuffer[i] * _colorMultiplier1);
                }
                else
                {
                    _audioMaterial[i].SetColor(_colorName1, _color1[i] * 0f);
                }
            }
            if (_useColor2)
            {
                if (_audioVis._audioBand[i] > _colorThreshold2)
                {
                    _audioMaterial[i].SetColor(_colorName2, _color2[i] * _audioVis._audioBand[i] * _colorMultiplier2);
                }
                else
                {
                    _audioMaterial[i].SetColor(_colorName2, _color2[i] * 0f);
                }
            }
        }
    }
}
