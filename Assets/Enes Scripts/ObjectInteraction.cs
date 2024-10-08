using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public Text interactionText;
    public string objectTag = "Collectible";
    private int itemsCollected = 0;
    private int totalItems;
    public void Start()
    {
        totalItems = GameObject.FindGameObjectsWithTag(objectTag).Length;
        UpdateInteractionText();
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(objectTag))
        {
            itemsCollected++;
            Destroy(other.gameObject);
            UpdateInteractionText();
        }
    }

   public void UpdateInteractionText()
    {
       interactionText.text = "Items Collected: " + itemsCollected+ " / " + totalItems;

       if(itemsCollected == totalItems)
        {
            interactionText.text = "All items collected!(" + itemsCollected + " / " + totalItems + ")";
        }
    }
}