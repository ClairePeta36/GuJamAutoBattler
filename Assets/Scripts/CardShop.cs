using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShop : MonoBehaviour
{
    public List<PurchaseCard> allCards;
    public Text money;

    private EntityDatabase cachedDb;
    private int refreshCost = 1;

    private void Start()
    {
        cachedDb = GameManager.Instance.EntityDatabase;
        GenerateCard();
        PlayerData.Instance.OnUpdate += Refresh;
        Refresh();
    }

    private void GenerateCard()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
                allCards[i].gameObject.SetActive(true);

            allCards[i].Setup(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
        }
    }

    public void OnCardClick(PurchaseCard card, EntityDatabase.EntityData cardData)
    {
        //check if we can afford this card
        if (!PlayerData.Instance.CanAfford(cardData.cost))
        {
            return;
        }
        
        GameManager.Instance.SetPurchasing(true);
        GameManager.Instance.SetPurchasingItem(card, cardData);
    }

    public void OnRefreshClick()
    {
        if (!PlayerData.Instance.CanAfford(refreshCost))
        {
            return;
        }
        
        PlayerData.Instance.SpendMoney(refreshCost);
        GenerateCard();
    }

    void Refresh()
    {
        money.text = PlayerData.Instance.Money.ToString();
    }
}