using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    [SerializeField] private InputAction primaryInput;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private Camera mainCam;
    bool playerControllsActive = true;

    private void Awake()
    {
        instance = this;
        primaryInput.Enable();
        primaryInput.started += PrimaryInputStart;
    }

    private void PrimaryInputStart(InputAction.CallbackContext context)
    {
        if (!playerControllsActive) return;
        Ray clickRay = mainCam.ScreenPointToRay(Pointer.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(clickRay, out hit, 100f, interactableLayer))
        {
            if (hit.collider.TryGetComponent<InteractableObject>(out InteractableObject interactable))
            {
                interactable.OnInteract();
            }
            playerAgent.SetDestination(hit.point);
        }
        else if (Physics.Raycast(clickRay, out hit, 100f, floorLayer))
            playerAgent.SetDestination(hit.point);
    }
    public void ActivatePlayerControlls()
    {
        playerControllsActive = true;
    }
    public void DisablePlayerControlls()
    {
        playerControllsActive = false;
    }
}
