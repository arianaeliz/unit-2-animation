using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson_Controller : MonoBehaviour
{

    Animator anim;
    Rigidbody rb;

    //character movement
    public float jumpVelocity = 10f;
    public float accel = 10f;
    public float decel = 10f;
    public float maxSpeed = 10f;
    [HideInInspector]
    public float speed = 0;

    //camera movement
    public Transform cam;
    public float turnSmoothSpeed = 0.1f;
    float turnSmoothVelocity;

    //ground check
    bool isGrounded;
    public LayerMask groundLayer;

    [HideInInspector]
    public Vector3 input;



    // Start is called before the first frame update
    void Start()
    {
        //get reference to rigidbody
        rb = GetComponent<Rigidbody>();
        //get reference to animator component on game object
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //getting input from the player and storing it as a vector
        //original vector 3
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        float inputMagnitude = input.magnitude;

        //using input magnitude variable to avoid costly square root calulation when we normalize direction
        //clamp down length of vector by one
        //now we can easily scale it by any speed variable
        Vector3 direction = input/inputMagnitude;

        //if the movement is parial, we want to limit movement speed
        float currentMaxSpeed = inputMagnitude * maxSpeed;

        //make sure we dont move faster diagonally
        if(currentMaxSpeed > maxSpeed)
        {
            currentMaxSpeed = maxSpeed;
        }

        //if player is pressing the movement buttons, accelerate but dont exceed the max speed
        //if user pushes anything at all
        if(direction.magnitude >= 0.05f){

            //ramp up to max speed
            if(speed < currentMaxSpeed){
                speed += accel * Time.deltaTime;
            } else {
                //otherwise slow down
                speed = Mathf.Lerp(speed, currentMaxSpeed, 0.1f);
            }

            //find an angle of our character facing
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            //create smooth transition from one facing position to another facing position
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothSpeed);

            //face direction of player movement
            transform.rotation = Quaternion.Euler(0, angle, 0);

           
            
            /* old version of rotation
            //face the direction of the player movement
            //set transform on forward to be set to vector along horizontal and vertical axis
            //transform.forward = direction;
            */
        
        
        } 
        
        //decelerate if the player isnt pressing movemnt buttons
        else { 
            //natural deceleration if no input
            speed -= decel * Time.deltaTime;
        }

        //make sure we dont go backwards or that the speed variable doesnt count backwards
        if (speed < 0)
        {
            speed = 0;
        }


        //communicate speed to animator
        //set parameter to variable
        anim.SetFloat("speed", speed);

        //communicate groundedness to animator
        //set parameter to variable
        anim.SetBool("isGrounded", isGrounded);

        //call jump method when user pushes space bar
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            anim.SetTrigger("jump");
        }
        
    }

    public void FixedUpdate()
    {
        CheckGround();
    }

    public void Jump()
    {
        Vector3 vel = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        rb.velocity = vel;
        anim.applyRootMotion = false;
    }

    void CheckGround()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, groundLayer)){
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
}
