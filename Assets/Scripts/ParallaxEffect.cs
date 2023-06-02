using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;

    Transform cameraTransform;
    float startingPos;
    Vector3 previousCameraPosition;

    private void Start()
    {
        startingPos = transform.position.x;
        cameraTransform = Camera.main.transform;
        //previousCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 position = cameraTransform.transform.position;
        float distance = position.x * parallaxFactor;

        Vector3 newPos = new Vector3(startingPos + distance, transform.position.y, transform.position.z);

        transform.position = newPos;
    }

    //private void FixedUpdate()
    //{
    //    float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxFactor;
    //    transform.Translate(new Vector3(deltaX, 0, 0));
    //    previousCameraPosition = cameraTransform.position;
    //}
}
