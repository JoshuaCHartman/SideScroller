using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 2.5D Controller.
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
    [SerializeField] private float _playerWalkSpeed = 12f;
    [SerializeField] private float _playerRunSpeed = 24f;
    [SerializeField] private float _playerActiveSpeed; // holder for speed depending on if walking or running

    [SerializeField] private float _gravity = 50f; // starting value for testing = 20
    [SerializeField] private float _playerJump = 15f; // starting value for testing = 10. adjusted for snappiness
    // this is a visual check in inspector to confirm gravity is being applied
    [SerializeField] private float _verticalVelocity;
    [SerializeField] private float _doubleJumpSpeed = 35f;

    // double jump bool - will allow one extra jump
    private bool hasDoubleJump = false;

    // variables to check if player off 0Z and set to 0Z
    private Vector3 _tempPos;

    // flip character if going left or right
    // horizontal input >0 = flip facing right, horizontal input <0 flip facing left
    private float _playerFacingDirection;

    // local transform to flip the frame and model, NOT the parent player object
    [SerializeField] private Transform _playerAnimatedCharacterTF;

    private void Awake()
    {
        // references 

        _characterController = GetComponent<CharacterController>();
        _playerAnim = GetComponentInChildren<Animator>(); // animator is attached to child component holding models
    }
    private void FixedUpdate() // physics, end of frame
    {
        // if player is not on 0z, change xz value to 0

        _tempPos = transform.position;

        if (transform.position.z != 0)
        {
            _tempPos.z = 0;
        }

        transform.position = _tempPos;

    }
    // Update is called once per frame
    void Update()
    {
        // move (rotate to face correct direction, walk, run, jump, double jump, double jump cool down)
        MovePlayer();

        // attack (melee, projectile)
        // in attack script
    }

    

    void MovePlayer()
    {
        //// assign input, and use to determine which direciton player is facing, and rotate accordingly
      
        RotatePlayerFacingDirectionBasedOnInput();

        // make if walk/run/idle

        // _movementDirection *= _playerWalkSpeed * Time.deltaTime; // for testing. player speed is affected by and set in states below

       SetActiveMovementSpeedAndTriggerAnimations();

        _movementDirection *= _playerActiveSpeed * Time.deltaTime;

        // apply gravity. charactercontroller has no gravity component. test for jump input & if can double jump. jump. trigger double 
        // jump cooldown coroutine
        ApplyGravityToPlayerAndJump();

        // apply vector to character controller to move player
        _characterController.Move(_movementDirection);
    }

    void RotatePlayerFacingDirectionBasedOnInput()
    {
        // assign input, and use to determine which direciton player is facing, and rotate accordingly
        _playerFacingDirection = Input.GetAxis("Horizontal"); // GetAxis returns values between -1 & 0 (left) or between 0 & 1 (right)
        if (_playerFacingDirection > 0)
        {
            // particular y values are dependent on the desired stance/view of character
            // a local rotation is applied, not globabl
            _playerAnimatedCharacterTF.localRotation = Quaternion.Euler(0f, -250, 0f);//-270
        }

        if (_playerFacingDirection < 0)
        {
            // particular y values are dependent on the desired stance/view of character
            // a local rotation is applied, not globabl
            _playerAnimatedCharacterTF.localRotation = Quaternion.Euler(0f, -110, 0f);//270
        }

        // take input and make a new vector3, change coordinates to global, multiply by speed factor & time.deltatime
        _movementDirection = new Vector3(_playerFacingDirection, 0f, 0f);
        _movementDirection = transform.TransformDirection(_movementDirection); // change coordinates to global
    }


    void SetActiveMovementSpeedAndTriggerAnimations()
    {
        if (_characterController.isGrounded)
        {
            if (_movementDirection == Vector3.zero)
            {
                // Idle
                Idle();
            }

            if (_movementDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            // if vector3 = 0, then there is no motion. this checks to make sure character is in motion AND Left Shift NOT held down
            {
                // walk
                Walk();
            }

            if (_movementDirection != Vector3.zero && Input.GetKeyDown(KeyCode.LeftShift))
            {
                // run
                Run();
            }
        }
    }

    private void Idle()
    {
        _playerAnim.SetFloat("playerActiveSpeed", 0f, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        _playerActiveSpeed = _playerWalkSpeed;
        _playerAnim.SetFloat("playerActiveSpeed", 0.5f, 0.25f, Time.deltaTime);
    }

    private void Run()
    {
        _playerActiveSpeed = _playerRunSpeed;
        _playerAnim.SetFloat("playerActiveSpeed", 1f, 0.1f, Time.deltaTime);
    }

    private void ApplyGravityToPlayerAndJump()
    {

        // subtract gravity to vertical velocity, use for y value of movementdirection vector3 (x,y,z)
        _verticalVelocity -= _gravity * Time.deltaTime;
        // if jumping, change vertical velocity to jump value
        CheckForInputForPlayerJump();
        // change the move direction y to a positive one when jumping 

        _movementDirection.y = _verticalVelocity * Time.deltaTime;

    }

    void CheckForInputForPlayerJump()
    {
        // check if ground & pressed spacebar, has double jump, apply playerJump value

        if (_characterController.isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            // set double jump bool to true since on ground
            hasDoubleJump = true;

            // modify playerJump value in inspector & above to find desired effect
            ApplyPlayerJump();

        }
        else if (Input.GetKeyDown(KeyCode.Space) && hasDoubleJump == true)
        {

            // turn off double jump so not able to spam jumps
            hasDoubleJump = false;
            ApplyPlayerDoubleJump();

        }
    }

    void ApplyPlayerJump()
    {
        // set vertical velocity to playerJump value
        _verticalVelocity = _playerJump;

        _playerAnim.SetTrigger("jumpTrigger");
    }

    void ApplyPlayerDoubleJump()
    { 
        // increase values to make double jump snappier.
        // starts cooldown with a gravity penalty for 2s
        _verticalVelocity = _playerJump * 1.5f;
        _playerActiveSpeed = _doubleJumpSpeed;
        _gravity = 100f;
        _playerAnim.SetTrigger("jumpTrigger");
        StartCoroutine(CooldownToRestoreGravityAfterDoubleJump()); // prevents constant doublejumping at more dramatic result. Jumping will be nerfed with higher gravity for 2seconds.
    }

    IEnumerator CooldownToRestoreGravityAfterDoubleJump()
    {
        yield return new WaitForSeconds(2f);
        _gravity = 50f;
    }


    // helper scripts

    public float GetPlayerDirection() // return a - or + value float to indicate player facing direction ONLY WHEN MOVING
        // negative = left
        // positive = right
    {
        float playerDirection;

        // assign input, and use to determine which direciton player is facing, and rotate accordingly
        _playerFacingDirection = Input.GetAxis("Horizontal"); // GetAxis returns values between -1 & 0 (left) or between 0 & 1 (right)
        playerDirection = _playerFacingDirection;
        return playerDirection;

    }

}
