using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rigidbody rb;
    public float acceleration;
    public float speedLimit = 10f;
    public Animator animator;
    public GameObject goPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.GetFloat("xVelocity"));
        if (Input.GetAxis("Horizontal") <  0)
        {
            goPlayer.transform.localScale = new Vector3(-1,1,1);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            goPlayer.transform.localScale = Vector3.one;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * acceleration ,0f ,0f));
        rb.velocity = Vector3.ClampMagnitude(rb.velocity,speedLimit);
        animator.SetFloat("xVelocity" , Mathf.Abs(rb.velocity.magnitude) );
    }
}
