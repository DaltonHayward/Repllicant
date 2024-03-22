using ReplicantPackage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;
using FMOD.Studio;

public class PlayerController : MonoBehaviour, ISubscriber

{
    // Coupled EffectableObject script here so that effects can be applied to the player
    protected EffectableObject Effectable;

    private EventInstance FootstepsGrass;
    private Transform _playerCamera;
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float Speed;
    public float RotationSmoothTime = 0.12f;
    public Vector3 InputDirection;
    private CharacterController _controller;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Header("Camera Rotation")]
    [SerializeField]
    [Range(0.1f, 5f)]
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
    private bool _canDodge = true;
    private float _dodgeDuration;
    private float _dodgeCooldown = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _delayBeforeInvinsible = 0.2f;
    [SerializeField]
    [Range(0f, 2f)]
    private float _invinsibleDuration = 1f;
    [SerializeField]
    [Range(0f, 5f)]

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

    public Vector3 strokeBackTargetPosition;

    // for hurt/petrify color changes
    private Color _originalMaterialColor;
    private SkinnedMeshRenderer meshRenderer;

    // Equipment
    public enum Equipment { WEAPON, PICKAXE, AXE };
    private Equipment _currentEquipment;
    private int _currentTool;
    private EquippedTool[] _tools;
    public Transform ToolHolder;
    private bool _canScroll = true;
    [SerializeField]
    private float _scrollDelay;
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
    public enum State { MOVING, STANDING, DODGING, INTERACTING, SWINGING, INVENTORY, PETRIFIED, KNOCKBACK, CHARMED, DIALOG, CRAFTING, PAUSED };
    [SerializeField]
    public State _playerState;

    public Canvas _effectCanvas;


    // Start is called before the first frame update
    void Awake()
    { 
        // retrieve effects
        Effectable = GetComponent<EffectableObject>();

        // Interact range
        GetComponentInChildren<SphereCollider>().radius = _interactRange;

        // Initialize states
        _playerState = State.STANDING;
        _currentEquipment = Equipment.WEAPON;

        // set current Tool as weapon and initalize tools array
        _currentTool = 0;
        _tools = new EquippedTool[3];

        // set base color
        meshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _originalMaterialColor = meshRenderer.materials[0].GetColor("_BaseColor");

        // Inital dodge setup
        Keyframe dodge_lastFrame = _dodgeCurve[_dodgeCurve.length - 1];
        _dodgeDuration = dodge_lastFrame.time;

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

    public void Start()
    {
        FootstepsGrass = AudioManager.instance.CreateInstance(FMODEvents.instance.footStepsGrass);
        // initalize tools
        _tools[0] = ToolHolder.GetChild(0).GetComponentInChildren<EquippedTool>();
        _tools[1] = ToolHolder.GetChild(1).GetComponentInChildren<EquippedTool>();
        _tools[2] = ToolHolder.GetChild(2).GetComponentInChildren<EquippedTool>();
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
            case State.MOVING:
                HandleMovement();
                HandleInteract();
                HandleDodge();
                HandleClick();
                RotateCamera();
                ToggleInventory();
                HandleEquipedItemChange();
                HandleMenuPress();
                break;

            case State.SWINGING:
                HandleClick();
                break;

            case State.DODGING:
                break;

            case State.INTERACTING:
                HandleInteract();
                HandleMenuPress();
                ToggleInventory();
                break;

            case State.INVENTORY:
                ToggleInventory();
                HandleMenuPress();
                break;

            case State.PETRIFIED:
                break;

            case State.KNOCKBACK:
                Knockback();
                break;

            case State.CHARMED:
                break;

            case State.DIALOG:
            case State.CRAFTING:
            case State.PAUSED:
                HandleMenuPress();
                break;

        }
        ExitAttack();
    }

    public void SetState(State state)
    {
        _playerState = state;
    }

    #region - Movement -

    private void HandleMovement()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        // addition here to have Effectable affect move speed
        float targetSpeed = InputManager.instance.SprintInput ? SprintSpeed : Effectable.Effect_MovementSpeed(MoveSpeed);

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
            Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            Speed = Mathf.Round(Speed * 1000f) / 1000f;
        }
        else
        {
            Speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        InputDirection = new Vector3(InputManager.instance.MoveInput.x, 0.0f, InputManager.instance.MoveInput.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (InputManager.instance.MoveInput != Vector2.zero)
        {
            if (_playerState != State.DODGING)
                _playerState = State.MOVING;
            _targetRotation = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg + _playerCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            if (_playerState != State.DODGING)
                _playerState = State.STANDING;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) + new Vector3(0.0f, 0, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (_animator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    public void MoveTowardsTarget(Vector3 target)
    {
        var offset = target - transform.position;

        //Get the difference.
        if (offset.magnitude > 3f)
        {

            _targetRotation = Mathf.Atan2(target.x, target.z) * Mathf.Rad2Deg + _playerCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.

            offset = offset.normalized * MoveSpeed;
            //normalize it and account for movement speed.
            Speed = MoveSpeed;
            _controller.Move(offset * Time.deltaTime);
            //actually move the character.

            if (_animator)
            {
                _animator.SetFloat(_animIDSpeed, Speed);
                _animator.SetFloat(_animIDMotionSpeed, 1f);
            }
        }
        else
        {
            if (_animator)
            {
                _animator.SetFloat(_animIDSpeed, 0);
                _animator.SetFloat(_animIDMotionSpeed, 1f);
            }
        }
    }

    #endregion

    #region - Combat/Equipment -
    private void HandleClick()
    {
        if (_tools[_currentTool] == null) { return; }

        if (_currentEquipment == Equipment.WEAPON)
        {
            if (Time.time - _lastComboEnd > _timeBetweenCombos && _comboCounter < Combo.Count && (InputManager.instance.AttackInput || _bufferNextAttack))
            {
                CancelInvoke("EndCombo");
                //_stateBeforeAttacking = (_playerState != State.SWINGING) ? _playerState : _stateBeforeAttacking;
                _playerState = State.SWINGING;


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
            Reset = ResetStateAfterSeconds(2.4f / AttackSpeed);
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
        _playerState = State.STANDING;
    }

    public void BeginCollison()
    {
        _tools[_currentTool].BeginCollision();
    }

    public void EndCollision()
    {
        _tools[_currentTool].EndCollision();
    }

    public void BeginTrail()
    {
        _tools[_currentTool].BeginTrail();
    }
    public void EndTrail()
    {
        _tools[_currentTool].EndTrail();
    }

    private void Knockback()
    {
        if (!GetComponent<PlayerHealth>().isInvincible())
        {
            transform.position = Vector3.MoveTowards(transform.position, strokeBackTargetPosition, 30 * Time.deltaTime);
            if (Vector3.Distance(transform.position, strokeBackTargetPosition) < 0.2f)
            {
                _playerState = State.STANDING;
            }
        }  
    }

    #endregion

    #region - Interact -
    private void HandleInteract()
    {
        if (InputManager.instance.InteractInput)
        {
            if (!_isCrafting)
            {
                _isCrafting = true;
            }
            else
            {
                _isCrafting = false;
            }        
        }
    }

    #endregion

    #region - Dodge -
    // This will need to be changed to work with a character controller
    private void HandleDodge()
    {
        if (InputManager.instance.DodgeInput && InputDirection != Vector3.zero && _canDodge)
        {
            StartCoroutine(Dodge());
            GetComponent<PlayerHealth>().Invincible(_delayBeforeInvinsible, _invinsibleDuration);
            StartCoroutine(DodgeCooldown());
        }
    }

    IEnumerator Dodge()
    {
        _playerState = State.DODGING;
        float timer = 0f;
        _animator.SetTrigger("isDodging");

        Vector3 dir = transform.forward;

        while (timer < _dodgeDuration)
        {
            float speed = _dodgeCurve.Evaluate(timer);
            _controller.Move((dir * speed* MoveSpeed) * Time.deltaTime);
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        Debug.Log("HerE");
        _playerState = State.STANDING;
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

        while (elapsedTime <= _rotationSpeed)
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

                if (DropdownController.instance.isActiveAndEnabled)
                {
                    InventoryController.instance.HideContextMenu();
                }
                    
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

    public IEnumerator SlowDown()
    {
        float elapsedTime = 0f;
        float ratio = elapsedTime / 0.2f;

        while (_animator.GetFloat(_animIDSpeed) > 0.01f)
        {
            _animator.SetFloat(_animIDSpeed, Mathf.Lerp(_animator.GetFloat(_animIDSpeed), 0, ratio));
            elapsedTime += Time.unscaledDeltaTime;
            ratio = elapsedTime / 0.2f;

            yield return null;
        }

        _animator.SetFloat(_animIDSpeed, 0);
    }

    #endregion

    #region - Equipment -
    private void HandleEquipedItemChange()
    {
        if (InputManager.instance.ScrollInput < 0 && _canScroll)
        {
            StartCoroutine(ScrollDelay());
            switch (_currentEquipment)
            {
                case Equipment.WEAPON:
                    _currentEquipment = Equipment.PICKAXE;
                    ActivatePickaxeSlot();
                    break;

                case Equipment.PICKAXE:
                    _currentEquipment = Equipment.AXE;
                    ActivateAxeSlot();
                    break;

                case Equipment.AXE:
                    _currentEquipment = Equipment.WEAPON;
                    ActivateWeaponSlot();
                    break;
            }
        }
        if (InputManager.instance.ScrollInput > 0 && _canScroll)
        {
            StartCoroutine(ScrollDelay());
            switch (_currentEquipment)
            {
                case Equipment.WEAPON:
                    _currentEquipment = Equipment.AXE;
                    ActivateAxeSlot();
                    break;

                case Equipment.PICKAXE:
                    _currentEquipment = Equipment.WEAPON;
                    ActivateWeaponSlot();
                    break;

                case Equipment.AXE:
                    _currentEquipment = Equipment.PICKAXE;
                    ActivatePickaxeSlot();
                    break; 
            }
        }
    }

    IEnumerator ScrollDelay()
    {
        _canScroll = false;
        yield return new WaitForSeconds(_scrollDelay);
        _canScroll = true;
    }

    public void ActivateWeaponSlot()
    {
        ToolHolder.GetChild(0).gameObject.SetActive(true);
        ToolHolder.GetChild(1).gameObject.SetActive(false);
        ToolHolder.GetChild(2).gameObject.SetActive(false);
        _currentTool = 0;
    }

    public void ActivatePickaxeSlot()
    {
        ToolHolder.GetChild(0).gameObject.SetActive(false);
        ToolHolder.GetChild(1).gameObject.SetActive(true);
        ToolHolder.GetChild(2).gameObject.SetActive(false);
        _currentTool = 1;
    }

    public void ActivateAxeSlot()
    {
        ToolHolder.GetChild(0).gameObject.SetActive(false);
        ToolHolder.GetChild(1).gameObject.SetActive(false);
        ToolHolder.GetChild(2).gameObject.SetActive(true);
        _currentTool = 2;
    }

    public void EquipTool(Equipment type, Inventory_Item tool)
    {
        GameObject newTool;
        switch (type)
        {
            case Equipment.WEAPON:
                Transform weaponHolder = ToolHolder.GetChild(0);
                if (weaponHolder.childCount != 0)
                {
                    Destroy(weaponHolder.GetChild(0).gameObject);
                }   

                newTool = Instantiate(tool.itemData.equippedModel, weaponHolder);
                newTool.GetComponent<EquippedTool>().SetInvTool(tool.GetComponent<InvTool>());
                _tools[0] = newTool.GetComponent<EquippedTool>();

                break;

            case Equipment.PICKAXE:
                Transform pickaxeHolder = ToolHolder.GetChild(1);
                if (pickaxeHolder.childCount != 0)
                {
                    Destroy(pickaxeHolder.GetChild(0).gameObject);
                }

                newTool = Instantiate(tool.itemData.equippedModel, pickaxeHolder);
                newTool.GetComponent<EquippedTool>().SetInvTool(tool.GetComponent<InvTool>());
                _tools[1] = newTool.GetComponent<EquippedTool>();

                break;

            case Equipment.AXE:
                Transform axeHolder = ToolHolder.GetChild(2);
                if (axeHolder.childCount != 0)
                {
                    Destroy(axeHolder.GetChild(0).gameObject);
                }

                newTool = Instantiate(tool.itemData.equippedModel, axeHolder);
                newTool.GetComponent<EquippedTool>().SetInvTool(tool.GetComponent<InvTool>());
                _tools[2] = newTool.GetComponent<EquippedTool>();
                break;
        }
    }

    public void UnequipTool(Equipment type)
    {
        switch (type)
        {
            case Equipment.WEAPON:
                Transform weaponHolder = ToolHolder.GetChild(0);
                if (weaponHolder.childCount == 0)
                {
                    return;
                }

                Destroy(weaponHolder.GetChild(0).gameObject);
                _tools[0] = null;

                break;

            case Equipment.PICKAXE:
                Transform pickaxeHolder = ToolHolder.GetChild(1);
                if (pickaxeHolder.childCount == 0)
                {
                    return;
                }

                Destroy(pickaxeHolder.GetChild(0).gameObject);
                _tools[1] = null;

                break;

            case Equipment.AXE:
                Transform axeHolder = ToolHolder.GetChild(2);
                if (axeHolder.childCount == 0)
                {
                    return;
                }

                Destroy(axeHolder.GetChild(0).gameObject);
                _tools[2] = null;

                break;
        }
    }

    public EquippedTool[] GetTools()
    {
        return _tools;
    }

    #endregion

    #region - MenuInput -

    public void HandleMenuPress()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            bool isCurrentlyPaused = GameManager.instance.isPaused;
            switch (_playerState)
            {
                case State.MOVING:
                case State.STANDING:
                    if (!isCurrentlyPaused)
                    {
                        MenuManager.instance.OpenMainMenu();
                        GameManager.instance.Pause();
                        _playerState = State.PAUSED;
                    }
                    break;

                case State.PAUSED:
                    GameManager.instance.UnPause();
                    MenuManager.instance.CloseAllMenus();
                    _playerState = State.STANDING;
                    break;

                case State.INTERACTING:
                    
                    break;

                case State.INVENTORY:
                    _inInventory = false;
                    _inventory.enabled = false;

                    if (DropdownController.instance.isActiveAndEnabled)
                    {
                        InventoryController.instance.HideContextMenu();
                    }

                    _playerState = State.STANDING;
                    break;

                case State.DIALOG:
                    // sets player state to standing in coroutine
                    StartCoroutine(DialogueManager.instance.ExitDialogueMode());
                    break;

                case State.CRAFTING:
                    CraftingManager.instance.ExitCraftingMode();
                    _playerState = State.STANDING;
                    break;
            }
            
        }
    }

    #endregion

    #region - ISubscriber -
    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Frequency"))
        {
            _effectCanvas.enabled = true;
        }
        else
        {
            _effectCanvas.enabled = false;
        }

        if (channel.Equals("Petrified"))
        {
            Debug.Log("petrified");
            _playerState = State.PETRIFIED;
            PetrifyCooldownCoroutine = PetrifyCooldown(2f);
            //GetComponent<Charmable>().ResetCharm();
            StartCoroutine(PetrifyCooldownCoroutine);
        }
        if (channel.Split(':').Equals("DamageOnPlayer"))
        {
            GetComponent<PlayerHealth>().TakeDamage(float.Parse(channel.Split(':')[1]));
        }
        if (channel.Split(':')[0].Equals("SpeedChange"))
        {
            MoveSpeed *= float.Parse(channel.Split(':')[1]);
            SprintSpeed *= float.Parse(channel.Split(':')[1]);
        }

    }

    IEnumerator PetrifyCooldown(float seconds)
    {
        meshRenderer.materials[0].SetColor("_BaseColor", Color.grey);
        _animator.enabled = false;
        yield return new WaitForSeconds(seconds);
        _playerState = State.STANDING;
        _animator.enabled = true;
        meshRenderer.materials[0].SetColor("_BaseColor", _originalMaterialColor);
    }
    #endregion

    private void UpdateSound()
    {

    }
}
