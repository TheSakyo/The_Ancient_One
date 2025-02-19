using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rigidbody rb;
    public float accelRate;
    public float speedLimit = 10f;
    public float jumpForce = 100f;
    public Animator animator;
    public GameObject goPlayer;

    private float targetSpeed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.GetFloat("xVelocity"));
        //flip charachter sprite based one movement inputs
        if (Input.GetAxis("Horizontal") <  0) //player is pressing left
        {
            goPlayer.transform.localScale = new Vector3(-1,1,1);
            targetSpeed = -speedLimit;
        }
        else if (Input.GetAxis("Horizontal") > 0) //player is pressing right
        {
            goPlayer.transform.localScale = Vector3.one;
            targetSpeed = speedLimit;
        }
        else //player is not pressing movement buttons
        {
            targetSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {


        float speedDif = targetSpeed - rb.velocity.x;
        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector3.right);

        //movement animation variable update
        animator.SetFloat("xVelocity" , Mathf.Abs(rb.velocity.magnitude) );
        
        //jump
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        
    }
}
