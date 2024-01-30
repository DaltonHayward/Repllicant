using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


    private enum State {MOVING, DODGING, INTERACTING, ATTACKING};
    private State _playerState;


    // Start is called before the first frame update
    void Awake()
    {
        // _anim = GetComponentInChildren<Animator>();
        _playerState = State.MOVING;
        _previousPos = transform.position;
        _cameraYAngle = FIRST;
        _playerCamera.rotation = Quaternion.Euler(_playerCamera.localEulerAngles.x, _cameraYAngle, _playerCamera.localEulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_playerState)
        {
            case State.MOVING:
            {
                HandleMovement();
                HandleDodge();
                LookAtMouse();

                // cooldown for camera rotation
                if ((InputManager.instance.GetKey("rotateCameraLeft") || InputManager.instance.GetKey("rotateCameraRight")) && !_isRotating)
                { 
                    RotateCamera();
                }
                break;
            }
            case State.DODGING:
            {
                LookAtMouse();
                break;
            }
        }
    }

    private void HandleMovement() 
    {
        Vector3 direction = new Vector3(0,0,0);

        // Player movement
        if (InputManager.instance.GetKey("up")) 
        {
            direction += ConvertToCameraSpace(Vector3.forward);
        } 
        else if (InputManager.instance.GetKey("down")) 
        {
            direction -= ConvertToCameraSpace(Vector3.forward);
        }

        if(InputManager.instance.GetKey("left"))
        {
            direction += ConvertToCameraSpace(Vector3.left);
        } 
        else if (InputManager.instance.GetKey("right")) 
        {

            direction -= ConvertToCameraSpace(Vector3.left);
        }

        transform.position += _movementSpeed * Time.deltaTime * direction.normalized;
    }

    private void HandleDodge() 
    {
        Vector3 direction = (transform.position - _previousPos).normalized;
        // Debug.Log(direction);
        
        if (InputManager.instance.GetKeyDown("dodge") && !_isDodging && direction != Vector3.zero)
        {
            GetComponent<Health>().Invinsible(_delayBeforeInvinsible, _invinsibleDuration);
            Debug.Log(transform.position + direction * _dodgeDistance);
            StartCoroutine(Dodge(transform.position + direction * _dodgeDistance));
            StartCoroutine(DodgeCooldown());
        }

        _previousPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            _isColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
         if (!collision.collider.CompareTag("Ground"))
        {
            _isColliding = false;
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

        float elapsedTime = 0f;
        float ratio = elapsedTime / _dodgeDuration;
        // Vector3 velocity = Vector3.zero;
        
        while(elapsedTime < _dodgeDuration && !_isColliding)
        {
            // float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / _dodgeDuration);

            transform.position = Vector3.Lerp(transform.position, newPosition, EaseOut(ratio));
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
        switch (_cameraYAngle) 
        {
            case FIRST:
                if (InputManager.instance.GetKey("rotateCameraRight")) 
                {
                    _cameraYAngle = FOURTH;
                }
                else if (InputManager.instance.GetKey("rotateCameraLeft"))
                {
                    _cameraYAngle = SECOND;
                }
                break;
            case SECOND:
                if (InputManager.instance.GetKey("rotateCameraRight")) 
                {
                    _cameraYAngle = FIRST;
                }
                else if (InputManager.instance.GetKey("rotateCameraLeft"))
                {
                    _cameraYAngle = THIRD;
                }
                break;
            case THIRD:
                if (InputManager.instance.GetKey("rotateCameraRight")) 
                {
                    _cameraYAngle = SECOND;
                }
                else if (InputManager.instance.GetKey("rotateCameraLeft"))
                {
                    _cameraYAngle = FOURTH;
                }
                break;
            case FOURTH:
                if (InputManager.instance.GetKey("rotateCameraRight")) 
                {
                    _cameraYAngle = THIRD;
                }
                else if (InputManager.instance.GetKey("rotateCameraLeft"))
                {
                    _cameraYAngle = FIRST;
                }
                break;
        }

        StartCoroutine(LerpRotation(_cameraYAngle));
    }

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // create a ray from the mouse position of screen to a world point
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
