using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShop : MonoBehaviour
{
    public List<PurchaseCard> allCards;
    public Text money;

    private EntityDatabase cachedDb;
    public int refreshCost = 1;

    private void Start()
    {
        cachedDb = GameManager.Instance.EntityDatabase;
        GenerateCard();
        PlayerData.Instance.OnUpdate += Refresh;
        Refresh();
    }

    private List<int> validCharacters = new List<int> { 3, 5, 8, 10, 16, 17, 18, 19, 24, 25, 27, 29 };

    private void GenerateCard()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
                allCards[i].gameObject.SetActive(true);

            //allCards[i].SetupShop(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
            allCards[i].SetupShop(cachedDb.allEntities[validCharacters[Random.Range(0, validCharacters.Count)]], this);
        }
    }
    public void GenerateSingleCard(EntityDatabase.EntityData entityData, int healthoverride = 0, int attackoverride = 0, int quantityoverride = 0, Transform overRidePosition = null)
    {
        var overwrite = overRidePosition == null ? GameManager.Instance.cardSpawnLocation.transform : overRidePosition;
        var card = Instantiate(GameManager.Instance.PurchaseCardPrefab, overwrite);
        if (healthoverride > 0)
        {
            entityData.health = healthoverride;
        }
        if (attackoverride > 0)
        {
            entityData.attack = attackoverride;
        }
        if (healthoverride > 0)
        {
            entityData.quantity = quantityoverride;
        }
        card.SetupShop(entityData, this);
        card.Setup();
        card.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnCardClick(PurchaseCard card, EntityDatabase.EntityData cardData)
    {
        //This is now going to place the card into the hand
        if (!PlayerData.Instance.CanAfford(cardData.cost))
        {
            return;
        }
        
        PlayerData.Instance.SpendMoney(cardData.cost);
        card.shopRef = this;
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