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
    
    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();

    private bool isPurchasing = false;
    public bool IsPurchasing => isPurchasing;
    private PurchaseCard tryingToPurchaseCard;
    private EntityDatabase.EntityData tryingToPurchaseEntity;
    
    
    private void Start()
    {
        cam = Camera.main;
        Instance = this;
    }

    private void OnEntityBrought(EntityDatabase.EntityData entityData, Node spawnPosition)
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

    private void OnMouseUp()
    {
        if (!isPurchasing || tryingToPurchaseCard == null)
        {
            return;
        }
        
        //get mouse position/tile
        Tile spawnPosition = GetTileUnder();
        
        if (spawnPosition == null)
        {
            return;
        }
        
        int cost = tryingToPurchaseEntity.cost;
        PlayerData.Instance.SpendMoney(cost);
            
        tryingToPurchaseCard.gameObject.SetActive(false);
        OnEntityBrought(tryingToPurchaseEntity, GridManager.Instance.GetNodeFromTile(spawnPosition));
    }

    private Tile GetTileUnder()
    {
        LayerMask releaseMask = new LayerMask();
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, releaseMask);

        if (hit.collider == null)
        {
            return null;
        }
        
        //Released over something!
        Tile t = hit.collider.GetComponent<Tile>();
        return t;
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