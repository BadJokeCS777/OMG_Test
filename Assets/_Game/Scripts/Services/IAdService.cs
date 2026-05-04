using System;

namespace Services
{
    public interface IAdService
    {
        bool IsRewardedAdReady { get; }
        void ShowRewardedAd(Action onRewarded, Action onFailed);
    }
}
