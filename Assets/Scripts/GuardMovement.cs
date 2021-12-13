using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : KinematicMover
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] private Transform guardTransform;

    [SerializeField] private GameObject pathParent;
    [SerializeField] private float safetyMargin;

    private List<Vector3> waypoints;
    KinematicSteering steering;
    
    private int goalWaypointIndex;


    // Start is called before the first frame update
    void Start()
    {

        steering = new KinematicSteering();

        waypoints = new List<Vector3>();
        //get all nodes
        for(int i = 0; i < pathParent.transform.childCount; i++) {
            waypoints.Add(pathParent.transform.GetChild(i).position);
        }

        goalWaypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToGoal = (guardTransform.position - waypoints[goalWaypointIndex]).magnitude;
        if(distanceToGoal < safetyMargin) {
            if(goalWaypointIndex < waypoints.ToArray().Length - 1){
                goalWaypointIndex++;
            } else {
                goalWaypointIndex = 0;
            }
        }

        //animation
        animator.SetFloat("Horizontal", steering.velocity.x);
        animator.SetFloat("Vertical", steering.velocity.y);
        animator.SetFloat("Speed", steering.velocity.magnitude);


        if (steering.velocity.x > 0) { //right
            animator.SetFloat("IdleDirection", 4);
        } else if (steering.velocity.x < 0) { //left
            animator.SetFloat("IdleDirection", 3);
        } else if (steering.velocity.y > 0) { //up
            animator.SetFloat("IdleDirection", 2);
        } else if (steering.velocity.y < 0) {//down
            animator.SetFloat("IdleDirection", 1);
        }

        steering = GetKinematicSteer(rb.position, waypoints[goalWaypointIndex]);
    }

    void FixedUpdate() {
        rb.velocity = steering.velocity;
    }
}
