using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInstruction : MonoBehaviour
{
    public int beat;
    public float hitTime;
    public float hitRange = 0.1f;
    public KeyCode hitKey;
    public float bpm;
    public ScoreController scoreController;

    public int successPoints=100;
    public int failPoints=-50;
    public bool isHit=false;
    public float zSpeed;
    bool isBadHit=false;
    float badHitTime;
    public float badHitDuration;
    public Color badHitColor;
    public float yAccel=-.000000001f;
    float ySpeed=0f;
    float rotSpeed=0f;
    public float rotAccel=0.000001f;
    float xSpeed=0f;
    public float xDamp=0.00000000001f;
    bool xFallRight=true;
    public float xMaxSpeed=0.0001f;

    public GameObject explosionPrefab;
    public AudioSource successSource;
    public AudioSource failSource;

    Color initialEmissionColor;

    // Start is called before the first frame update
    void Start()
    {
        Material material = GetComponent<MeshRenderer>().material;
        
        /*
        string [] propertyNames = material.GetPropertyNames(MaterialPropertyType.Texture);
        for (int i=0; i<propertyNames.Length; i++){
            Debug.Log(propertyNames[i]);
        }
        */
        initialEmissionColor = material.GetColor("_EmissionColor");
        xSpeed = Random.Range(0f,xMaxSpeed);
        if (Random.Range(0f,10f) > 5f) {
            xFallRight=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Material material = GetComponent<MeshRenderer>().material;
        float currentTime = Time.time;
        if (isBadHit) {
            //Debug.Log( "BAD HIT Duration: "+badHitDuration+" Time: "+badHitTime+" Current Time: "+currentTime);
            if (currentTime > badHitTime+badHitDuration) {
                //Debug.Log("Descroying object");
                Destroy(this.gameObject);
                return;
            }
            if (currentTime > (badHitTime+(badHitDuration*.1f)))
            {
                xSpeed=xSpeed-xDamp/Time.deltaTime;
                if (xSpeed <0f) { xSpeed=0f; }
                float trueXSpeed = xFallRight ? xSpeed : -1f * xSpeed;
                ySpeed+=yAccel/Time.deltaTime;
                rotSpeed+=rotAccel/Time.deltaTime;
                Vector3 rotation = new Vector3(0f, rotSpeed/Time.deltaTime, 0f); 
                this.gameObject.transform.Rotate(rotation);
                Vector3 pos = new Vector3(trueXSpeed/Time.deltaTime,ySpeed/Time.deltaTime,0f);
                this.gameObject.transform.Rotate(rotation);
                this.gameObject.transform.position += pos;
            }

            material.SetColor("_EmissionColor", badHitColor/2f);
            material.SetColor("_Color", badHitColor);
            return;
        }

        float deltaTime = Time.deltaTime;
        float zOffset = -1f*deltaTime*zSpeed;
        Vector3 deltaPos = new Vector3(0f, 0f, zOffset);
        this.transform.position += deltaPos;
        material.SetColor("_EmissionColor", initialEmissionColor);
        // check to see if inputs should even be checked, only check in the prior beat to the one we need
        float timeToHit = hitTime-currentTime;
        float beatsToHit = timeToHit/(60f/bpm);
        //Debug.Log ( "Time to Hit: "+timeToHit+" beatsToHit: "+beatsToHit);

        if (beatsToHit>2f) {
            material.SetColor("_EmissionColor", initialEmissionColor/10f);
            return;
        }
        // check to see if we're within range and the key is hit
        if (! isHit )
        {
            if ((currentTime >= hitTime-hitRange) && (currentTime <= hitTime+hitRange)) {
                //Debug.Log("Setting emission color in range");
                material.SetColor("_EmissionColor", initialEmissionColor*20f);
                // In Hit Range
                if (Input.GetKey(hitKey)) {
                    //Debug.Log ("GOOD -- Current Time: "+currentTime+" hitTime: "+hitTime+" hitRange: "+hitRange);
                    successSource.Play();
                    scoreController.AddScore(successPoints);
                    isHit=true;
                    GameObject explosion = Instantiate(explosionPrefab, this.gameObject.transform.position, Quaternion.identity);
                    ParticleSystem system = explosion.GetComponent<ParticleSystem>();
                    Material particleMaterial = system.GetComponent<Renderer>().material; 
                    particleMaterial.SetTexture("_BaseMap",material.GetTexture("_BaseMap"));
                    system.Play(true);
                    Destroy(this.gameObject);
                }
            } 
            else if (Input.GetKey(hitKey)) {
                //Debug.Log ("BAD HIT -- Current Time: "+currentTime+" hitTime: "+hitTime+" hitRange: "+hitRange);
                failSource.Play();
                scoreController.AddScore(failPoints);
                isHit=true;
                isBadHit=true;
                badHitTime = currentTime;
            }
            else if (currentTime > hitTime + hitRange) {
                //Debug.Log ("TOO LONG -- Current Time: "+currentTime+" hitTime: "+hitTime+" hitRange: "+hitRange);
                failSource.Play();
                scoreController.AddScore(failPoints*2);
                isHit = true;
                isBadHit=true;
                badHitTime = currentTime;
            }          
        }
    }
}
