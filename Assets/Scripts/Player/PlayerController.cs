using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

<<<<<<< HEAD
<<<<<<< HEAD
public class PlayerController : MonoBehaviour, ISubscriber
=======
public class PlayerController : MonoBehaviour
>>>>>>> parent of f0a100f (no message)
=======
public enum State { MOVING, STANDING, DODGING, INTERACTING, ATTACKING, INVENTORY, stone ,strokeBack};

public class PlayerController : MonoBehaviour
=======
public class PlayerController : MonoBehaviour, ISubscriber
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
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
    public Vector3 InputDirection;
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
    private bool _isRotating = false;
    private float _cameraYAngle;
    const float FIRST = 0f;
    const float SECOND = 90f;
    const float THIRD = 180f;
    const float FOURTH = 270f;

    [Header("Dodge")]
    [SerializeField]
    private AnimationCurve _dodgeCurve;
    private bool _isDodging;
    private float _dodgeTimer;

    [SerializeField][Range(1f, 10f)]
    private float _dodgeDistance = 5f;
    [SerializeField][Range(0.1f, 2f)]
    private float _dodgeDuration = 1;
    [SerializeField][Range(0f, 1f)]
    private float _delayBeforeInvinsible = 0.2f;
    [SerializeField][Range(0f, 2f)]
    private float _invinsibleDuration = 1f;
    [SerializeField][Range(0f, 5f)]   
    private float _dodgeCooldown = 1;
    private bool _canDodge = true;
    private bool _isColliding = false;

    [Header("Combat/Equipment")]
    // Combat
    public List<AttackSO> Combo;
    public float AttackSpeed;
    private float _lastClickedTime;
    private float _lastComboEnd;
    private int _comboCounter;
    private float _windowUntilCanBuffer = 0.4f;
    private bool _bufferNextAttack = false;
    [SerializeField]
    private float _timeBetweenCombos = 0.2f;
    [SerializeField]
    private float _windowBetweenComboAttacks = 0.3f;
    public IEnumerator PetrifyCooldownCoroutine;

    // Equipment
    public enum Equipment { WEAPON, PICKAXE, AXE };
    private Equipment _currentEquipment;
    private int _currentTool;
    public Tool[] Tools;
    public GameObject ToolHolder;
    public IEnumerator Reset;

    [Header("Inventory")]
    [SerializeField]
    private Canvas _inventory;
    private bool _inInventory = false;

    [Header("Interact")]
    [SerializeField] private float _interactRange = 3f;
    // const float GOLDEN_RATIO = .54f;
    private bool _isCrafting = false;

    [Header("Animator")]
    // animation IDs
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private int _animIDAttackSpeed;
    
    // State
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
    public enum State {MOVING, STANDING, DODGING, INTERACTING, SWINGING, INVENTORY, PETRIFIED};
    private State _playerState;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0

    public Canvas _effectCanvas;
=======
    public enum State {MOVING, STANDING, DODGING, INTERACTING, SWINGING, INVENTORY};
    private State _playerState;
