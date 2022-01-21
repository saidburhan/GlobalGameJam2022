using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float speed = 5;
    private float rotationSpeed = 5f;
    private CharacterController characterControler;

    // Start is called before the first frame update
    void Start()
    {
        characterControler = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        characterControler.SimpleMove(new Vector3(horizontal,0,0));

       // Vector3 movementDirection = new Vector3(horizontal,0,vertical);
    }
}

//private Rigidbody rigidBody;
//private float speed = 5;
//private bool jump;
//private bool isGrounded;
//private float jumpForse = 6;

//void Start()
//{
//    rigidBody = GetComponent<Rigidbody>();
//}

//void Update()
//{
//    if (Input.GetButtonDown("Jump"))
//    {
//        jump = true;
//    }
//}


//private void FixedUpdate()
//{
//    // Moving Player..
//    float horizontal = Input.GetAxis("Horizontal");
//    rigidBody.velocity = new Vector3(horizontal * 5, rigidBody.velocity.y, 0);

//    // isgrounded
//    isGrounded = Physics.OverlapBox(rigidBody.position, Vector3.one * 2).Length > 2;


//    if (jump && isGrounded)
//    {
//        rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForse, 0);
//        jump = false;
//    }

//}