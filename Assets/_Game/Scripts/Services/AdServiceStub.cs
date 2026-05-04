using System;
using UnityEngine;

namespace Services
{
    public class AdServiceStub : IAdService
    {
        public bool IsRewardedAdReady => true;

        public void ShowRewardedAd(Action onRewarded, Action onFailed)
        {
            Debug.Log("[AdServiceStub] Показ рекламы (заглушка)...");
            onRewarded?.Invoke();
        }
    }
}
