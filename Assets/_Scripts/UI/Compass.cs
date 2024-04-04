using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private Transform playerTransform;
    public Transform target;
    public RectTransform compassUI;
    private Transform mainCameraTransform;

    void Start()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Find the main camera
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // prevent null ref exceptions
        if (target == null || compassUI == null || playerTransform == null) 
        {
            Debug.Log("Missing compass references, failed check: if (target == null || compassUI == null || playerTransform == null) ");
            return; 
        }

        // direction from compass to target
        Vector3 direction = target.position - playerTransform.position;

        // Apply camera rotation
        direction = Quaternion.Euler(0, -mainCameraTransform.eulerAngles.y, 0) * direction;

        // rotation to point towards target
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Convert rotation to UI space
        Vector3 eulerRotation = rotation.eulerAngles;
        eulerRotation.z = -eulerRotation.y + 46;
        eulerRotation.x = eulerRotation.y = 0;

        // Apply rotation to compass UI
        compassUI.localRotation = Quaternion.Euler(eulerRotation);
    }


}
