using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.2f;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;

    private void Awake() {
        if(target != null) {
            transform.position = target.transform.position;
        } else {
            transform.position = new Vector3(0,0,-10);
        }
            
    }

    void Update()
    {   
        if(target != null) {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
        }   
    }
}
