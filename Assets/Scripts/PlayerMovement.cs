using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] Animator animator;

    private Vector2 velocity;

    // Update is called once per frame
    void Update()
    {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Vertical", velocity.y);
        animator.SetFloat("Speed", velocity.magnitude);

        //set idle direction and flashlight angle
        if (velocity.x > 0) {
            animator.SetFloat("IdleDirection", 4);
        } else if (velocity.x < 0) {
            animator.SetFloat("IdleDirection", 3);
        } else if (velocity.y > 0) {
            animator.SetFloat("IdleDirection", 2);
        } else if (velocity.y < 0) {
            animator.SetFloat("IdleDirection", 1);
        }
    }


    //non frame rate dependant updates
    void FixedUpdate() {
        rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
    }
}
