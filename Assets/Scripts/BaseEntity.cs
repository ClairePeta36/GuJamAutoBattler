using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This will be used as the base class for each team member/party member
public class BaseEntity : MonoBehaviour
{
    public Animator animator;

    protected readonly List<Keyword> ownedKeywords = new List<Keyword>();
    public IReadOnlyList<Keyword> OwnedKeywords => ownedKeywords;

    [HideInInspector]
    public int baseDamage = 1;
    [HideInInspector]
    public int baseHealth = 3;
    [HideInInspector]
    public int range = 5;
    float movementSpeed = 10f; //Movement per second
    int quantity = 1;

    protected Team myTeam;
    protected Tribe myTribe;
    protected bool tribeBonus = false;
    [HideInInspector]
    public BaseEntity currentTarget = null;
    protected Node currentNode;

    public Node CurrentNode => currentNode;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsEnemyInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    
    protected bool moving;
    protected Node destinationNode;

    protected bool dead = false;
    protected bool canAttack = true;

    public DraggableEntity draggableEntity;

    protected bool attackThisUpdate = false;

    [HideInInspector] 
    public List<Keyword> appliedKeywords;
    
    [HideInInspector] 
    public List<Vector3> spawnpositions = new List<Vector3>
    {
        new Vector3(0, 0, 0),
        new Vector3(2.5f, 0, 2.5f),
        new Vector3(2.5f, 0, -2.5f),
        new Vector3(-2.5f, 0, 2.5f),
        new Vector3(-2.5f, 0, -2.5f),
    };

    [HideInInspector] 
    public EntityDatabase.EntityData _entityData;
    
    public void Setup(Team team, Node currentNode, EntityDatabase.EntityData entityData)
    {
        myTeam = team;
        this.currentNode = currentNode;
        transform.position = currentNode.worldPosition;
        if (team == Team.Team1)
        {
            transform.Rotate(0, -90, 0);
        }
        else
        {
            transform.Rotate(0, 90, 0);
        }

        switch (entityData.tribe)
        {
            case "Amazon":
                myTribe = Tribe.Amazon;
                break;
            case "Anubian":
                myTribe = Tribe.Anubian;
                break;
            case "Dragon":
                myTribe = Tribe.Dragon;
                break;
            case "Viking":
                myTribe = Tribe.Viking;
                break;
            case "Wild":
                myTribe = Tribe.Wild;
                break;
            default:
                myTribe = Tribe.None;
                break;
        }

        baseHealth = entityData.health;
        baseDamage = entityData.attack;
        quantity = entityData.quantity;
        _entityData = entityData;
        currentNode.SetOccupied(true);
    }

    public Tribe getTribe()
    {
        return myTribe;
    }

    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnEntityDied += OnUnitDied;
        GameManager.Instance.OnEntityAdded += OnEntityAdded;
        GameManager.Instance.OnEntityPurchased += OnEntityPurchased;

    }

    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnUnitDied(BaseEntity diedUnity) { }
    protected virtual void OnEntityAdded(BaseEntity addedUnity) { }
    protected virtual void OnEntityPurchased(PurchaseCard broughtUnity) { }

    public Team GetTeam()
    {
        return myTeam;
    }
    public void IncreaseQuantity(int val)
    {
        quantity += val;
    }
    public int GetQuantity()
    {
        return quantity;
    }
    protected void FindTarget()
    {
        var allEnemies = GameManager.Instance.GetEntitiesAgainst(myTeam);
        float minDistance = Mathf.Infinity;
        BaseEntity entity = null;
        foreach (BaseEntity e in allEnemies)
        {
            if (Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
            {
                if (entity != null && entity.appliedKeywords.Contains(Keyword.Frontline) && e.appliedKeywords.Contains(Keyword.Frontline))
                {
                    continue;
                }
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }else if (e.appliedKeywords.Contains(Keyword.Frontline) && Vector3.Distance(e.transform.position, this.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }
        }

        currentTarget = entity;
    }

    private bool MoveTowards(Node nextNode)
    {
        Vector3 direction = (currentTarget.transform.position - this.transform.position);


        if (direction.magnitude < 5f)
        {
            transform.position = currentTarget.transform.position;
            animator.SetBool("walking", false);
            return true;
        }
        //animator.SetBool("walking", true);

        this.transform.position += direction.normalized * (movementSpeed * Time.deltaTime);
        
        var lookPosition = currentTarget.transform.position - this.transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        
        return false;
    }

    protected bool EntityInRange(Vector3 from, Vector3 to)
    {
        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range)
        {
            return true;
        }

        return false;
    }

    
    protected void GetInRange()
    {
        if (currentTarget == null)
            return;

        if(!moving)
        {
            destinationNode = null;
            List<Node> candidates = GridManager.Instance.GetNodesCloseTo(currentTarget.CurrentNode);
            candidates = candidates.OrderBy(x => Vector3.Distance(x.worldPosition, this.transform.position)).ToList();
            for(int i = 0; i < candidates.Count;i++)
            {
                if (!candidates[i].IsOccupied)
                {
                    destinationNode = candidates[i];
                    break;
                }
            }
            if (destinationNode == null)
                return;

            var path = GridManager.Instance.GetPath(currentNode, destinationNode);
            if (path == null && path.Count >= 1)
                return;

            if (path[1].IsOccupied)
                return;

            path[1].SetOccupied(true);
            destinationNode = path[1];            
        }

        moving = !MoveTowards(destinationNode);
        if(!moving)
        {
            //Free previous node
            currentNode.SetOccupied(false);
            SetCurrentNode(destinationNode);
        }
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    public bool DealDamage(int amount)
    {
        Debug.Log($"Claire damage dealt to {this.name}");
        baseHealth -= amount;

        if (baseHealth > 0 || dead)
        {
            return false;
        }
        
        dead = true;
        currentNode.SetOccupied(false);
        GameManager.Instance.EntityDead(this);
        return true;
    }

    protected virtual void Attack()
    {
        Debug.Log($"Claire attack function for {this.name}");
        if (!canAttack)
            return;

        animator.SetTrigger("Attack");
        return;
    }
 
}