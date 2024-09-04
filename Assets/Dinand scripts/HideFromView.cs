using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideFromView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float boundModifier;
    enum BoundDirection
    {
        posX,
        negY,
        posZ
    }
    [SerializeField] private BoundDirection boundDirection;
    [SerializeField] public List<HideObject> hideObjects {  get; private set; } = new List<HideObject>();
    [SerializeField] private List<HideFromView> childHiders = new List<HideFromView>();
    [SerializeField] private float hightLimit = 500;
    [SerializeField] private float maxPlayerDistance = 500;

    public bool disabledByParent;
    bool wasVisible;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerInput>().transform;
        //hideObjects = new List<HideObject>();
        hideObjects.AddRange(GetComponentsInChildren<HideObject>());
        childHiders.AddRange(GetComponentsInChildren<HideFromView>());
        childHiders.Remove(this);
        foreach (HideFromView hfv in childHiders)
        {
            for (int i = 0; i < hfv.hideObjects.Count; i++)
            {
                hideObjects.Remove(hfv.hideObjects[i]);
            }
        }

        if (disabledByParent) return;

        bool isVisible = true;
        switch (boundDirection)
        {
            case BoundDirection.posX:
                if (playerTransform.position.x > transform.position.x + boundModifier) isVisible = false;
                break;
            case BoundDirection.negY:
                if (playerTransform.position.y < transform.position.y + boundModifier) isVisible = false;
                break;
            case BoundDirection.posZ:
                if (playerTransform.position.z > transform.position.z + boundModifier) isVisible = false;
                break;
        }
        for (int i = 0; i < hideObjects.Count; i++)
        {
            hideObjects[i].RevealImmediately(isVisible);
        }
        foreach (HideFromView hfv in childHiders)
        {
            hfv.disabledByParent = !isVisible;
            for (int i = 0; i < hfv.hideObjects.Count; i++)
            {
                hfv.hideObjects[i].RevealImmediately(isVisible);
            }
        }
        wasVisible = isVisible;
    }

    private void FixedUpdate()
    {
        if (disabledByParent) return;
        bool isVisible = true;
        switch (boundDirection)
        {
            case BoundDirection.posX:
                if (playerTransform.position.x > transform.position.x + boundModifier) isVisible = false;
                break;
            case BoundDirection.negY:
                if (playerTransform.position.y < transform.position.y + boundModifier) isVisible = false;
                break;
            case BoundDirection.posZ:
                if (playerTransform.position.z > transform.position.z + boundModifier) isVisible = false;
                break;
        }
        if (playerTransform.position.y >= hightLimit || Vector3.Distance(transform.position, playerTransform.position) >= maxPlayerDistance) isVisible = true;
        if (isVisible != wasVisible)
        {
            for (int i = 0; i < hideObjects.Count; i++)
            {
                if (isVisible)
                    hideObjects[i].Show();
                else
                    hideObjects[i].Hide();
            }
            wasVisible = isVisible;

            foreach (HideFromView hfv in childHiders)
            {
                hfv.disabledByParent = !isVisible;
                hfv.SequenceCheck(isVisible);
            }
        }
    }

    public void SequenceCheck(bool isVisible)
    {
        //if (isVisible != wasVisible)
        {
            for (int i = 0; i < hideObjects.Count; i++)
            {
                if (isVisible)
                    hideObjects[i].Show();
                else
                    hideObjects[i].Hide();
            }
            wasVisible = isVisible;
        }
    }
}