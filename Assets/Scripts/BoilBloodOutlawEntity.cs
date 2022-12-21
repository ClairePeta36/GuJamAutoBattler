
public class BoilBloodOutlawEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        FindTarget();
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
        
        //Permanently gain +1 strength after killing an enemy. 
        
        if(currentTarget.DealDamage(baseDamage))
        {
            baseDamage += 1;
        }
    }
        
        
}

