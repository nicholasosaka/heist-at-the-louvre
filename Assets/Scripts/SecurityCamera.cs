using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private Transform securityCamera;
    [SerializeField] private int degreesOfFreedom;
    [SerializeField] private int middleRotationPoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform player;

    private Quaternion currentRotationTarget;
    private bool addDoF;
    private float margin = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Quaternion startingPosition = Quaternion.Euler(0, 0, middleRotationPoint - degreesOfFreedom);
        securityCamera.rotation = startingPosition;

        currentRotationTarget = Quaternion.Euler(0, 0, middleRotationPoint + degreesOfFreedom);
        addDoF = false;
    }

    // Update is called once per frame
    void Update()
    {
        securityCamera.rotation = Quaternion.Slerp(securityCamera.rotation, currentRotationTarget, Time.deltaTime * rotationSpeed);
        if (Quaternion.Angle(securityCamera.rotation, currentRotationTarget) < margin) {
            // Debug.Log("Security Camera reached target roation");
            if (addDoF) {
                currentRotationTarget = Quaternion.Euler(0, 0, middleRotationPoint + degreesOfFreedom);
            } else {
                currentRotationTarget = Quaternion.Euler(0, 0, middleRotationPoint - degreesOfFreedom);
            }
            addDoF = !addDoF;
        }
    }

}
