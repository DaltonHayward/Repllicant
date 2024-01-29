using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;

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
    public float DodgeCooldown = 1;
    private float _cooldownTimer;


    private enum State {MOVING, DODGING, INTERACTING, ATTACKING};
    private State _playerState;


    // Start is called before the first frame update
    void Start()
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
                LookAt();

                // cooldown for camera rotation
                if ((InputManager.instance.GetKey("rotateCameraLeft") || InputManager.instance.GetKey("rotateCameraRight")) && !_isRotating)
                { 
                    RotateCamera();
                }
                break;
            }
            case State.DODGING:
            {
                LookAt();
                break;
            }
        }
    }

    private void HandleMovement() 
    {

        // Player movement
        if (InputManager.instance.GetKey("up")) 
        {
            transform.position += ConvertToCameraSpace(Vector3.forward) * Time.deltaTime * _movementSpeed;
        } 
        else if (InputManager.instance.GetKey("down")) 
        {
            transform.position -= ConvertToCameraSpace(Vector3.forward) * Time.deltaTime * _movementSpeed;
        }

        if(InputManager.instance.GetKey("left"))
        {
            transform.position += ConvertToCameraSpace(Vector3.left) * Time.deltaTime * _movementSpeed;
        } 
        else if (InputManager.instance.GetKey("right")) 
        {
            transform.position -= ConvertToCameraSpace(Vector3.left) * Time.deltaTime * _movementSpeed;
        }
    }

    private void HandleDodge() 
    {
        Vector3 direction = (transform.position - _previousPos).normalized;
        if (InputManager.instance.GetKey("dodge") && !_isDodging)
        {
            GetComponent<Health>().Invinsible(_delayBeforeInvinsible, _invinsibleDuration);
            StartCoroutine(Dodge(transform.position + direction * _dodgeDistance));
        }

        _previousPos = transform.position;
    }

    IEnumerator Dodge(Vector3 newPosition)
    {
        _isDodging = true;
        _playerState = State.DODGING;
        float elapsedTime = 0f;
        
        while(elapsedTime < _dodgeDuration)
        {
            float ratio = elapsedTime / _dodgeDistance;
            transform.position = Vector3.Lerp(transform.position, newPosition, ratio);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _isDodging = false;
        _playerState = State.MOVING;
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
        float fraction = elapsedTime / _rotationSpeed;

        while(elapsedTime <= _rotationSpeed)
        {
            _playerCamera.rotation = Quaternion.Lerp(_playerCamera.rotation, Quaternion.Euler(_playerCamera.localEulerAngles.x, cameraYAngle, _playerCamera.localEulerAngles.z), fraction);
            elapsedTime += Time.deltaTime;
            fraction = elapsedTime / _rotationSpeed;

            yield return Time.deltaTime;
        }

        _isRotating = false;
    }


    // 360 rotation of the player towards the mouse position
    public void LookAt()
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
