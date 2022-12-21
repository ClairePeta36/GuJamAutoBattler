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

    private List<int> validCharacters = new List<int> { 17, 18, 21 };

    private void GenerateCard()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
                allCards[i].gameObject.SetActive(true);

            //allCards[i].SetupShop(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
            allCards[i].SetupShop(cachedDb.allEntities[validCharacters[Random.Range(0, validCharacters.Count)]], this);
            //allCards[i].SetupShop(cachedDb.allEntities[17], this);
        }
    }

    public void OnCardClick(PurchaseCard card, EntityDatabase.EntityData cardData)
    {
        //This is now going to place the card into the hand
        if (!PlayerData.Instance.CanAfford(cardData.cost))
        {
            return;
        }
        
        PlayerData.Instance.SpendMoney(cardData.cost);
        card.gameObject.SetActive(false);
        GameManager.Instance.OnEntityBroughtFromShop(card, GameManager.Instance.cardSpawnLocation);
    }
    
    public void OnCardClickDrag(PurchaseCard card, EntityDatabase.EntityData cardData)
    {
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
        money.text = "Cash " + PlayerData.Instance.Money.ToString();
    }
}