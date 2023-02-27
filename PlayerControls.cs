

    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float thrustSpeed = 6000;
    public float turnSpeed = 1000;
    public float hoverPower = 30;
    public float hoverHeight = 2;


    //NEW
    public float jumpHeight = 1;  //Jump height (above hover)
    public float jumpPower = 200; //Jump power (against gravity)
    public float extra_gravity = 0; //Additional downwards force of rigidbody

    private float thrustInput;
    private float turnInput;
    private Rigidbody shipRigidBody;

    // Use this for initialization
    void Start () {
        shipRigidBody = GetComponent<Rigidbody>();
        shipRigidBody.inertiaTensorRotation = Quaternion.identity;
    }
        
        // Update is called once per frame
        void Update () {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate() {

        //Increase ship fall speed...
        shipRigidBody.AddForce(0f, extra_gravity, 0f);

        // Turning the ship
        shipRigidBody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

        // Moving the ship
        shipRigidBody.AddRelativeForce(0f, 0f, thrustInput * thrustSpeed);


        //NEW 

        //Raycast is only defined once:
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        
        // Make the bunny hop (code based on hovering):
        //Additional condition: pressing GetButtonDown
        if (Physics.Raycast(ray, out hit, hoverHeight) && Input.GetButtonDown("Jump"))
        {
   
            float proportionalHeight = hoverHeight + ((jumpHeight - hit.distance) / jumpHeight);
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * jumpPower;
            shipRigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        // Hovering
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverPower;
            shipRigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }


        //Bunny drifting
        if (Input.GetButton("Fire3"))
        {
   
            // Turning the ship
            shipRigidBody.AddRelativeTorque(0f, turnInput * turnSpeed * 2f, 0f);
            //shipRigidBody.AddRelativeForce(0f, 0f, thrustInput * thrustSpeed * 0.5f);
        }
    }
}
