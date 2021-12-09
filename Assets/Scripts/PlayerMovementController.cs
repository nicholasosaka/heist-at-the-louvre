using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public float maxVelocity = 16;

    Rigidbody rb;
    Transform playerTransform;
    Camera mainCamera;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO replace with kinematic arrive call on click

        Vector3 clickLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y);
        Vector3 worldClickLocation = mainCamera.ScreenToWorldPoint(clickLocation);
        transform.LookAt(worldClickLocation + Vector3.up * transform.position.y);
        velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * maxVelocity;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
