using System;
using UnityEngine;

namespace Services
{
    public class CurrencyServiceStub : ICurrencyService
    {
        private int _coins = 30;

        public int Coins => _coins;

        public event Action<int> OnCoinsChanged;

        public bool TrySpend(int amount)
        {
            if (_coins < amount)
            {
                Debug.Log("[CurrencyServiceStub] Недостаточно монет.");
                return false;
            }

            _coins -= amount;
            OnCoinsChanged?.Invoke(_coins);
            Debug.Log($"[CurrencyServiceStub] Потрачено {amount} монет. Остаток: {_coins}");
            return true;
        }

        public void AddCoins(int amount)
        {
            _coins += amount;
            OnCoinsChanged?.Invoke(_coins);
        }
    }
}
