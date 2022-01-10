using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Player")]

    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    private FirstPersonInputs input;

    private CharacterController characterController;

    private void Start()
    {
        input = GetComponent<FirstPersonInputs>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(input.Move == Vector2.zero)
            return;

        var targetDirection = new Vector3(
            input.Move.x,
            0f,
            input.Move.y
        );

        characterController.Move(targetDirection * MoveSpeed * Time.deltaTime);
    }
}
