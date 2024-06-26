using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialFromAudioScorce : MonoBehaviour
{
    public AudioSource source;
    public float minScale;
    public float maxScale;
    public AudioLoudnessDetection detector;

    public float loudnessSencabillity = 100;
    public float thresshold = 0.1f;

    public float currentLoundness;
    private Renderer rend;
    private void Start()
    {
        SetAlpha(0);
        source = GetComponent<AudioSource>();
        rend  = GetComponent<Renderer>();
    }
    void Update()
    {
        float loundness = detector.GetLoudnessFromMicrophone() * loudnessSencabillity;
        if (loundness < thresshold)
            loundness = 0; 

        currentLoundness = Mathf.Lerp(minScale, maxScale, loundness);
        SetAlpha(currentLoundness);
    }
    private void SetAlpha(float alphaValue)
    {
        if (rend != null)
        {
            Color color = rend.material.color;
            color.a = alphaValue;
            rend.material.color = color;
        }
    }
}
