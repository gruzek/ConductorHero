using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class InstructionMoveController : MonoBehaviour, IPulsable
{
    [Serializable]
    public struct Instruction {
        public int beat;
        public int symbolIndex;
        
    }

    [Serializable]
    public struct InstructionSymbol {
        public KeyCode keyCode;
        public GameObject symbolPrefab;
    }
    // will need to store start times & such
    public float badHitDuration=0.3f;
    public Color badHitColor;
    public BeatController beatController;
    public InstructionSymbol [] symbols;
    public Instruction [] leftInstructions;
    public Instruction [] rightInstructions;
    public float hitRange = 0.15f;
    public float zStart=10f;
    public float yStart=4f;
    public float xOffset=2f;
    public float zBeatInterval=2.5f;
    public int numberOfBeatLines=8;
    int currentLeftInstruction=0;
    int currentRightInstruction=0;
    int currentBeat=0;
    float zSpeed;

    public GameObject explosionPrefab;
    public GameObject scoreText;
    ScoreController scoreController;
    public AudioSource successSource;
    public AudioSource failSource;

    public TextAsset rightInstructionsCSV;
    public TextAsset leftInstructionsCSV;
    bool hasStarted=false;


    //List<GameObject> activeGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        float bpm = beatController.bpm;
        float bps = 60f/bpm;
        zSpeed = zBeatInterval/bps;
        //this.activeGameObjects = new List<GameObject>();
        scoreController = scoreText.GetComponent<ScoreController>();
        rightInstructions = parseInstructions(rightInstructionsCSV);
        //Debug.Log(rightInstructions.Length);
        //Debug.Log(rightInstructionsCSV.text);
        leftInstructions = parseInstructions(leftInstructionsCSV);
    }


    // Update is called once per frame
    void Update()
    {
        /*
        float deltaTime = Time.deltaTime;
        float zOffset = -1f*deltaTime*zSpeed;
        Vector3 deltaPos = new Vector3(0f, 0f, zOffset);

        if (activeGameObjects != null) {
            foreach (GameObject obj in activeGameObjects) {
                obj.transform.position += deltaPos;
            }        
        }
        */
    }

    Instruction [] parseInstructions( TextAsset csvFile ) {
        string [] lines = csvFile.text.Split("\n");
        List<Instruction> instructions = new List<Instruction>();

        for (int i=0; i<lines.Length; i++) {
            Instruction instruction = new Instruction();
            string [] data = lines[i].Split(",");
            instruction.beat=int.Parse(data[0]);
            instruction.symbolIndex=int.Parse(data[1]);
            instructions.Add(instruction);
        }

        return instructions.ToArray();
    }


    public bool isReady() {
        return true;
    }
    public void Pulse( int beat ) {
        currentBeat = beat;
        // check to see if we need to instantiate an object

        if (currentRightInstruction < rightInstructions.Length &&
            rightInstructions[currentRightInstruction].beat==currentBeat+numberOfBeatLines) {
                Instruction theInstruction = rightInstructions[currentRightInstruction];
                InstructionSymbol theSymbol = symbols[theInstruction.symbolIndex];

                Quaternion initialRotation = Quaternion.Euler(90f, 0f, 180f);
                //calculate the hit time
                float currentTime = Time.time;
                float secondsPerBeat = 60f/beatController.bpm;

                GameObject newObject = Instantiate(theSymbol.symbolPrefab, new Vector3(xOffset,yStart,zStart), initialRotation);
                NoteInstruction instruction = newObject.GetComponent<NoteInstruction>();
                instruction.hitTime = 8*secondsPerBeat+currentTime;
                instruction.beat = rightInstructions[currentRightInstruction].beat;
                instruction.hitKey = theSymbol.keyCode;
                instruction.zSpeed = zSpeed;
                instruction.badHitDuration = badHitDuration;
                instruction.badHitColor = badHitColor;
                instruction.explosionPrefab = explosionPrefab;
                instruction.bpm=beatController.bpm;
                instruction.scoreController=scoreController;
                instruction.successSource = successSource;
                instruction.failSource = failSource;
                instruction.hitRange = hitRange;
                currentRightInstruction++;
        }

        if (currentLeftInstruction < leftInstructions.Length &&
            leftInstructions[currentLeftInstruction].beat==currentBeat+numberOfBeatLines) {
                Instruction theInstruction = leftInstructions[currentLeftInstruction];
                InstructionSymbol theSymbol = symbols[theInstruction.symbolIndex];

                Quaternion initialRotation = Quaternion.Euler(90f, 0f, 180f);
                GameObject newObject = Instantiate(theSymbol.symbolPrefab, new Vector3(-1f*xOffset,yStart,zStart), initialRotation);
                NoteInstruction instruction = newObject.GetComponent<NoteInstruction>();
                //calculate the hit time
                float currentTime = Time.time;
                float secondsToHit = 60f/beatController.bpm;
                instruction.hitTime = 8*secondsToHit+currentTime;
                instruction.beat = leftInstructions[currentLeftInstruction].beat;
                instruction.hitKey = theSymbol.keyCode;
                instruction.zSpeed = zSpeed;
                instruction.badHitDuration = badHitDuration;
                instruction.badHitColor = badHitColor;
                instruction.explosionPrefab = explosionPrefab;
                instruction.bpm=beatController.bpm;
                instruction.scoreController=scoreController;
                instruction.successSource = successSource;
                instruction.failSource = failSource;
                instruction.hitRange = hitRange;
                currentLeftInstruction++;
        }    
    }
}
