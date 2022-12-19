using System.Collections.Generic;
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
    public float movementSpeed = 1f; //Attacks per second

    protected Team myTeam;
    protected Tribe myTribe;
    protected bool tribeBonus = false;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;

    public Node CurrentNode => currentNode;
    
    protected bool moving;
    protected Node destination;

    protected bool dead = false;
    protected bool canAttack = true;

    public void Setup(Team team, Node currentNode)
    {
        myTeam = team;
        this.currentNode = currentNode;
        transform.position = currentNode.worldPosition;
        currentNode.SetOccupied(true);
    }

    protected void Start()
    {
        GameManager.Instance.OnRoundStart += OnRoundStart;
        GameManager.Instance.OnRoundEnd += OnRoundEnd;
        GameManager.Instance.OnEntityDied += OnUnitDied;
    }

    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnUnitDied(BaseEntity diedUnity) { }
    
    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }
}