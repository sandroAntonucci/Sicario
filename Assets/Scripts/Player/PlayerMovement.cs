using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 0.1f;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset PlayerControls;

    [Header("Sliding")]
    [SerializeField] private float slideDuration = 1f; // Duration of the slide
    [SerializeField] private float slideSpeedMultiplier = 2f; // Speed multiplier during the slide
    [SerializeField] private float slideHeight = 0.5f; // Height of the character controller during the slide
    [SerializeField] private float slideCooldown = 2f; // Cooldown before the player can slide again
    [SerializeField] CameraStep cameraStep;

    private float originalSlideSpeedMultiplier;

    private float originalHeight; 
    private bool isSliding = false; 
    private float slideTimer = 0f; 
    private float slideCooldownTimer = 0f;

    private Vector3 targetMovement = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private float verticalVelocity;

    [SerializeField] GameObject cameraSlideHolder;

    private int lastPlayedIndex = -1;
    public bool isMoving;
    private float nextStepTime;
    private Vector3 currentMovement = Vector3.zero;
    private CharacterController characterController;

    private GameObject itemHolder;

    private InputAction moveAction;
    private InputAction sprintAction;

    private Vector2 moveInput;
    private InputAction slideAction;

    private PlayerRotate _rotate;
    private PlayerRotate _rotateSmooth;
    private PlayerRotate _currentRotate;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _rotate = GetComponents<PlayerRotate>()[0];
        _rotateSmooth = GetComponents<PlayerRotate>()[1];
        _currentRotate = _rotateSmooth;

#if UNITY_EDITOR
        _currentRotate = _rotate;
#else
        _currentRotate = _rotateSmooth;
#endif
    

        characterController = GetComponent<CharacterController>();
        originalHeight = cameraSlideHolder.transform.localPosition.y;
        originalSlideSpeedMultiplier = slideSpeedMultiplier;
        itemHolder = GameObject.FindGameObjectWithTag("ItemHolder");

        moveAction = PlayerControls.FindActionMap("Player").FindAction("Move");
        sprintAction = PlayerControls.FindActionMap("Player").FindAction("Sprint");
        slideAction = PlayerControls.FindActionMap("Player").FindAction("Slide");

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;

        slideAction.performed += ctx => StartSlide();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        slideAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        slideAction.Disable();
    }

    private void Update()
    {
        _currentRotate.Rotate();
        HandleMovement();
        //HandleFootsteps();
    }

    // Player movement
    private void HandleMovement()
    {
        if (isSliding)
        {
            
            slideTimer -= Time.deltaTime;
            slideSpeedMultiplier = originalSlideSpeedMultiplier * (slideTimer / slideDuration) * 2;
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
        else if (slideCooldownTimer > 0f)
        {
            slideCooldownTimer -= Time.deltaTime;
        }

        float speedMultiplier = sprintAction.ReadValue<float>() > 0.1f ? sprintMultiplier : 1f;

        if (isSliding)
        {
            speedMultiplier *= slideSpeedMultiplier;
        }

        targetMovement = new Vector3(moveInput.x, 0, moveInput.y) * walkSpeed * speedMultiplier;
        targetMovement = transform.rotation * targetMovement;

        // Movement acceleration / deceleration
        currentMovement.x = Mathf.Lerp(currentMovement.x, targetMovement.x, acceleration * Time.deltaTime);
        currentMovement.z = Mathf.Lerp(currentMovement.z, targetMovement.z, acceleration * Time.deltaTime);

        // Gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * 2;
        }

        currentMovement.y = verticalVelocity;
        characterController.Move(currentMovement * Time.deltaTime);
        isMoving = moveInput.sqrMagnitude > 0;

    }



    private void StartSlide()
    {
        if (!isSliding && slideCooldownTimer <= 0f && isMoving)
        {
            isSliding = true;
            slideTimer = slideDuration;

            if (this != null && gameObject != null)
            {
                StartCoroutine(LerpCameraHeight(originalHeight, slideHeight, 0.2f, true));

                if (CameraEffects.Instance != null)
                {
                    CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV * 1.2f, 0.2f));
                }
            }
        }
    }

    private void StopSlide()
    {
        if (isSliding)
        {
            isSliding = false;
            slideCooldownTimer = slideCooldown;
            slideSpeedMultiplier = originalSlideSpeedMultiplier;

            // Start a coroutine to smoothly transition back to the original height
            StartCoroutine(LerpCameraHeight(slideHeight, originalHeight, 0.2f, false)); // Adjust the duration (0.2f) as needed
            
            if (gameObject.GetComponent<PlayerAim>().isAiming)
            {
                CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV * 0.6f, 0.2f));
            }
            else
            {
                CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV, 0.2f));
            }
        }
    }

    private IEnumerator LerpCameraHeight(float fromHeight, float toHeight, float duration, bool deactivateBobbing)
    {
        float elapsedTime = 0f;

        if (deactivateBobbing)
        {
            cameraStep.enabled = false;
        }

        while (elapsedTime < duration)
        {
            // Interpolate the height
            float newHeight = Mathf.Lerp(fromHeight, toHeight, elapsedTime / duration);
            cameraSlideHolder.transform.position += new Vector3(0, newHeight - cameraSlideHolder.transform.localPosition.y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!deactivateBobbing)
        {
            cameraStep.enabled = true;
        }

        // Ensure the final height is set exactly
        cameraSlideHolder.transform.localPosition = new Vector3(0, toHeight, 0);
    }

    /*
    private void HandleFootsteps()
    {
        float currentStepInterval = sprintAction.ReadValue<float>() > 0 ? sprintStepInterval : walkStepInterval;

        if (characterController.isGrounded && isMoving && Time.time > nextStepTime && characterController.velocity.magnitude > velocityThreshold)
        {
            nextStepTime = Time.time + currentStepInterval;
            cameraMovement.StepCamera();
            //PlayFootstepSounds();
        }
    }*/
    

    /* Play a random footstep sound 
    private void PlayFootstepSounds()
    {

        int randomIndex;

        if (footstepSounds.Length == 1)
        {
            randomIndex = 0;
        }
        else
        {
            randomIndex = Random.Range(0, footstepSounds.Length-1);
            if (randomIndex >= lastPlayedIndex)
            {
                randomIndex = footstepSounds.Length - 1;
            }
        }

        lastPlayedIndex = randomIndex;
        footstepSource.clip = footstepSounds[randomIndex];
        footstepSource.Play();
    }
    */

}
