using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour, IPulsable
{
    public TMP_Text messageText;
    public BeatController beatController; 
    public float letterDuration=0.0625f;
    public float messageDuration=1.5f;

    [Serializable]
    public struct MessageInstruction {
        public int beat;
        public string message;
    }

    public MessageInstruction [] instructions;
    // Start is called before the first frame update

    string currentMessageText;
    int currentInstructionIndex=0;
    float currentMessageStartTime;

    void Start()
    {
        messageText.text="";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMessageText!=null) {
            //Debug.Log(currentMessageText);
            // calculate which letter we should be on
            float timePerLetter = 60f/beatController.bpm*letterDuration;
            float timeSinceMessageStart = Time.time-currentMessageStartTime;
            int numLetters = Mathf.FloorToInt(timeSinceMessageStart/timePerLetter);
            if (numLetters>=currentMessageText.Length) {
                messageText.text = currentMessageText;
            }
            else {
                messageText.text = currentMessageText.Substring(0,numLetters);
            }

            if ((Time.time-currentMessageStartTime)>messageDuration) {
                currentMessageText=null;
                messageText.text="";
            }
        }
    }

    public bool isReady() {
        return true;
    }
    public void Pulse(int beat) {
             //Debug.Log("Pulse: "+beat+" currentInstruction Beat: "+instructions[currentInstructionIndex].beat);
       
        if (currentInstructionIndex>=instructions.Length) {
            return;
        }

        if (currentMessageText == null) {
            if (beat >= instructions[currentInstructionIndex].beat) {
                currentMessageText = instructions[currentInstructionIndex].message;
                currentMessageStartTime=Time.time;
                currentInstructionIndex++;
            }
        }
    }
}
