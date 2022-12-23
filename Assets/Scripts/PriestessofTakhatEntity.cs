using UnityEngine;

// Ranged
// After another Anubian unit within range dies, gain +2 strength for this battle. 

public class PriestessofTakhatEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        FindTarget();
        this.range = 10;
    }
    
    protected override void OnUnitDied(BaseEntity entity)
    {
        if (entity.getTribe() != Tribe.Anubian || entity.GetTeam() != this.myTeam)
        {
            return;
        }
        if (Vector3.Distance(entity.transform.position, this.transform.position) < this.range)
        {
            this.baseDamage += 2;
        }
    }
    
    
    protected override void OnEntityPurchased(PurchaseCard entity)
    {
        if (entity.tribe.text == "Wild")
        {
            GameManager.Instance.cardShop.GenerateSingleCard(this._entityData);
        }
        Destroy(entity);
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

