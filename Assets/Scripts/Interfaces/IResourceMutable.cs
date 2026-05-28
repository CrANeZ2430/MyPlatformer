public interface IResourceMutable
{
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int MaxMana { get; }
    int CurrentMana { get; }
    int Coins { get; }

    void AddHealth(int healthAmount);
    void AddMana(int manaAmount);
    void AddCoins(int coinsAmount);
}
