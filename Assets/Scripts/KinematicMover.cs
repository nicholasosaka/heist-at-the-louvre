using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicMover : MonoBehaviour {
    public float satisfactionRadius;
    public float maxSpeed;
    public float timeToTarget;

    public class KinematicSteering {
        public Vector2 velocity;
        public KinematicSteering() {
            velocity = new Vector2(0,0);
        }
    }

    public KinematicSteering GetKinematicSteer(Vector2 current, Vector2 target) {

        KinematicSteering steering = new KinematicSteering();

        steering.velocity = target - current;


        if(steering.velocity.magnitude > satisfactionRadius) {

            steering.velocity /= timeToTarget;

            if(steering.velocity.magnitude > maxSpeed) {
                steering.velocity.Normalize();
                steering.velocity *= maxSpeed;
            }

        } else {
            steering = new KinematicSteering();
        }
        return steering;
    }
}
