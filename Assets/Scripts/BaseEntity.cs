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
    int range = 11;
    public float attackSpeed = 1f; //Attacks per second
    float movementSpeed = 10f; //Movement per second

    protected Team myTeam;
    protected Tribe myTribe;
    protected bool tribeBonus = false;
    public BaseEntity currentTarget = null;
    public Vector3 currentTargetPosition;
    protected Node currentNode;

    public Node CurrentNode => currentNode;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsEnemyInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    
    protected bool moving;
    protected Node destinationNode;

    protected bool dead = false;
    protected bool canAttack = true;

    public DraggableEntity draggableEntity;


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
            FindTarget();
        }
        
        // we then want to check is this enemy target we are aiming for in range to start attacking
        if (IsEnemyInRange && !moving)
        {
            if (!canAttack)
            {
                return;
            }
            
            //attack and deal damage
            Attack();
            //currentTarget.DealDamage(baseDamage);
        }
        else
        {
            // no enemy in range we need to keep moving
            GetInRange();
        }
        
    }


    private void FindTarget()
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
        if (currentTarget != null)
        {
            currentTargetPosition = currentTarget.transform.position;
        }
        
    }

    private bool MoveTowards(Node nextNode)
    {
        Vector3 direction = (currentTarget.transform.position - this.transform.position);

        if (direction.magnitude < 5f)
            return false;

        if(direction.sqrMagnitude <= 0.005f)
        {
            transform.position = currentTarget.transform.position;
            animator.SetBool("walking", false);
            return true;
        }
        animator.SetBool("walking", true);

        this.transform.position += direction.normalized * (movementSpeed * Time.deltaTime);
        return false;
    }

    private void GetInRange()
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

        animator.SetTrigger("idle");
        var lookPosition = currentTarget.transform.position - this.transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
    }
 
}