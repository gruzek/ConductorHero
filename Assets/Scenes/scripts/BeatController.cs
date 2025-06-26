using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BeatController : MonoBehaviour
{
    // one type of each object to be controlled
    public GameObject [] gameObjects;
    public float bpm=90f;
    public float steps=2f;
    public float offsetSeconds=0.65f;

    public AudioSource audioSource;
    public AudioSource tickAudioSource;
    public bool playTick=true;

    public TMP_Text pressAnyKey;
    string pressAnyKeyText;
    public TMP_Text gameByKletiz;
    string gameByKletizText;
    public TMP_Text attribution;
    string attributionText;
    float timeMusicStarted=0f;

    float startTime;
    int lastPulseBeat;
    bool hasStarted=false;
    bool holdingDown=false;
    bool isReady=false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        lastPulseBeat = 0;

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        pressAnyKeyText = pressAnyKey.text;
        pressAnyKey.text = "";
        gameByKletizText = gameByKletiz.text;
        gameByKletiz.text = "";
        attributionText = attribution.text;
        attribution.text = "";
    }

    public bool MusicHasStarted() {
        return hasStarted;
    }

    public float GetTimeMusicStarted() {
        return timeMusicStarted;
    }

    public void StartTheMusic () {
        if (!hasStarted) {
            audioSource.Play();
            timeMusicStarted=Time.time;
            hasStarted=true;
        }
    }

    public float PreciseBeat() {
        float timeDeltaTrigger = 60f/bpm/steps;
        float sampleTime = (float)audioSource.timeSamples / (float)audioSource.clip.frequency;
        return (sampleTime-offsetSeconds)/timeDeltaTrigger;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isReady ) {
            for ( int i=0; i<gameObjects.Length; i++) {
                // get a script and call the "Pulse()" method
                GameObject obj = gameObjects[i];
                IPulsable pulsable = obj.GetComponent<IPulsable>();
                if (!pulsable.isReady()) {
                    return;
                }
            }     
            isReady=true;
            StartTheMusic();
        }

        if (!hasStarted && !audioSource.isPlaying) {
                return;
        }

        float timeDeltaTrigger = 60f/bpm/steps;
        float sampleTime = (float)audioSource.timeSamples / (float)audioSource.clip.frequency;
        int currentBeat = Mathf.FloorToInt((sampleTime-offsetSeconds)/timeDeltaTrigger);

        if (currentBeat>lastPulseBeat) {
            lastPulseBeat = currentBeat;
            
            if (playTick) { tickAudioSource.Play(); }

            for ( int i=0; i<gameObjects.Length; i++) {
                // get a script and call the "Pulse()" method
                GameObject obj = gameObjects[i];
                IPulsable pulsable = obj.GetComponent<IPulsable>();
                pulsable.Pulse(currentBeat);
            }     
        }

        if (hasStarted && (! audioSource.isPlaying) && lastPulseBeat>10) {
            gameByKletiz.text = gameByKletizText;
            pressAnyKey.text = pressAnyKeyText;
            attribution.text = attributionText;

            if (Input.anyKey) {
                holdingDown=true;
            }
            else if (holdingDown) {
                // a key was released
                // go back to start screen
                SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            }
        }
    }
}
