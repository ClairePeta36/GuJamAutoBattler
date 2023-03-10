
public class PlayerData : Manager<PlayerData>
{
    public int Money { get; private set; }

    public System.Action OnUpdate;

    private void Awake()
    {
        base.Awake();
        Money = 3;
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

    public void winMoney(int amount)
    {
        Money += amount;
        OnUpdate?.Invoke();
    }
}