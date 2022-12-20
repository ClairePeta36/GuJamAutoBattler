using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : Manager<GameManager>
{
    public EntityDatabase EntityDatabase;
    
    public Transform team1Parent;
    public Transform team2Parent;

    private bool isGameRunning = false;
    public Button startGameButton;
    
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
    public void CreateTeamTwo()
    {
        for (int i = 0; i < team1Parent.childCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, EntityDatabase.allEntities.Count);
            BaseEntity newEntity = Instantiate(EntityDatabase.allEntities[randomIndex].prefab, team2Parent);

            team2Entities.Add(newEntity);

            newEntity.Setup(Team.Team2, GridManager.Instance.GetFreeNode(Team.Team2));
        }
    }
    

}

public enum Team
{
    Team1,
    Team2
}