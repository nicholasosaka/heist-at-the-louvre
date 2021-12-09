using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlertSystem : MonoBehaviour
{

    [SerializeField] private Slider alertSlider;
    Alert alert;

    private float timeToDecrement;
    private float timeLastAdjusted;

    // Start is called before the first frame update
    void Start()
    {
        alert = new Alert();
        timeToDecrement = 15f; //15s to decrement
    }

    // Update is called once per frame
    void Update()
    {

        float currentAlertLevel = alert.GetAlertLevel();
        alertSlider.value = currentAlertLevel;

        if(timeLastAdjusted >= timeToDecrement) {
            alert.Decrement();
            alertSlider.value = alert.GetAlertLevel();
            timeLastAdjusted = 0;
        }

        timeLastAdjusted += Time.deltaTime;
    }
}
