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
    public ShopPanel shopPanel;

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

    private int difficulty = 0;
    public Dropdown AIdifficulty;
    private List<int> EasyDifficulty = new List<int> { 3, 5, 10, 11, 16, 18, 19, 25, 27}; // only tier one
    private List<int> MeduimDifficulty = new List<int> { 3, 5, 8, 10, 11, 16, 17, 18, 19, 24, 25, 27, 29 }; // all
    private List<int> HardDifficulty = new List<int> { 8, 17, 24, 27 }; // only tier two

    private int countOfTeam1PLayed = 0;

    public GameObject endGameScreen;

    public int getcountOfTeam1PLayed()
    {
        return countOfTeam1PLayed;
    }

    public void setcountOfTeam1PLayed(int val)
    {
        countOfTeam1PLayed += val;
    }
    
    public PurchaseCard GetTryingToPurchaseCard()
    {
        return tryingToPurchaseCard;
    }

    public void SetDifficulty()
    {
        Debug.Log($"Claire value {AIdifficulty.value}");
        difficulty = AIdifficulty.value;
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
        if (newEntity.GetTeam() == Team.Team1)
        {
            Debug.Log($"Claire OnEntityAdded for {newEntity.name}");
            setcountOfTeam1PLayed(1);
        }

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
        shopPanel.ClosePanelOnGameStart();
    }

    private void CreateTeamTwo()
    {
        Debug.Log($"Claire countOfTeam1PLayed count {countOfTeam1PLayed}");
        for (int i = 0; i < countOfTeam1PLayed; i++)
        {
            int randomIndex = 0;
            BaseEntity newEntity = new BaseEntity();
            Node nodelocation = null;
            switch (difficulty)
            {
                case 0:
                    //easy
                    randomIndex = UnityEngine.Random.Range(0, EasyDifficulty.Count);
                    nodelocation = GridManager.Instance.GetFreeNode(Team.Team2);
                    for (int j = 0; j < EntityDatabase.allEntities[EasyDifficulty[randomIndex]].quantity; j++)
                    {
                        newEntity = Instantiate(EntityDatabase.allEntities[EasyDifficulty[randomIndex]].prefab, team2Parent);
                        newEntity.gameObject.name = EntityDatabase.allEntities[EasyDifficulty[randomIndex]].name;
                        team2Entities.Add(newEntity);
        
                        newEntity.Setup(Team.Team2, nodelocation, EntityDatabase.allEntities[randomIndex]);
                        newEntity.transform.position += newEntity.spawnpositions[i];
                    }
                    OnEntityAdded?.Invoke(newEntity);
                    break;
                case 1:
                    //medium
                    randomIndex = UnityEngine.Random.Range(0, EasyDifficulty.Count);
                    nodelocation = GridManager.Instance.GetFreeNode(Team.Team2);
                    for (int j = 0; j < EntityDatabase.allEntities[EasyDifficulty[randomIndex]].quantity; j++)
                    {
                        newEntity = Instantiate(EntityDatabase.allEntities[EasyDifficulty[randomIndex]].prefab, team2Parent);
                        newEntity.gameObject.name = EntityDatabase.allEntities[EasyDifficulty[randomIndex]].name;
                        team2Entities.Add(newEntity);
        
                        newEntity.Setup(Team.Team2, nodelocation, EntityDatabase.allEntities[randomIndex]);
                        newEntity.transform.position += newEntity.spawnpositions[i];
                    }
                    OnEntityAdded?.Invoke(newEntity);
                    break;
                case 2:
                    //hard
                    randomIndex = UnityEngine.Random.Range(0, EasyDifficulty.Count);
                    nodelocation = GridManager.Instance.GetFreeNode(Team.Team2);
                    for (int j = 0; j < EntityDatabase.allEntities[EasyDifficulty[randomIndex]].quantity; j++)
                    {
                        newEntity = Instantiate(EntityDatabase.allEntities[EasyDifficulty[randomIndex]].prefab, team2Parent);
                        newEntity.gameObject.name = EntityDatabase.allEntities[EasyDifficulty[randomIndex]].name;
                        team2Entities.Add(newEntity);
        
                        newEntity.Setup(Team.Team2, nodelocation, EntityDatabase.allEntities[randomIndex]);
                        newEntity.transform.position += newEntity.spawnpositions[i];
                    }
                    OnEntityAdded?.Invoke(newEntity);
                    break;
            }
        }
    }

    public void RoundEnd()
    {
        endGameScreen.SetActive(true);
    }
}

public enum Team
{
    Team1,
    Team2
}

