using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    //private CharacterController _characterController;

    //// variables for basic motion
    //private Vector3 _moveDirection;
    //public Vector3 velocity;

    //[SerializeField] private float _playerSpeed;
    //[SerializeField] private float _gravity;
    //[SerializeField] private float _jumpHeight;

    //private float _veritcalVelocity; // for gravity

    //private void Awake()
    //{
    //    // references 

    //    _characterController = GetComponent<CharacterController>();
    //}


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    MovePlayer();
    //    //ApplyGravity();

    //}

    //private void ApplyGravity()
    //{
    //    // Moved to PlayerJump method
    //    //if ( _characterController.isGrounded == true && Input.GetKeyDown(KeyCode.Space))
    //    //{
    //    //    _veritcalVelocity = _jumpHeight;
    //    //}

    //    _veritcalVelocity -= _gravity * Time.deltaTime; 

    //    PlayerJump();

    //    _moveDirection.y = _veritcalVelocity * Time.deltaTime;
    //}

    //void PlayerJump()
    //{


    //    if (_characterController.isGrounded == true && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        _veritcalVelocity = _jumpHeight;
    //    }
    //}

    // void MovePlayer()
    //{
    //    // get keyboard input, apply to new vector3, move the controller

    //    _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
    //    //_moveDirection *= _playerSpeed * Time.deltaTime;
    //    velocity = _moveDirection * _playerSpeed;


    //    //_characterController.Move(_moveDirection);
    //    _characterController.Move(velocity * Time.deltaTime);

    //    if (_characterController.isGrounded == true)
    //    {
    //        print("player grounded");
    //    }
    //    else
    //    {
    //        velocity.y -= _gravity;
    //    }
    //}


    private CharacterController characterController;
    private Vector3 moveDirection;

    public float speed = 5f;
    private float gravity = 20f;

    public float jumpForce = 10f;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); // reference to Player Character Controller component

    }
    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        // GetAxis - incremental values vs GetAxisRaw - whole integer (-1,0,1)
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f,0f); // (x,y,z) using TagHolder helper script

        moveDirection = transform.TransformDirection(moveDirection); // change from local to global directions
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity(); // below - test gravity for jump

        characterController.Move(moveDirection); // charactercontroller.move requires a V3 to be passed. moveDirection defined as V3 above, and will change every frame.

    }

    void ApplyGravity()
    {
        //if (_characterController.isGrounded)
        //{
        verticalVelocity -= gravity * Time.deltaTime; // apply gravity so player remains planted and does not bounce/fall/float

        //jump
        PlayerJump();

        //}
        //else
        //{
        //    verticalVelocity -= gravity * Time.deltaTime; // not on ground, apply gravity
        //}

        moveDirection.y = verticalVelocity * Time.deltaTime; // multiply by deltaTime to smooth out and reduce vertical velocity

    }

    private void PlayerJump()
    {
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;

        }
    }





}
