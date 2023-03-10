
public class BadgerEntity : BaseEntity
{
    //When you summon another Wild creature, grow by (1).
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        FindTarget();
    }
    
    protected override void OnEntityAdded(BaseEntity entity)
    {
        base.OnEntityAdded(entity);
        if (entity.getTribe() == Tribe.Wild && entity.GetTeam() == this.myTeam)
        {
            this.IncreaseQuantity(1);
            GameManager.Instance.OnEntityCreated(this, this.spawnpositions[this.GetQuantity() - 1]);
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

