using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlPanel : MonoBehaviour
{

    Transform panel;
    public Transform player;
    public int reachableTreshold;
    float cameraDisabledTime;
    public float cameraDisabledCooldown;

    Alert alert;

    // Start is called before the first frame update
    void Start()
    {
        CameraController.disabled = false;
        panel = GetComponent<Transform>();
        cameraDisabledTime = 0;
        alert = new Alert();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Vector3.Distance(player.position, panel.position) < reachableTreshold) {
            CameraController.disabled = true;
            cameraDisabledTime = cameraDisabledCooldown;
            Debug.Log("CAMERAS DISABLED FOR " + cameraDisabledCooldown + " SECONDS");
            alert.Reset();
        }

        cameraDisabledTime -= Time.deltaTime;
        if(cameraDisabledTime <= 0 && CameraController.disabled) {
            CameraController.disabled = false;
            Debug.Log("CAMERAS ENABLED");
        }
    }
}
