using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color validColor;
    public Color wrongColor;
    public Renderer renderer;

    private void SetHighlight(bool active, bool valid)
    {
        if (active)
        {
            var colour = valid ? validColor : wrongColor;
            renderer.material.SetColor("_Color", colour);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.white);
        }
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.IsPurchasing)
        {
            return;
        }

        bool valid = !GridManager.Instance.GetNodeFromTile(this).IsOccupied && this.transform.position.x > 25;
        SetHighlight(true, valid);
    }

    private void OnMouseExit()
    {
        if (!GameManager.Instance.IsPurchasing)
        {
            return;
        }
        SetHighlight(false, !GridManager.Instance.GetNodeFromTile(this).IsOccupied);
    }
    
    public void OnMouseUp()
    {
        if (!GameManager.Instance.IsPurchasing || GameManager.Instance.GetTryingToPurchaseCard() == null)
        {
            return;
        }
        GameManager.Instance.GetTryingToPurchaseCard().SetDragging(false);
        PlayerData.Instance.SpendMoney(GameManager.Instance.GetTryingToPurchaseEntity().cost);
            
        // Destroy Card instance
        GameManager.Instance.cardShop.allCards.Remove(GameManager.Instance.GetTryingToPurchaseCard());
        Destroy(GameManager.Instance.GetTryingToPurchaseCard());
        GameManager.Instance.GetTryingToPurchaseCard().gameObject.SetActive(false);
        GameManager.Instance.OnEntityBrought(GameManager.Instance.GetTryingToPurchaseEntity(), GridManager.Instance.GetNodeFromTile(this));
        SetHighlight(false, false);
    }
}