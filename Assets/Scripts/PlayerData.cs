
public class PlayerData : Manager<PlayerData>
{
    public int Money { get; private set; }

    public System.Action OnUpdate;

    private void Start()
    {
        Money = 7;
    }
    
    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        OnUpdate?.Invoke();
    }
}