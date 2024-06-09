using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Animation blinking;
    private float timeBetweenBlinks = 5;
    private float timer = 0;

    void Start()
    {
        blinking = gameObject.GetComponent<Animation>();
        RandomTimeRange();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenBlinks )
        {
            blinking.Play();
            timer = 0;
            RandomTimeRange();
        }
    }
    private void RandomTimeRange()
    {
        timeBetweenBlinks = Random.Range(5, 12);
    }
}

