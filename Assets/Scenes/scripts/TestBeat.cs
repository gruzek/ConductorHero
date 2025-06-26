using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestBeat : MonoBehaviour
{
    // sets vinette intensity based on a beat
    public float pulseSize = 10f;
    Color startColor;
    Color currentColor;
    Material material;
    float lastPulseTime;

    //public GameObject [] objects;
    public float returnTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;    
        //string [] names = material.GetPropertyNames(MaterialPropertyType.Vector);
        /*for (int i=0; i<names.Length; i++) {
            Debug.Log(names[i]);
        }  
        */
        startColor = material.GetColor("_pulseColor");
        StartCoroutine(TestBPM());
        //Color color = currentColor*10f;
        //material.SetColor("_pulseColor", color);  
        //var maxColorComponent = _emissionColor.maxColorComponent;
        //var scaleFactor = k_MaxByteForOverexposedColor / maxColorComponent;
        //float intensity = Mathf.Log(255f / scaleFactor) / Mathf.Log(2f); 
        lastPulseTime=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSince = (Time.time-lastPulseTime);
        float factor = timeSince > returnTime ? 1f : timeSince/returnTime;
        Color theColor = Color.Lerp(currentColor, startColor, factor);
        material.SetColor("_pulseColor",theColor);
    }

    public void Pulse() {
        currentColor = startColor * pulseSize;
        Debug.Log("Pulse() +"+currentColor.ToString());

        lastPulseTime=Time.time;
        Debug.Log(lastPulseTime);
    }

    IEnumerator TestBPM() {
        while (true) {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }
}
