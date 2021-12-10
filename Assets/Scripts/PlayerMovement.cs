using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : KinematicMover
{

    Camera mainCamera;
    [SerializeField] public Animator animator;
    KinematicSteering steering;
    Vector2 target;
    Rigidbody2D rb;

    void Start() {
        mainCamera = Camera.main;
        steering = new KinematicSteering();
        rb = GetComponent<Rigidbody2D>();
        target = new Vector2(-4, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) { //left click
            Vector2 clickLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            target = Camera.main.ScreenToWorldPoint(clickLocation);
        }

        steering = GetKinematicSteer(rb.position ,target);


        // velocity.x = Input.GetAxisRaw("Horizontal");
        // velocity.y = Input.GetAxisRaw("Vertical");

        Debug.Log(steering.velocity);

        animator.SetFloat("Horizontal", steering.velocity.x);
        animator.SetFloat("Vertical", steering.velocity.y);
        animator.SetFloat("Speed", steering.velocity.magnitude);

        //set idle direction and flashlight angle
        if (steering.velocity.x > 0) {
            animator.SetFloat("IdleDirection", 4);
        } else if (steering.velocity.x < 0) {
            animator.SetFloat("IdleDirection", 3);
        } else if (steering.velocity.y > 0) {
            animator.SetFloat("IdleDirection", 2);
        } else if (steering.velocity.y < 0) {
            animator.SetFloat("IdleDirection", 1);
        }
    }


    //non frame rate dependant updates
    void FixedUpdate() {
        rb.MovePosition(rb.position + steering.velocity * maxSpeed * Time.fixedDeltaTime);
    }
}
