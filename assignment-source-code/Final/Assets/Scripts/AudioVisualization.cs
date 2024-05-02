using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class AudioVisualization : MonoBehaviour
{
    AudioSource _audioSource;
    float[] _samplesLeft = new float[512];
    float[] _samplesRight = new float[512];
    public float[] _freqBand = new float[8];
    float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];
    public float[] _freqBandHighest = new float[8];
    public  float[] _audioBand = new float[8];
    public  float[] _audioBandBuffer = new float[8];

    public float _Amplitude, _AmplitudeBuffer;
    float _AmplitudeHighest;
    public float _audioProfile;

    [SerializeField] private AudioClip _audioClip;

    public enum _channel {Stero,Left,Right};
    public _channel channel = new _channel();

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);
        _audioSource.clip = _audioClip;
        _audioClip.LoadAudioData();

        if (_audioClip.loadState == AudioDataLoadState.Loaded)
        {
            Debug.Log("Audio clip loaded successfully!");
        }
        else
        {
            Debug.LogError("Audio clip failed to load!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetSpecturmAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();

        if (Input.GetKeyDown(KeyCode.K))
        {
            // Check if the audio source is not already playing
            if (!_audioSource.isPlaying)
            {
                // Start playing the audio source
                _audioSource.Play();
            }
        }
    }
    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;

        }
    }
    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_CurrentAmplitude > _AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }
    void GetSpecturmAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft,0,FFTWindow.BlackmanHarris);
        _audioSource.GetSpectrumData(_samplesRight,1, FFTWindow.BlackmanHarris);
    }
    void MakeFrequencyBands()
    {
        int count = 0;
        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stero)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if (channel == _channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }
                count++;

            }
            average /= count;
            _freqBand[i] = average*10;
        }
    }
    void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }
}
