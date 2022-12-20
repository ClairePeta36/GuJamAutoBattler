using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, ISelectHandler
{
    private PurchaseCard purchaseCard;
    public void Setup(PurchaseCard card)
    {
        purchaseCard = card;
    }
    public void OnSelect (BaseEventData eventData) 
    {
        purchaseCard.shopRef.OnCardClick(purchaseCard, purchaseCard.myData);
        purchaseCard.SetDragging(true);
        purchaseCard = null;
    }
}
