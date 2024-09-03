using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideFromView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float maxBound;
    enum BoundDirection
    {
        posX,
        negY,
        posZ
    }
    [SerializeField] private BoundDirection boundDirection;
    [SerializeField] private GameObject[] objectsToHide;

    private void FixedUpdate()
    {
        bool isvisible = true;
        switch (boundDirection)
        {
            case BoundDirection.posX:
                if (playerTransform.position.x > maxBound) isvisible = false;
                break;
            case BoundDirection.negY:
                if (playerTransform.position.y < maxBound) isvisible = false;
                break;
            case BoundDirection.posZ:
                if (playerTransform.position.z > maxBound) isvisible = false;
                break;
        }
        for (int i = 0;i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(isvisible);
        }
    }
}