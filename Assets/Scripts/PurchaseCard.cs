using UnityEngine;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    public Draggable draggableReference;
    
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
    
    public bool isDragging = false;

    private void Start()
    {
        bg.gameObject.AddComponent<Draggable>();
        draggableReference = bg.gameObject.GetComponent<Draggable>();
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