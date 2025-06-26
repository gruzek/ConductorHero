using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseIntensity : MonoBehaviour, IPulsable
{
    public float pulseSize = 10f;
    public Color startColor;
    Color pulseColor;
    Material material;
    float lastPulseTime;
    Color currentPulseColor;
    int theBeat;

    //public GameObject [] objects;
    public float returnTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;    
        //startColor = material.GetColor("_pulseColor");
        //Debug.Log(startColor);
        lastPulseTime=Time.time;
        pulseColor = startColor * pulseSize;
        currentPulseColor = pulseColor;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSince = (Time.time-lastPulseTime);
        float factor = timeSince > returnTime ? 1f : timeSince/returnTime;
        if ((theBeat-1)%4 == 0) {
            currentPulseColor = pulseColor*pulseSize; 
        }
        else {
            currentPulseColor = pulseColor;
        }
        Color theColor = Color.Lerp(currentPulseColor, startColor, factor);
        material.SetColor("_pulseColor",theColor);
    }
    public bool isReady() {
        return true;
    }
    
    public void Pulse(int beat) {
        theBeat=beat;
        lastPulseTime=Time.time;
    }   
}
