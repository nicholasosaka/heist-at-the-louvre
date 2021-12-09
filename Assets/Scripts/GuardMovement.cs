using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeedModifier;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] private Transform guardTransform;

    [SerializeField] private GameObject pathParent;
    [SerializeField] private float safetyMargin;

    private List<Vector3> pathNodes;
    
    private float interpolationValue;
    private int goalNodeIndex;


    // Start is called before the first frame update
    void Start()
    {
        pathNodes = new List<Vector3>();
        //get all nodes
        for(int i = 0; i < pathParent.transform.childCount; i++) {
            pathNodes.Add(pathParent.transform.GetChild(i).position);
        }

        Debug.Log("Guard path is " + pathNodes.ToArray().Length + " nodes long.");

        interpolationValue = 0;
        goalNodeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        interpolationValue += Time.deltaTime/movementSpeedModifier;
        float distanceToGoal = (guardTransform.position - pathNodes[goalNodeIndex]).magnitude;
        if(distanceToGoal > safetyMargin) {
            Vector3 interpolatedPosition = Vector3.Lerp(guardTransform.position, pathNodes[goalNodeIndex], interpolationValue);
            
            Vector2 deltaPosition = interpolatedPosition - guardTransform.position;
            guardTransform.position = interpolatedPosition;

            //animation
            animator.SetFloat("Horizontal", deltaPosition.x);
            animator.SetFloat("Vertical", deltaPosition.y);
            animator.SetFloat("Speed", deltaPosition.magnitude);


            if (deltaPosition.x > 0) { //right
                animator.SetFloat("IdleDirection", 4);
            } else if (deltaPosition.x < 0) { //left
                animator.SetFloat("IdleDirection", 3);
            } else if (deltaPosition.y > 0) { //up
                animator.SetFloat("IdleDirection", 2);
            } else if (deltaPosition.y < 0) {//down
                animator.SetFloat("IdleDirection", 1);
            }

        } else {
            Debug.Log("Goal State");
            if(goalNodeIndex < pathNodes.ToArray().Length - 1){
                goalNodeIndex++;
            } else {
                goalNodeIndex = 0;
            }
            interpolationValue = 0;

        }
    }
}
