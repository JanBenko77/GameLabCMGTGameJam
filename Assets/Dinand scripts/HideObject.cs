using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    [SerializeField] private Material tpMaterial;
    [SerializeField] private Material opMaterial;
    
    Color opacityColor;
    bool revealing;
    MeshRenderer mRenderer;
    Collider mCollider;
    private const float minOpacity = 0.2f;

    private void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
        mCollider = GetComponent<Collider>();
        opacityColor = mRenderer.material.color;

    }
    public void Show()
    {
        revealing = true;
    }
    public void Hide()
    {
        revealing = false;
    }

    public void RevealImmediately(bool reveal)
    {
        revealing = reveal;
        opacityColor.a = reveal? 1 : minOpacity;
        if (reveal)
        {
            mRenderer.material = opMaterial;
            mRenderer.material.color = opacityColor;
            //mRenderer.enabled = true;
            if (mCollider != null) mCollider.enabled = true;
        }
        else
        {
            mRenderer.material = tpMaterial;
            mRenderer.material.color = opacityColor;
            //mRenderer.enabled = false;
            if (mCollider != null) mCollider.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if ((opacityColor.a <= minOpacity && !revealing) || (opacityColor.a == 1 && revealing)) return;

        if (revealing)
        {
            //mRenderer.enabled = true;
            if (mCollider != null) mCollider.enabled = true;

            opacityColor.a = Mathf.Clamp(opacityColor.a + 0.05f, minOpacity, 1);
            if (opacityColor.a == 1)
                mRenderer.material = opMaterial;
            
            mRenderer.material.color = opacityColor;
        }
        else if (!revealing)
        {
            if (opacityColor.a == 1)
                mRenderer.material = tpMaterial;
            
            opacityColor.a = Mathf.Clamp(opacityColor.a - 0.05f, minOpacity, 1);

            mRenderer.material.color = opacityColor;

            if (opacityColor.a <= minOpacity)
            {
                //mRenderer.enabled = false;
                if (mCollider != null) mCollider.enabled = false;
            }
        }
    }
}
