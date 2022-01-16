using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Place script on the player object. In this instance, an empty gameObject called "Player" holds a child object that in turn
// holds the rigged models, weapons, etc. Once applied, the player movement script will have several visible changeable variables 
// and an empty spot to assign a local transform 

public class playerMovement : MonoBehaviour
{
    // access the standard character controller and animator (animator has state machines and logic built in animator window in unity)
    private CharacterController _characterController;
    private Animator _playerAnim;
   
    // direction player is moving, that we will apply other variables and factors to
    private Vector3 _movementDirection;

    // variables affecting movement and jumping- these can be modified in the inspector window to achieve desired results
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private float _gravity = 20f;
    [SerializeField] private float _playerJump = 10f;
    // this is a visual check to confirm gravity is being applied
    [SerializeField] private float _verticalVelocity;

    // variables to check if player off 0Z and set to 0Z
    private Vector3 _tempPos;

    // flip character if going left or right
    // horizontal input >0 = flip facing right, horizontal input <0 flip facing left
    // must 
    private float _playerFacingDirection;
    private bool _isFacingLeft = true;
    // local transform to flip the frame and model, NOT the parent player object
    [SerializeField] private Transform playerAnimatedCharacterTF;


    
    private void Awake()
    {
        // references 

        _characterController = GetComponent<CharacterController>();
        _playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // assign input, and use to determine which direciton player is facing, and rotate accordingly
        _playerFacingDirection = Input.GetAxis("Horizontal"); // GetAxis returns values between -1 & 0 (left) or between 0 & 1 (right)
        if (_playerFacingDirection > 0 )
        {
            // particular y values are dependent on the desired stance/view of character
            playerAnimatedCharacterTF.localRotation = Quaternion.Euler(0f, -250, 0f);//-270
        }

        if (_playerFacingDirection < 0)
        {
            // particular y values are dependent on the desired stance/view of character
            playerAnimatedCharacterTF.localRotation = Quaternion.Euler(0f, -110, 0f);//270
        }

        // take input and make a new vector3, change coordinates to global, multiply by speed factor & time.deltatime
        _movementDirection = new Vector3(_playerFacingDirection, 0f, 0f);
        _movementDirection = transform.TransformDirection(_movementDirection); // change coordinates to global
        _movementDirection *= _playerSpeed * Time.deltaTime;

        // apply gravity. charactercontroller has no gravity component. test for jump.
        ApplyGravityToPlayer();

        // apply vector to character controller to move player
        _characterController.Move(_movementDirection);
    }

    private void ApplyGravityToPlayer()
    { // subtract gravity to vertical velocity, use for y value of movementdirection vector3 (x,y,z)
        _verticalVelocity -= _gravity * Time.deltaTime;

        playerJump();
        // change the move direction y to a positive one when jumping 
        _movementDirection.y = _verticalVelocity * Time.deltaTime;
    }

    void playerJump()
    {
        // check if ground & pressed spacebar, apply playerJump value

        if (_characterController.isGrounded == true && Input.GetKey(KeyCode.Space))
        {
            // modify playerJump to find desired effect
            _verticalVelocity = _playerJump;
        }
    }
}
