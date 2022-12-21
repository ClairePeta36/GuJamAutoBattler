using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableCard : MonoBehaviour, ISelectHandler
{
    private PurchaseCard purchaseCard;
    public void Setup(PurchaseCard card)
    {
        purchaseCard = card;
    }
    public void OnSelect (BaseEventData eventData) 
    {
        if (GameManager.Instance.GetIsGameRunning())
        {
            return;
        }

        if (purchaseCard == null)
        {
            return;
        }
        
        purchaseCard.shopRef.OnCardClick(purchaseCard, purchaseCard.myData);
        purchaseCard = null;
    }
}