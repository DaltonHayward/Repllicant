using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [Header("Player")]
    [SerializeField] private float _movementSpeed;

    const float FIRST = 0f;
    const float SECOND = 90f;
    const float THIRD = 180f;
    const float FOURTH = 270f;
    private float _cameraYAngle;
    [Header("Camera Rotation")]
    [SerializeField][Range(0.1f, 5f)]
    private float _rotationSpeed = 1;
    private bool _isRotating = false;

    [Header("Dodge")]
    [SerializeField][Range(1f, 10f)]
    private float _dodgeDistance = 5f;
    [SerializeField][Range(0.1f, 2f)]
    private float _dodgeDuration = 1;
    private bool _isDodging = false;
    // private Animator _anim;
    private Vector3 _previousPos;

    [SerializeField][Range(0f, 1f)]
    private float _delayBeforeInvinsible = 0.2f;
    [SerializeField][Range(0f, 2f)]
    private float _invinsibleDuration = 1f;
    [SerializeField][Range(0f, 5f)]   
    private float _dodgeCooldown = 1;
    private bool _isColliding = false;

    [SerializeField] private float _interactRange = 3f;
    const float GOLDEN_RATIO = .54f;
    private bool _isCrafting = false;


    private enum State {MOVING, DODGING, INTERACTING, ATTACKING};
    private State _playerState;


    // Start is called before the first frame update
    void Awake()
    {
        // _anim = GetComponentInChildren<Animator>();
        GetComponent<SphereCollider>().radius = _interactRange * GOLDEN_RATIO; //finding this number was hell
        _playerState = State.MOVING;
        _previousPos = transform.position;
        _cameraYAngle = FIRST;
        _playerCamera.rotation = Quaternion.Euler(_playerCamera.localEulerAngles.x, _cameraYAngle, _playerCamera.localEulerAngles.z);
    }

    void Update()
    {
        if (InputManager.instance.CameraLeftInput)
        {
            Debug.Log("LEft");
        }
        if (InputManager.instance.CameraRightInput)
        {
            Debug.Log("Right");
        }
        switch (_playerState)
        {
            case State.MOVING:
                {
                    HandleMovement();
                    LookAtMouse();
                    HandleInteract();
                    HandleDodge();

                    // cooldown for camera rotation
                   
                    
                    RotateCamera();
                    break;
                    
                }
            case State.DODGING:
                {
                    LookAtMouse();
                    break;
                }
            case State.INTERACTING:
                {
                    HandleInteract();
                    break;
                }
        }
    }

    private void HandleMovement() 
    {
        Vector3 direction = new Vector3(InputManager.instance.MoveInput.x, transform.position.y, InputManager.instance.MoveInput.y);

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.MovePosition(rb.position + ConvertToCameraSpace(direction) * _movementSpeed * Time.deltaTime);
    
    }

    private void HandleInteract()
    {
        if (InputManager.instance.InteractInput)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, _interactRange);
   
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Interactable"))
                {
                    Debug.Log("Here");
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
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
                if (c.CompareTag("Siren")) {
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                }
            }
        }
    }
    
    private void HandleDodge() 
    {
        Vector3 direction = new Vector3(InputManager.instance.MoveInput.x, transform.position.y, InputManager.instance.MoveInput.y);
        // Debug.Log(direction);

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

    public static float EaseOut(float t)
    {
        return Flip(Flip(t) * Flip(t));
    }

    IEnumerator Dodge(Vector3 newPosition)
    {
        _playerState = State.DODGING;
        Rigidbody rb = GetComponent<Rigidbody>();

        float elapsedTime = 0f;
        float ratio = elapsedTime / _dodgeDuration;
        
        while(elapsedTime < _dodgeDuration && !_isColliding)
        {
            // float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / _dodgeDuration);

            rb.MovePosition(Vector3.Lerp(transform.position, newPosition, EaseOut(ratio)));
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

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) 
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

            StartCoroutine(LerpRotation(_cameraYAngle));
        }
        
    }

    IEnumerator LerpRotation(float cameraYAngle)
    {
        Debug.Log("Lerping");
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
            { // check that the ray collides with the ground and only the ground
                Debug.DrawRay(hits[i].transform.position, hits[i].transform.forward, Color.green);
                transform.LookAt(hits[i].point); // Look at the point
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation
            }
        }
    }
}
