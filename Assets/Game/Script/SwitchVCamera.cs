using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

public class SwitchVCamera : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoosAmount = 10;
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }
    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();

    }
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    private void StartAim()
    {
        virtualCamera.Priority += priorityBoosAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoosAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
