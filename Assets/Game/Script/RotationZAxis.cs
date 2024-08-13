using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateZAxis : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float rotationAmount = mouseY * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
