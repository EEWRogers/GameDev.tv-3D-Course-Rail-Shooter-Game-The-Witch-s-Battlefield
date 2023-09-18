using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("General Keybindings")]
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;

    [Header("General Tuning Variables")]
    [Tooltip("How fast player moves up and down based on input")]
    [SerializeField] float movementSpeed = 40f;

    [Tooltip("Level of dampening on player input")]
    [SerializeField] float smoothInputSpeed = 0.1f;

    [Tooltip("Distance ship can travel on x axis")]
    [SerializeField] float xRange = 16f;

    [Tooltip("Distance ship can travel on y axis")]
    [SerializeField] float yRange = 11.5f;


    [Header("Screen Position Based Tuning")]
    [Tooltip("Degree of x rotation of ship based on position")]
    [SerializeField] float positionPitchFactor = -2f;

    [Tooltip("Degree of y rotation of ship based on position")]
    [SerializeField] float positionYawFactor = 3f;


    [Header("Player Input Based Tuning")]
    [Tooltip("Degree of x rotation of ship based on player input")]
    [SerializeField] float controlPitchFactor = -25f;

    [Tooltip("Degree of z rotation of ship based on player input")]
    [SerializeField] float controlRollFactor = -20f;

    [Header("Laser Array")]
    [Tooltip("Add all child lasers here")]
    [SerializeField] GameObject[] lasers;

    float xThrow;
    float yThrow;

    Vector2 currentInputVector;
    Vector2 smoothInputVelocity;

    void Update()
    {
        SmoothController();
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void SmoothController()
    {
        Vector2 input = movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);

        xThrow = currentInputVector.x;
        yThrow = currentInputVector.y;
    }
    void ProcessTranslation()
    {
        float xOffset = xThrow * movementSpeed * Time.deltaTime; // makes raw x input framerate independant and tunes by movement speed
        float yOffset = yThrow * movementSpeed * Time.deltaTime; // makes raw y input framerate independant and tunes by movement speed

        float rawXPosition = transform.localPosition.x + xOffset; // gives raw value for where player should move on x
        float rawYPosition = transform.localPosition.y + yOffset; // gives raw value for where player should move on y

        float clampedXPosition = Mathf.Clamp(rawXPosition, -xRange, xRange); // restricts x movement to screen
        float clampedYPosition = Mathf.Clamp(rawYPosition, -yRange, yRange); // restricts y movement to screen

        transform.localPosition = new Vector3(clampedXPosition, clampedYPosition, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;

        float rollDueToControl = xThrow * controlRollFactor;
        
        float pitch = pitchDueToPosition + pitchDueToControl;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = rollDueToControl;
        
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring()
    {
        if (fire.ReadValue<float>() > 0.5)
        {
            SetLaserState(true);
        }
        else
        {
            SetLaserState(false);
        }
    }
    void SetLaserState(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    void OnEnable() 
    {
        movement.Enable();
        fire.Enable();
    }

    void OnDisable() 
    {
        movement.Disable();
        fire.Disable();
    }
}
