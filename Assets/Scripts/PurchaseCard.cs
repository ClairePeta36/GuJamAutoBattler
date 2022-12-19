using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    // Physical Card
    public Button bg;
    public Image icon;
    public Text name;
    public Text cost;
    public Text tribe;
    public Text health;
    public Text attack;
    public Text quantity;

    public CardShop shopRef;
    public EntityDatabase.EntityData myData;
    
    // Dragging
    public Vector3 dragOffset = new Vector3(0, -0.4f, 0);
    public bool isDragging = false;

    private void Start()
    {
        bg.gameObject.AddComponent<AvailableTrayItem>();
    }
    
    public void Setup(EntityDatabase.EntityData myData, CardShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();
        /*tribe.text = myData.tribe;
        health.text = myData.health.ToString();
        attack.text = myData.attack.ToString();
        quantity.text = myData.quantity.ToString();*/

        this.myData = myData;
        this.shopRef = shopRef;
        AvailableTrayItem.Setup(this);
    }

    public void SetDragging(bool val)
    {
        isDragging = val;
    }
    
    private void Update()
    {
        if (isDragging)
        {
            this.transform.position = Input.mousePosition + dragOffset;
        }
    }
}
class AvailableTrayItem : MonoBehaviour, ISelectHandler
{
    private static PurchaseCard _purchaseCard;

    public static void Setup(PurchaseCard card)
    {
        _purchaseCard = card;
    }
    public void OnSelect (BaseEventData eventData) 
    {
        _purchaseCard.shopRef.OnCardClick(_purchaseCard, _purchaseCard.myData);
        _purchaseCard.SetDragging(true);
    }
}