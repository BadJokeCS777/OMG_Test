using System;

namespace Services
{
    public interface ICurrencyService
    {
        int Coins { get; }
        bool TrySpend(int amount);
        event Action<int> OnCoinsChanged;
    }
}
