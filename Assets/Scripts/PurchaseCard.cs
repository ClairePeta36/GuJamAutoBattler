using System;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    // Physical Card
    public Image icon;
    public Text name;
    public Text cost;
    public Text tribe;
    public Text health;
    public Text attack;
    public Text quantity;

    private CardShop shopRef;
    private EntityDatabase.EntityData myData;
    
    // Dragging
    private Camera cam;
    public Vector3 dragOffset = new Vector3(0, -0.4f, 0);
    private bool isDragging = false;

    private void Start()
    {
        cam = Camera.main;
    }
    
    public void Setup(EntityDatabase.EntityData myData, CardShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();
        tribe.text = myData.tribe;
        health.text = myData.health.ToString();
        attack.text = myData.attack.ToString();
        quantity.text = myData.quantity.ToString();

        this.myData = myData;
        this.shopRef = shopRef;
    }

    public void OnClick()
    {
        shopRef.OnCardClick(this, myData);
        isDragging = true;
    }

    public void SetDragging(bool val)
    {
        isDragging = val;
    }
    
    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
            newPosition.z = 0;
            this.transform.position = newPosition;
        }
    }
}