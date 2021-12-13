using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : KinematicMover
{

    Camera mainCamera;
    [SerializeField] public Animator animator;
    KinematicSteering steering;
    Vector2 target;
    Rigidbody2D rb;

    AStar aStarHandler;
    
    public Grid tilemapGrid;

    List<AStar.Node> path;
    private int pathGoalNodeIndex;

    private float moveCooldown;

    void Start() {
        mainCamera = Camera.main;
        steering = new KinematicSteering();
        rb = GetComponent<Rigidbody2D>();
        target = new Vector2(-4.25f, 4.25f);

        aStarHandler = new AStar(tilemapGrid);
        path = new List<AStar.Node>();
        moveCooldown = -1f;
        pathGoalNodeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && moveCooldown < 0) { //left click
            Vector2 clickLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            path = aStarHandler.GetPath(Camera.main.ScreenToWorldPoint(clickLocation), rb.position);
            moveCooldown = 1f;
            pathGoalNodeIndex = 0;
        }

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

        ProgressTargetAlongPath(path);
        steering = GetKinematicSteer(rb.position, target);

        // Vector3 debugstartXX = new Vector3(target.x-.25f, target.y, 2);
        // Vector3 debugendXY = new Vector3(target.x+.25f, target.y, 2);
        // Vector3 debugstartYX = new Vector3(target.x, target.y-.25f, 2);
        // Vector3 debugendYY = new Vector3(target.x, target.y+.25f, 2);
        // Debug.DrawLine(debugstartXX, debugendXY, Color.green, 1f, false);
        // Debug.DrawLine(debugstartYX, debugendYY, Color.green, 1f, false);

        moveCooldown -= Time.deltaTime;
    }


    //non frame rate dependant updates
    void FixedUpdate() {
        rb.MovePosition(rb.position + steering.velocity * maxSpeed * 2 * Time.fixedDeltaTime);
    }

    void ProgressTargetAlongPath(List<AStar.Node> path) {
        // foreach(AStar.Node p in path) {
        //     Vector3 debugstartX = new Vector3(p.position.x-.25f, p.position.y, 1);
        //     Vector3 debugendX = new Vector3(p.position.x+.25f, p.position.y, 1);
        //     Debug.DrawLine(debugstartX, debugendX, Color.red, 50f, false);
        //     Vector3 debugstartY = new Vector3(p.position.x, p.position.y-.25f, 1);
        //     Vector3 debugendY = new Vector3(p.position.x, p.position.y+.25f, 1);
        //     Debug.DrawLine(debugstartY, debugendY, Color.red, 50f, false);
        // }

        if(path.ToArray().Length != 0) {
            AStar.Node goalNode = path[pathGoalNodeIndex];
            float distanceToGoal = Vector2.Distance(rb.position, goalNode.position);

            if(distanceToGoal < satisfactionRadius) {
                if(pathGoalNodeIndex < path.ToArray().Length -1) {
                    pathGoalNodeIndex++;
                    target = path[pathGoalNodeIndex].position;

                    // // DEBUG
                    // Vector3 debugstartXX = new Vector3(target.x-.25f, target.y, 3);
                    // Vector3 debugendXY = new Vector3(target.x+.25f, target.y, 3);
                    // Vector3 debugstartYX = new Vector3(target.x, target.y-.25f, 3);
                    // Vector3 debugendYY = new Vector3(target.x, target.y+.25f, 3);
                    // Debug.DrawLine(debugstartXX, debugendXY, Color.blue, 2f, false);
                    // Debug.DrawLine(debugstartYX, debugendYY, Color.blue, 2f, false);
                } else {
                }
            } else {
                if(target != goalNode.position) {
                    target = goalNode.position;
                }
            }
        }
    }
}
