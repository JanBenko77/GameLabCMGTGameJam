using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePopup : InteractableObject
{
    [SerializeField] private GameObject popup;
    public override void OnInteract()
    {
        PlayerInput.instance.DisablePlayerControlls();
        popup.SetActive(true);
    }
}
