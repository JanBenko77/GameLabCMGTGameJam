using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    [SerializeField] private InputAction primaryInput;
    [SerializeField] private InputAction cursorMove;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private Camera mainCam;
    bool playerControllsActive = true;

    [SerializeField] private GameObject magGlass;

    private void Awake()
    {
        instance = this;
        primaryInput.Enable();
        primaryInput.started += PrimaryInputStart;
        cursorMove.Enable();
        cursorMove.performed += CursorMoveDelta;
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

    private void CursorMoveDelta(InputAction.CallbackContext context)
    {
        if (magGlass != null) magGlass.transform.position = Pointer.current.position.ReadValue();
        //magGlass.transform.position = mainCam.ScreenToViewportPoint(Pointer.current.position.ReadValue());
        Ray clickRay = mainCam.ScreenPointToRay(Pointer.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(clickRay, out hit, 100f, interactableLayer) && playerControllsActive)
        {
            if (magGlass != null) magGlass.SetActive(true);
            Debug.Log("Point at interactable");
        }
        else
        {
            if (magGlass != null) magGlass.SetActive(false);
        }

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
