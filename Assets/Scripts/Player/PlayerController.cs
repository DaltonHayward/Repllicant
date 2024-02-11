using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Transform _playerCamera;
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    private CharacterController _controller;
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Header("Camera Rotation")]
    [SerializeField][Range(0.1f, 5f)]
    private float _rotationSpeed = 1;
    private Vector3 _lookDirection;
    private Quaternion _rotationGoal;
    private bool _isRotating = false;
    private float _cameraYAngle;
    const float FIRST = 0f;
    const float SECOND = 90f;
    const float THIRD = 180f;
    const float FOURTH = 270f;
    

    [Header("Dodge")]
    [SerializeField][Range(1f, 10f)]
    private float _dodgeDistance = 5f;
    [SerializeField][Range(0.1f, 2f)]
    private float _dodgeDuration = 1;
    private Vector3 _previousPos;
    [SerializeField][Range(0f, 1f)]
    private float _delayBeforeInvinsible = 0.2f;
    [SerializeField][Range(0f, 2f)]
    private float _invinsibleDuration = 1f;
    [SerializeField][Range(0f, 5f)]   
    private float _dodgeCooldown = 1;
    private bool _isDodging = false;
    private bool _isColliding = false;

    [Header("Combat")]
    public List<AttackSO> Combo;
    public float AttackSpeed;
    private float _lastClickedTime;
    private float _lastComboEnd;
    private int _comboCounter = 0;

    [SerializeField]
    private float _timeBetweenCombos = 0.2f;
    [SerializeField]
    private float _windowBetweenComboAttacks = 0.3f;
    private State _stateBeforeAttacking;

    [Header("Inventory")]
    [SerializeField]
    private Canvas _inventory;
    private bool _inInventory = false;

    [SerializeField] private float _interactRange = 3f;
    // const float GOLDEN_RATIO = .54f;
    private bool _isCrafting = false;

    // animation IDs
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private int _animIDAttackSpeed;

    public enum State {MOVING, STANDING, DODGING, INTERACTING, ATTACKING, INVENTORY};
    private State _playerState;


    // Start is called before the first frame update
    void Awake()
    {;
        GetComponentInChildren<SphereCollider>().radius = _interactRange;
        _playerState = State.STANDING;
        _previousPos = transform.position;
        _playerCamera = GameObject.FindGameObjectWithTag("VirtualCamera").transform;
        _cameraYAngle = FIRST;
        _playerCamera.rotation = Quaternion.Euler(_playerCamera.localEulerAngles.x, _cameraYAngle, _playerCamera.localEulerAngles.z);
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        AssignAnimationIDs();

        _animator.SetFloat(_animIDAttackSpeed, AttackSpeed);
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAttackSpeed = Animator.StringToHash("AttackSpeed");
    }

    void Update()
    {
        switch (_playerState)
        {
            case State.STANDING:
            {
                HandleMovement();
                HandleInteract();
                HandleDodge();
                HandleAttack();
                RotateCamera();
                ToggleInventory();
                //LookAtMouse();
                break;
            }
            case State.MOVING:
            {
                HandleMovement();
                HandleInteract();
                HandleAttack();
                HandleDodge();
                RotateCamera();
                ToggleInventory();
                break;
            }
            case State.ATTACKING:
            {
                HandleAttack();
                break;
            }
            case State.DODGING:
            {
                break;
            }
            case State.INTERACTING:
            {
                HandleInteract();
                break;
            }
            case State.INVENTORY:
            {
                ToggleInventory();
                break;
            }
        }
        ExitAttack();
    }

    #region - Movement -

    private void HandleMovement() 
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = InputManager.instance.SprintInput ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (InputManager.instance.MoveInput == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        //float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(InputManager.instance.MoveInput.x, 0.0f, InputManager.instance.MoveInput.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (InputManager.instance.MoveInput != Vector2.zero)
        {
            _playerState = State.MOVING;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            _playerState = State.STANDING;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, 0, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (_animator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    #endregion

    #region - Combat -
    private void HandleAttack()
    {
        if (Time.time - _lastComboEnd > _timeBetweenCombos && _comboCounter < Combo.Count && InputManager.instance.AttackInput)
        {
            CancelInvoke("EndCombo");
            _stateBeforeAttacking = (_playerState != State.ATTACKING) ? _playerState : _stateBeforeAttacking;
            _playerState = State.ATTACKING;

            // if a click happens 70% of the way throug an attack animation
            if (Time.time - _lastClickedTime >= (Combo[_comboCounter].AttackLength()/AttackSpeed * 0.7f))
            {
                Debug.Log(_comboCounter + " " + Combo[_comboCounter].AttackLength() + " " + (Combo[_comboCounter].AttackLength() / AttackSpeed * 0.7f));
                // overide current attack animation
                _animator.runtimeAnimatorController = Combo[_comboCounter].AnimatorOV;
                // play new animation
                _animator.Play("Attack", 0, 0);

                // handle dmg, visual effect-----------------

                _comboCounter++;
                _lastClickedTime = Time.time;

                if (_comboCounter > Combo.Count)
                {
                    _comboCounter = 0;
                }
            }
            
        }
    }

    void ExitAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 0);
        }
    }

    void EndCombo()
    {
        _comboCounter = 0;
        _lastComboEnd = Time.time;
        _playerState = _stateBeforeAttacking;
    }

    #endregion

    #region - Interact -
    private void HandleInteract()
    {
        if (InputManager.instance.InteractInput)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, _interactRange);
   
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Interactable"))
                {
                    // check for subscriber interface on collided object
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                    // send approriate signal if they are a subscriber
                    if (subscriber != null && !_isCrafting)
                    {
                        subscriber.ReceiveMessage("OpenMenu");
                        _isCrafting = true;
                        _playerState = State.INTERACTING;
                        break;
                    }
                    else if (subscriber != null && _isCrafting)
                    {
                        subscriber.ReceiveMessage("CloseMenu");
                        _isCrafting = false;
                        _playerState = State.MOVING;
                        break;
                    }
                }
            }
        }
    }

    #endregion

    #region - Dodge -
    // This will need to be changed to work with a character controller
    private void HandleDodge() 
    {
        Vector3 direction = new Vector3(InputManager.instance.MoveInput.x, transform.position.y, InputManager.instance.MoveInput.y);

        if (InputManager.instance.DodgeInput && !_isDodging && direction != Vector3.zero)
        {
            GetComponent<Health>().Invinsible(_delayBeforeInvinsible, _invinsibleDuration);
            StartCoroutine(Dodge(transform.position + ConvertToCameraSpace(direction) * _dodgeDistance));
            StartCoroutine(DodgeCooldown());
        }

        _previousPos = transform.position;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Ground"))
        {
            _isColliding = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
         if (!other.collider.CompareTag("Ground"))
        {
            _isColliding = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.yellow);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable")) 
        {
            other.GetComponent<Renderer>().material.SetColor("_OutlineColor", other.GetComponent<CraftingTable>().GetOriginalOutline());
        }
    }

    static float Flip(float x)
    {
        return 1 - x;
    }

    // used with lerp fucntion, changes lerp from linear to ease out curve
    public static float EaseOut(float t)
    {
        return Flip(Flip(t) * Flip(t));
    }

    // allows dodge to take place outside of update loop, moves the player from one position to another specified position
    IEnumerator Dodge(Vector3 newPosition)
    {
        _playerState = State.DODGING;
        Rigidbody rb = GetComponent<Rigidbody>();

        float elapsedTime = 0f;
        float ratio = elapsedTime / _dodgeDuration;
        
        while(elapsedTime < _dodgeDuration && !_isColliding)
        {
            float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / _dodgeDuration);

            rb.MovePosition(Vector3.Lerp(transform.position, newPosition, ratio));
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / _dodgeDuration;

            yield return null;
        }

        yield return new WaitForSeconds(_dodgeDuration - elapsedTime);

        _playerState = State.MOVING;
    }

    IEnumerator DodgeCooldown()
    {
        _isDodging = true;
        yield return new WaitForSeconds(_dodgeCooldown + _dodgeDuration);
        _isDodging = false;
    }

    #endregion

    #region - Camera -

    // not needed anymore
    public Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) 
    {
        // store current Y value from original vector
        float currentYValue = vectorToRotate.y;

        // store forward and right of player camera
        Vector3 cameraForward = _playerCamera.forward;
        Vector3 cameraRight = _playerCamera.right;

        // ignore upward/downwards camera angles
        cameraForward.y = 0;
        cameraRight.y = 0;

        // re-normalize to a magnitude of 1
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // rotate vectorToRotate to camera space
        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        // return sum of both products in camera space
        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    // Logic for rotating the camera based on current rotation and user input
    private void RotateCamera() 
    {
        if (!_isRotating && (InputManager.instance.CameraLeftInput || InputManager.instance.CameraRightInput))
        {
            switch (_cameraYAngle)
            {
                case FIRST:
                    if (InputManager.instance.CameraLeftInput)
                    {
                        _cameraYAngle = FOURTH;
                    }
                    else if (InputManager.instance.CameraRightInput)
                    {
                        _cameraYAngle = SECOND;
                    }
                    break;
                case SECOND:
                    if (InputManager.instance.CameraLeftInput)
                    {
                        _cameraYAngle = FIRST;
                    }
                    else if (InputManager.instance.CameraRightInput)
                    {
                        _cameraYAngle = THIRD;
                    }
                    break;
                case THIRD:
                    if (InputManager.instance.CameraLeftInput)
                    {
                        _cameraYAngle = SECOND;
                    }
                    else if (InputManager.instance.CameraRightInput)
                    {
                        _cameraYAngle = FOURTH;
                    }
                    break;
                case FOURTH:
                    if (InputManager.instance.CameraLeftInput)
                    {
                        _cameraYAngle = THIRD;
                    }
                    else if (InputManager.instance.CameraRightInput)
                    {
                        _cameraYAngle = FIRST;
                    }
                    break;
            }

            Debug.Log(_cameraYAngle);

            StartCoroutine(LerpRotation(_cameraYAngle));
        }
        
    }

    // Smooth rotation of camera
    IEnumerator LerpRotation(float cameraYAngle)
    {
        _isRotating = true;

        float elapsedTime = 0f;
        float ratio = elapsedTime / _rotationSpeed;

        while(elapsedTime <= _rotationSpeed)
        {
            _playerCamera.rotation = Quaternion.Lerp(_playerCamera.rotation, Quaternion.Euler(_playerCamera.localEulerAngles.x, cameraYAngle, _playerCamera.localEulerAngles.z), ratio);
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / _rotationSpeed;

            yield return null;
        }

        _isRotating = false;
    }

    // 360 rotation of the player towards the mouse position
    public void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // create a ray from the mouse position of screen to a world point
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity); // cast the ray through all objects
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag == "Ground")
            {   // check that the ray collides with the ground and only the ground
                transform.LookAt(hits[i].point); // Look at the point
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation
            }
        }
    }

    #endregion

    #region - Inventory -
    public void ToggleInventory()
    {
        if (InputManager.instance.InventoryInput)
        {
            if (_inInventory)
            {
                _inInventory = false;
                _inventory.enabled = false;
                _playerState = State.STANDING;
            }
            else
            {
                _inInventory = true;
                _inventory.enabled = true;
                _playerState = State.INVENTORY;
                StartCoroutine(SlowDown());
                
            }
        }
    }

    IEnumerator SlowDown()
    {
        float elapsedTime = 0f;
        float ratio = elapsedTime / 0.2f;

        while (_animator.GetFloat(_animIDSpeed) > 0.01f)
        {
            _animator.SetFloat(_animIDSpeed, Mathf.Lerp(_animator.GetFloat(_animIDSpeed), 0, ratio));
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / _dodgeDuration;

            yield return null;
        }

        _animator.SetFloat(_animIDSpeed, 0);
    }

    #endregion
}
