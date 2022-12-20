﻿using UnityEngine;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    private DraggableCard draggableReference;
    
    // Physical Card
    public Button cardlayout;
    public Image imageBackground;
    public Text name;
    public Text cost;
    public Text tribe;
    public Text attack_health;
    public Text quantity;
    public Image iconTribe;
    public Text ability;

    [HideInInspector]
    public CardShop shopRef;
    
    public EntityDatabase.EntityData myData;
    
    public bool isDragging = false;

    private void Start()
    {
        cardlayout.gameObject.AddComponent<DraggableCard>();
        draggableReference = cardlayout.gameObject.GetComponent<DraggableCard>();
    }
    
    public void Setup(EntityDatabase.EntityData myData, CardShop shopRef)
    {
        imageBackground.sprite = myData.imageBackground;
        name.text = myData.name;
        cost.text = myData.cost.ToString();
        tribe.text = myData.tribe;
        attack_health.text = myData.attack + "/" + myData.health;
        quantity.text = myData.quantity.ToString();
        iconTribe.sprite = myData.iconTribe;
        ability.text = myData.ability;

        this.myData = myData;
        this.shopRef = shopRef;
        draggableReference.Setup(this);
    }

    public void SetDragging(bool val)
    {
        isDragging = val;
    }
    
    private void Update()
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }
}