using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : Manager<GameManager>
{
    public EntityDatabase EntityDatabase;
    private Camera cam;
    
    public Transform team1Parent;
    public Transform team2Parent;

    public Action OnRoundStart;
    public Action OnRoundEnd;
    public Action<BaseEntity> OnEntityDied;

    public CardShop cardShop;
    
    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();

    private bool isPurchasing = false;
    public bool IsPurchasing => isPurchasing;
    private PurchaseCard tryingToPurchaseCard;
    private EntityDatabase.EntityData tryingToPurchaseEntity;

    public PurchaseCard GetTryingToPurchaseCard()
    {
        return tryingToPurchaseCard;
    }
    public EntityDatabase.EntityData GetTryingToPurchaseEntity()
    {
        return tryingToPurchaseEntity;
    }
    
    private void Start()
    {
        cam = Camera.main;
        Instance = this;
    }

    public void OnEntityBrought(EntityDatabase.EntityData entityData, Node spawnPosition)
    {
        BaseEntity newEntity = Instantiate(entityData.prefab, team1Parent);
        newEntity.gameObject.name = entityData.name;
        team1Entities.Add(newEntity);

        //below either use the vector3 position of the mouse and put that into game space, or get the tile from the tile script
        //or potentially could do a raycast hit on the mouse position
        newEntity.Setup(Team.Team1, spawnPosition);

        SetPurchasing(false);
        SetPurchasingItem(null, new EntityDatabase.EntityData());
    }

    public void SetPurchasing(bool val)
    {
       isPurchasing = val;
    }
    public void SetPurchasingItem(PurchaseCard card, EntityDatabase.EntityData cardData)
    {
        tryingToPurchaseCard = card;
        tryingToPurchaseEntity = cardData;
    }

    public void EntityDead(BaseEntity entity)
    {
        team1Entities.Remove(entity);
        team2Entities.Remove(entity);

        OnEntityDied?.Invoke(entity);

        Destroy(entity.gameObject);
    }
}

public enum Team
{
    Team1,
    Team2
}