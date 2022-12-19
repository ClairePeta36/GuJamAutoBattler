using UnityEngine;
using UnityEngine.UI;

public class PurchaseCard : MonoBehaviour
{
    public Image icon;
    public Text name;
    public Text cost;

    private CardShop shopRef;
    private EntityDatabase.EntityData myData;

    public void Setup(EntityDatabase.EntityData myData, CardShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.myData = myData;
        this.shopRef = shopRef;
    }

    public void OnClick()
    {
        shopRef.OnCardClick(this, myData);
    }
}