using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatLineController : MonoBehaviour, IPulsable
{
    public BeatController beatController;
    public GameObject linePrefab;
    public float zStart=10f;
    public float zBeatInterval=2.5f;
    public int numberOfBeatLines=8;

    public TMP_Text beatText;

    float zSpeed;
    List<GameObject> beatLines;

    // Start is called before the first frame update
    void Start()
    {
        // calculate speed/sec based on bpm
        float bpm = beatController.bpm;
        float bps = 60f/bpm;
        zSpeed = zBeatInterval/bps;
        beatLines = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        float zOffset = -1f*deltaTime*zSpeed;
        Vector3 deltaPos = new Vector3(0f, 0f, zOffset);

        foreach (GameObject obj in beatLines) {
            obj.transform.position += deltaPos;
        }
    }
    public bool isReady() {
        return true;
    }
    public void Pulse(int beat) {
        // create a new beat line
        Quaternion initialRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject newObject = Instantiate(linePrefab, new Vector3(0,0,zStart), initialRotation);
        beatLines.Add(newObject);
        if (beatLines.Count > numberOfBeatLines) {
            GameObject delObject = beatLines[0];
            beatLines.RemoveAt(0);
            Destroy(delObject);
        }

        beatText.text = beat.ToString();
    }
}
