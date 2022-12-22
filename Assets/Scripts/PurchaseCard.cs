using UnityEngine;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    public DraggableCard draggableReference;
    public ClickableCard clickableReference;
    
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

    public CardShop shopRef;
    public EntityDatabase.EntityData myData;
    public bool isDragging = false;
    
    public void Setup()
    {
        Destroy(cardlayout.gameObject.GetComponent<ClickableCard>());
        clickableReference = null;

        cardlayout.gameObject.AddComponent<DraggableCard>();
        draggableReference = cardlayout.gameObject.GetComponent<DraggableCard>();
        draggableReference.Setup(this);
    }
    
    public void SetupShop(EntityDatabase.EntityData myData, CardShop shopRef)
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
        
        cardlayout.gameObject.AddComponent<ClickableCard>();
        clickableReference = cardlayout.gameObject.GetComponent<ClickableCard>();
        clickableReference.Setup(this);
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