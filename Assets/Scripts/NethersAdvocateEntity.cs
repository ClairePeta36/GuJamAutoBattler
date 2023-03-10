
//Afterlife: Summon a 1/1 Skeleton. 

using UnityEngine;

public class NethersAdvocateEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        FindTarget();
    }
    
    protected override void OnUnitDied(BaseEntity entity)
    {
        if (entity == this)
        {
            int position = GridManager.Instance.GetFreeNode(Team.Team1).index;
            Transform abc = GridManager.Instance.allTiles[position].transform;
            GameManager.Instance.cardShop.GenerateSingleCard(GameManager.Instance.EntityDatabase.allEntities[0], overRidePosition: abc);
        }
    }
    
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
        }
        else
        {
            animator?.SetBool("Attack", false);
            // no enemy in range we need to keep moving
            GetInRange();
        }
    }
        
    protected override void Attack()
    {
        base.Attack();

        currentTarget.DealDamage(baseDamage);
    }
}

