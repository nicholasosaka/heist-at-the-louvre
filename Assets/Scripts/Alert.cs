using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Alert {
    private static int alertLevel = 0;

    public int GetAlertLevel()  { 
        return alertLevel;
    }

    public int Increment() {
        Debug.Log("Incrementing alert");
        alertLevel++;
        if(alertLevel > 5) {
            alertLevel = 5;
        }
        return alertLevel;
    }

    public int Decrement() {
        alertLevel--;
        if(alertLevel < 0) {
            alertLevel = 0;
        }
        return alertLevel;
    }

}
