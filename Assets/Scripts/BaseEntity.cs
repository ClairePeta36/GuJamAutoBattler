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

    public int baseDamage = 1;
    public int baseHealth = 3;
    [Range(1, 5)]
    public int range = 1;
    public float attackSpeed = 1f; //Attacks per second
    float movementSpeed = 10f; //Movement per second

    protected Team myTeam;
    protected Tribe myTribe;
    protected bool tribeBonus = false;
    public BaseEntity currentTarget = null;
    protected Node currentNode;

    public Node CurrentNode => currentNode;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsEnemyInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    
    protected bool moving;
    protected Node destinationNode;

    protected bool dead = false;
    protected bool canAttack = true;


    private void OnRoundStart()
    {
        FindTarget();
    }
    public void Setup(Team team, Node currentNode)
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
        transform.position += new Vector3(-5, 0, -5);
        currentNode.SetOccupied(true);
    }

    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnEntityDied += OnUnitDied;
    }

    //protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnUnitDied(BaseEntity diedUnity) { }

    void FixedUpdate()
    {
        //this is for testing will be overridden in the new classes we create
        if (!GameManager.Instance.GetIsGameRunning())
        {
            return;
        }
        // first we check if we have an active enemy target if not we find one
        if (!HasEnemy)
        {
            Debug.Log($"Claire fixed update first if");
            FindTarget();
        }
        
        // we then want to check is this enemy target we are aiming for in range to start attacking
        if (IsEnemyInRange && !moving)
        {
            Debug.Log($"Claire fixed update second if");
            if (canAttack)
            {
                //attack and deal damage
                Attack();
                currentTarget.DealDamage(baseDamage);
            }
        }
        else
        {
            Debug.Log($"Claire fixed update first else");
            // no enemy in range we need to keep moving
            GetInRange();
        }
        
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
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }
        }

        currentTarget = entity;
    }

    protected bool MoveTowards(Node nextNode)
    {
        Vector3 direction = (nextNode.worldPosition - this.transform.position);
        if(direction.sqrMagnitude <= 0.005f)
        {
            transform.position = nextNode.worldPosition;
            animator.SetBool("walking", false);
            return true;
        }
        animator.SetBool("walking", true);

        this.transform.position += direction.normalized * (movementSpeed * Time.deltaTime);
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

        if (myTeam == Team.Team1)
        {
            Debug.Log($"Claire end of function position is {transform.position}");
        }
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }
    
    private void DealDamage(int amount)
    {
        baseHealth -= amount;

        if (baseHealth > 0 || dead)
        {
            return;
        }
        
        dead = true;
        currentNode.SetOccupied(false);
        GameManager.Instance.EntityDead(this);
    }

    protected virtual void Attack()
    {
        if (!canAttack)
            return;

        animator.SetTrigger("attack");
    }
 
}