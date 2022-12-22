using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : Manager<GameManager>
{
    public EntityDatabase EntityDatabase;
    public PurchaseCard PurchaseCardPrefab;
    public Transform team1Parent;
    public Transform team2Parent;

    private bool isGameRunning = false;
    public Button startGameButton;

    public Action OnRoundStart;
    public Action OnRoundEnd;
    public Action<BaseEntity> OnEntityDied;
    public Action<BaseEntity> OnEntityAdded;
    public Action<PurchaseCard> OnEntityPurchased;

    public CardShop cardShop;

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();

    private bool isPurchasing = false;
    private bool isDraggingEntity = false;
    public bool IsPurchasing => isPurchasing;
    public bool IsDraggingEntity => isDraggingEntity;
    private PurchaseCard tryingToPurchaseCard;
    private EntityDatabase.EntityData tryingToPurchaseEntity;
    private BaseEntity draggingEntity;

    public GameObject cardSpawnLocation;
    
    public PurchaseCard GetTryingToPurchaseCard()
    {
        return tryingToPurchaseCard;
    }

    public EntityDatabase.EntityData GetTryingToPurchaseEntity()
    {
        return tryingToPurchaseEntity;
    }

    public BaseEntity getDraggingEntity()
    {
        return draggingEntity;
    }

    public void SetIsDraggingEntity(bool val)
    {
        isDraggingEntity = val;
    }

    private void Start()
    {
        Instance = this;
    }

    public bool GetIsGameRunning()
    {
        return isGameRunning;
    }
    void SetGameStart()
    {
        isGameRunning = true;
        OnRoundStart?.Invoke();
    }

    public void OnEntityBrought(EntityDatabase.EntityData entityData, Node spawnPosition)
    {
        BaseEntity newEntity = new BaseEntity();
        for (int i = 0; i < entityData.quantity; i++)
        {
            newEntity = Instantiate(entityData.prefab, team1Parent);
            newEntity.gameObject.name = entityData.name;
            team1Entities.Add(newEntity);
        
            newEntity.Setup(Team.Team1, spawnPosition, entityData);
            newEntity.transform.position += newEntity.spawnpositions[i];
        }
        OnEntityAdded?.Invoke(newEntity);

        SetPurchasing(false);
        SetPurchasingItem(null, new EntityDatabase.EntityData());
    }
    
    public void OnEntityCreated(BaseEntity entityData, Vector3 spawnPosition)
    {
        BaseEntity newEntity = Instantiate(entityData, team1Parent);
        newEntity.gameObject.name = entityData.name;
        newEntity.transform.position = spawnPosition;
        team1Entities.Add(newEntity);
        
        SetPurchasing(false);
        SetPurchasingItem(null, new EntityDatabase.EntityData());
    }
    
    public void OnEntityBroughtFromShop(PurchaseCard card, GameObject spawnPosition)
    {
        var newCard = Instantiate(card, spawnPosition.transform);
        newCard.Setup();
        newCard.transform.localScale = new Vector3(1, 1, 1);
        newCard.gameObject.SetActive(true);

        card.gameObject.SetActive(false);
        OnEntityPurchased?.Invoke(card);
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

    public void SetDraggingEntity(BaseEntity entity)
    {
        draggingEntity = entity;
    }

    public List<BaseEntity> GetEntitiesAgainst(Team against)
    {
        return against == Team.Team1 ? team2Entities : team1Entities;
    }
    
    public void EntityDead(BaseEntity entity)
    {
        team1Entities.Remove(entity);
        team2Entities.Remove(entity);

        OnEntityDied?.Invoke(entity);

        Destroy(entity.gameObject);
    }

    public void StartGame()
    {
        if (isGameRunning)
        {
            return;
        }
        
        //first we want to instantiate a number of opposition units/entitys
        CreateTeamTwo();
        
        //we then want to trigger the start game countdown?

        //begin the tick counting
        SetGameStart();
        startGameButton.gameObject.SetActive(false);
    }

    private void CreateTeamTwo()
    {
        for (int i = 0; i < team1Parent.childCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, EntityDatabase.allEntities.Count);
            BaseEntity newEntity = Instantiate(EntityDatabase.allEntities[randomIndex].prefab, team2Parent);

            team2Entities.Add(newEntity);

            // might eventually adjust this to depend on the difficulty
            // make a easy, medium and hard database for the AI
            // or use the same database but limit selection
            newEntity.Setup(Team.Team2, GridManager.Instance.GetFreeNode(Team.Team2), EntityDatabase.allEntities[randomIndex]);
        }
    }
}

public enum Team
{
    Team1,
    Team2
}