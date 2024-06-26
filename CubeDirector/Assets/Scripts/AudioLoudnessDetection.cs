using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        
    }
    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);   
    }
    public float GetLoudnessFromMicrophone()
    {
        if (microphoneClip != null && Microphone.devices[0] != null)
        {
            return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
        }
        else
        {
            Debug.Log("No mic or mic not working");
            return 0;
        }
    }
    private float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0 ) 
            startPosition = 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //Compute Loudness
        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++) 
        { 
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        return totalLoudness / sampleWindow;
    }
}
