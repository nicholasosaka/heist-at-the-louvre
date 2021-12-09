using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFoVCollision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject cameraFOVGameObj;

    [SerializeField] private Transform securityCamera;


    private float durationOutsideOfFOV;
    private SpriteRenderer cameraFOVSprite;

    [SerializeField] private Color alertedRed;
    [SerializeField] private Color normalYellow;

    Alert alert;
    private bool canIncrementAlertValue;

    bool isInTriggerArea;
    void Start() {
        cameraFOVSprite = cameraFOVGameObj.GetComponent<SpriteRenderer>();

        //alert sprite transparent to start
        cameraFOVSprite.color = normalYellow;
        isInTriggerArea = false;
        
        alert = new Alert();
        canIncrementAlertValue = true;

    }

    void Update() {
        if(!isInTriggerArea) {
            durationOutsideOfFOV += Time.deltaTime;
        }

        if(durationOutsideOfFOV > 3f) {
            cameraFOVSprite.color = normalYellow;
            canIncrementAlertValue = true;
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Debug.Log("Enter");
            durationOutsideOfFOV = 0; //reset duration outside counter;
            
            //Set sprite to opaque
            cameraFOVSprite.color = alertedRed;
            isInTriggerArea = true;

            if(canIncrementAlertValue) {
                alert.Increment();
                canIncrementAlertValue = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player"){
            Debug.Log("Stay");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            Debug.Log("Exit");
            isInTriggerArea = false;
        }
    }
}
