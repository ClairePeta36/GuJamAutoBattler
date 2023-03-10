using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color validColor;
    public Color wrongColor;
    public Renderer renderer;
    private Color startColour;

    private void Start()
    {
        startColour = renderer.material.color;
    }

    private void SetHighlight(bool active, bool valid)
    {
        if (active)
        {
            var colour = valid ? validColor : wrongColor;
            renderer.material.SetColor("_Color", colour);
        }
        else
        {
            renderer.material.SetColor("_Color", startColour);
        }
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.IsPurchasing && !GameManager.Instance.IsDraggingEntity)
        {
            return;
        }
        bool valid = !GridManager.Instance.GetNodeFromTile(this).IsOccupied && this.transform.position.x > 25;
        SetHighlight(true, valid);
        if (GameManager.Instance.IsDraggingEntity)
        {
            GameManager.Instance.getDraggingEntity().draggableEntity.MoveCharacter(this);
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.IsPurchasing || GameManager.Instance.IsDraggingEntity)
        {
            SetHighlight(false, !GridManager.Instance.GetNodeFromTile(this).IsOccupied);
        }
    }
    
    public void OnMouseUp()
    {
        if (GameManager.Instance.GetIsGameRunning())
        {
            return;
        }
        if (GameManager.Instance.IsPurchasing && GameManager.Instance.GetTryingToPurchaseCard() != null)
        {
            GameManager.Instance.GetTryingToPurchaseCard().SetDragging(false);
            
            // Destroy Card instance
            GameManager.Instance.cardShop.allCards.Remove(GameManager.Instance.GetTryingToPurchaseCard());
            Destroy(GameManager.Instance.GetTryingToPurchaseCard());
            GameManager.Instance.GetTryingToPurchaseCard().gameObject.SetActive(false);
            GameManager.Instance.OnEntityBrought(GameManager.Instance.GetTryingToPurchaseEntity(), GridManager.Instance.GetNodeFromTile(this));
            SetHighlight(false, false);
        }
        else if (GameManager.Instance.IsDraggingEntity)
        {
            SetHighlight(false, false);
            GameManager.Instance.getDraggingEntity().draggableEntity.OnEndDrag(this);
        }
        else
        {
            return;
        }
        

    }
}