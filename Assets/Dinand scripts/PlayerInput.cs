using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputAction primaryInput;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private Camera mainCam;

    private void Awake()
    {
        primaryInput.Enable();
        primaryInput.started += PrimaryInputStart;
    }

    private void PrimaryInputStart(InputAction.CallbackContext context)
    {
        Ray clickRay = mainCam.ScreenPointToRay(Pointer.current.position.ReadValue());
        if (Physics.Raycast(clickRay, out RaycastHit hit, 100f, floorLayer))
            playerAgent.SetDestination(hit.point);
    }
}