>>>>>>> parent of f0a100f (no message)


    // Start is called before the first frame update
    void Awake()
    {;
        // Interact range
        GetComponentInChildren<SphereCollider>().radius = _interactRange;
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa

        // Initialize states
=======
>>>>>>> parent of f0a100f (no message)
        _playerState = State.STANDING;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
        _currentEquipment = Equipment.WEAPON;

        // set current tool as weapon
        _currentTool = 0;

        // Inital dodge setup
        Keyframe dodge_lastFrame = _dodgeCurve[_dodgeCurve.length - 1];
        _dodgeTimer = dodge_lastFrame.time;

        // Set up camera
        _playerCamera = GameObject.FindGameObjectWithTag("VirtualCamera").transform;
        _cameraYAngle = FIRST;
        _playerCamera.rotation = Quaternion.Euler(_playerCamera.localEulerAngles.x, _cameraYAngle, _playerCamera.localEulerAngles.z);

        // Set up animator
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
        _animator.SetFloat(_animIDAttackSpeed, AttackSpeed);

        // Assing Character controller
        _controller = GetComponent<CharacterController>();

        _effectCanvas.enabled = false;
    }


    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAttackSpeed = Animator.StringToHash("AttackSpeed");
    }

    void Update()
    {
        void Update()
    {
        attaclkTimer += Time.deltaTime;
        switch (_playerState)
        {
            case State.stone:
                break;
            case State.STANDING:
               
            {
                    Attack();
                HandleMovement();
                HandleInteract();
                HandleDodge();
                RotateCamera();
                ToggleInventory();
<<<<<<< HEAD
                LookAtMouse();
=======
                HandleEquipedItemChange();
<<<<<<< HEAD
<<<<<<< HEAD
=======
                //LookAtMouse();
>>>>>>> parent of f0a100f (no message)
=======
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
                break;
            }
            case State.MOVING:
            {
                    Attack();
                    HandleMovement();
                HandleInteract();
                HandleDodge();
                RotateCamera();
                ToggleInventory();
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
                //击退状态
            case State.strokeBack:
                {
                    transform.position = Vector3.MoveTowards(transform.position, strokeBackTargetPosition,5*Time.deltaTime);
                    if (Vector3.Distance(transform.position, strokeBackTargetPosition)<0.2f)
                    {
                     
                        _playerState = State.MOVING;
                    }
                    break;
                }
=======
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
            case State.PETRIFIED:
            {
                break;
            }
<<<<<<< HEAD
=======
>>>>>>> parent of f0a100f (no message)
=======
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
        }
    }
    }

    public void SetState(State state)
    {
        _playerState = state;
    }

    public Equipment GetCurrentEquipment()
    {
        return _currentEquipment;
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
        InputDirection = new Vector3(InputManager.instance.MoveInput.x, 0.0f, InputManager.instance.MoveInput.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (InputManager.instance.MoveInput != Vector2.zero)
        {
            _playerState = State.MOVING;
            _targetRotation = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg + _playerCamera.transform.eulerAngles.y;
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
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, 0, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (_animator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    #endregion

    #region - Combat/Equipment -
    private void HandleClick()
    {
        if (_currentEquipment == Equipment.WEAPON)
        {
            if (Time.time - _lastComboEnd > _timeBetweenCombos && _comboCounter < Combo.Count && (InputManager.instance.AttackInput || _bufferNextAttack))
            {
                CancelInvoke("EndCombo");
                //_stateBeforeAttacking = (_playerState != State.SWINGING) ? _playerState : _stateBeforeAttacking;
                _playerState = State.SWINGING;


                // check for click in the buffer window
                if (Time.time - _lastClickedTime > _windowUntilCanBuffer && InputManager.instance.AttackInput && Time.time - _lastClickedTime < _windowBetweenComboAttacks)
                {
                    _bufferNextAttack = true;
                }

                //Debug.Log(Combo[_comboCounter].AttackLength);
                if (Time.time - _lastClickedTime >= _windowBetweenComboAttacks || (_bufferNextAttack && Time.time - _lastClickedTime >= _windowBetweenComboAttacks))
                {
                    FireAttack();
                    _bufferNextAttack = false;
                }

            }
        }

        // If pickaxe equiped
        if (_playerState != State.SWINGING && _currentEquipment == Equipment.PICKAXE && InputManager.instance.AttackInput)
        {
            _playerState = State.SWINGING;
            _animator.SetTrigger("isMining");
            _animator.SetFloat("Speed", 0);
            Reset = ResetStateAfterSeconds(2.4f / AttackSpeed);
            StartCoroutine(Reset);
        }

        // If axe equiped
        if (_playerState != State.SWINGING && _currentEquipment == Equipment.AXE && InputManager.instance.AttackInput)
        {
            _playerState = State.SWINGING;
            _animator.SetTrigger("isChopping");
            _animator.SetFloat("Speed", 0);
            Reset = ResetStateAfterSeconds(2.4f/AttackSpeed);
            StartCoroutine(Reset);
        }
    }

    private void FireAttack()
    {
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
        _playerState = State.STANDING;
        _bufferNextAttack = false;
    }

    IEnumerator ResetStateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("Coroutine");
        _playerState = State.STANDING;
    }

    public void BeginCollison()
    {
        Tools[_currentTool].BeginCollision();
    }

    public void EndCollision()
    {
        Tools[_currentTool].EndCollision();
    }

    public void BeginTrail()
    {
        Tools[_currentTool].BeginTrail();
    }
    public void EndTrail()
    {
        Tools[_currentTool].EndTrail();
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
        if (InputManager.instance.DodgeInput && InputDirection != Vector3.zero)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            StartCoroutine(Dodge());
=======
            GetComponent<Health>().Invinsible(_delayBeforeInvinsible, _invinsibleDuration);
            StartCoroutine(Dodge(transform.position + ConvertToCameraSpace(direction) * _dodgeDistance));
            StartCoroutine(DodgeCooldown());
>>>>>>> parent of f0a100f (no message)
        }

        

        /*if (InputManager.instance.DodgeInput && _canDodge && direction != Vector3.zero)
        {
            GetComponent<PlayerHealth>().Invinsible(_delayBeforeInvinsible, _invinsibleDuration);
=======
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
            StartCoroutine(Dodge(transform.position + ConvertToCameraSpace(direction) * _dodgeDistance));
            StartCoroutine(DodgeCooldown());
=======
            StartCoroutine(Dodge());
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
        }
    }

    IEnumerator Dodge()
    {
        _animator.SetTrigger("isDodging");
        _playerState = State.DODGING;
        _isDodging = true;
        float timer = 0f;

        while (timer < _dodgeTimer)
        {
            float speed = _dodgeCurve.Evaluate(timer);
            _controller.Move(InputDirection * Time.deltaTime * speed);
            timer += Time.deltaTime;
            yield return null;
        }

        _isDodging = false;
        _playerState = State.STANDING;
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

    static float Flip(float x)
    {
        return 1 - x;
    }

    // used with lerp fucntion, changes lerp from linear to ease out curve
    public static float EaseOut(float t)
    {
        return Flip(Flip(t) * Flip(t));
    }

    IEnumerator DodgeCooldown()
    {
        _canDodge = false;
        yield return new WaitForSeconds(_dodgeCooldown + _dodgeDuration);
        _canDodge = true;
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

    #region - Equipment -

    private void HandleEquipedItemChange()
    {
        
        if (InputManager.instance.ScrollInput > 0)
        {
            if (_currentEquipment == Equipment.WEAPON)
            {
                _currentEquipment = Equipment.PICKAXE;
                ToolHolder.transform.GetChild(0).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(1).gameObject.SetActive(true);
                _currentTool = 1;
            }
            else if (_currentEquipment == Equipment.PICKAXE)
            {
                _currentEquipment = Equipment.AXE;
                ToolHolder.transform.GetChild(1).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(2).gameObject.SetActive(true);
                _currentTool = 2;
            }
            else if (_currentEquipment == Equipment.AXE)
            {
                _currentEquipment = Equipment.WEAPON;
                ToolHolder.transform.GetChild(2).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(0).gameObject.SetActive(true);
                _currentTool = 0;
            }
            //Debug.Log(_currentEquipment);
            //Debug.Log(InputManager.instance.ScrollInput);
        }
        if (InputManager.instance.ScrollInput < 0)
        {
            if (_currentEquipment == Equipment.WEAPON)
            {
                _currentEquipment = Equipment.AXE;
                ToolHolder.transform.GetChild(0).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(2).gameObject.SetActive(true);
                _currentTool = 2;
            }
            else if (_currentEquipment == Equipment.PICKAXE)
            {
                _currentEquipment = Equipment.WEAPON;
                ToolHolder.transform.GetChild(1).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(0).gameObject.SetActive(true);
                _currentTool = 0;
            }
            else if (_currentEquipment == Equipment.AXE)
            {
                _currentEquipment = Equipment.PICKAXE;
                ToolHolder.transform.GetChild(2).gameObject.SetActive(false);
                ToolHolder.transform.GetChild(1).gameObject.SetActive(true);
                _currentTool = 1;
            }
            //Debug.Log(_currentEquipment);
            //Debug.Log(InputManager.instance.ScrollInput);
        }
    }

    #endregion

    #region ISubscriber
    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Frequency"))
        {
            _effectCanvas.enabled = true;
        }
        if (channel.Equals("Petrified"))
        {
            PetrifyCooldownCoroutine = PetrifyCooldown(2f);
            StartCoroutine(PetrifyCooldownCoroutine);
        }
        else
        {
            _effectCanvas.enabled = false;
        }
    }

    IEnumerator PetrifyCooldown(float seconds)
    {
        _playerState = State.PETRIFIED;
        _animator.enabled = false;
        yield return new WaitForSeconds(seconds);
        _playerState = State.STANDING;
        _animator.enabled = true;
    }
    #endregion
}
