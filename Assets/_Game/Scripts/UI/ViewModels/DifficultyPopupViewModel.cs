using System;
using Common;
using Models;
using Services;
using UniRx;
using UnityEngine;

namespace UI.ViewModels
{
    public class DifficultyPopupViewModel : ViewModelBase
    {
        public PuzzleModel Puzzle { get; }

        public ReactiveProperty<Difficulty> SelectedDifficulty { get; } = new(Difficulty.None);
        public ReactiveProperty<StartCostType> CurrentStartType { get; } = new(StartCostType.Free);
        public StringReactiveProperty StartButtonText { get; } = new("Играть");
        public BoolReactiveProperty CanStart { get; } = new(true);
        public IntReactiveProperty GridSize { get; } = new(3);
        public StringReactiveProperty CoinsText { get; } = new("0");

        public Subject<Unit> OnCloseClicked { get; } = new();

        private readonly DifficultyConfig _difficultyConfig;
        private readonly ICurrencyService _currencyService;
        private readonly IAdService _adService;

        public DifficultyPopupViewModel(PuzzleModel puzzle, DifficultyConfig difficultyConfig, ICurrencyService currencyService, IAdService adService)
        {
            Puzzle = puzzle;
            _difficultyConfig = difficultyConfig;
            _currencyService = currencyService;
            _adService = adService;

            Subscribe(SelectedDifficulty, _ => RefreshStartState());
            Subscribe(Puzzle.FreeAttemptsLeft, _ => RefreshStartState());

            if (_currencyService != null)
                _currencyService.OnCoinsChanged += _ => RefreshStartState();
        }

        public void SelectDifficulty(Difficulty difficulty)
        {
            SelectedDifficulty.Value = difficulty;
        }

        public void TryStart()
        {
            var type = CurrentStartType.Value;
            int cost = GetCost();

            switch (type)
            {
                case StartCostType.Free:
                    Puzzle.FreeAttemptsLeft.Value = Math.Max(0, Puzzle.FreeAttemptsLeft.Value - 1);
                    OnStartClicked(type);
                    break;

                case StartCostType.Coins:
                    if (_currencyService != null && _currencyService.TrySpend(cost))
                        OnStartClicked(type);
                    break;

                case StartCostType.Ad:
                    _adService?.ShowRewardedAd(
                        () => OnStartClicked(type),
                        () => Debug.Log("[DifficultyPopupViewModel] Реклама не просмотрена"));
                    break;
            }
        }

        public void Close()
        {
            OnCloseClicked.OnNext(Unit.Default);
        }

        private void RefreshStartState()
        {
            var type = GetAvailableStartType(SelectedDifficulty.Value);
            CurrentStartType.Value = type;
            StartButtonText.Value = GetStartButtonText(type);
            CoinsText.Value = _currencyService != null ? $"{_currencyService.Coins}" : "0";
            CanStart.Value = type switch
            {
                StartCostType.Free => true,
                StartCostType.Coins => _currencyService != null && _currencyService.Coins >= GetCost(),
                StartCostType.Ad => _adService is { IsRewardedAdReady: true },
                _ => true
            };
            GridSize.Value = _difficultyConfig != null ? _difficultyConfig.GetGridSize(SelectedDifficulty.Value) : 3;
        }

        private StartCostType GetAvailableStartType(Difficulty difficulty)
        {
            bool hasFreeAttempts = Puzzle.FreeAttemptsLeft.Value > 0;

            if (hasFreeAttempts)
                return StartCostType.Free;

            int cost = GetCost();
            if (_currencyService != null && _currencyService.Coins >= cost)
                return StartCostType.Coins;

            return StartCostType.Ad;
        }

        private string GetStartButtonText(StartCostType type)
        {
            return type switch
            {
                StartCostType.Free => "Играть бесплатно",
                StartCostType.Coins => $"Играть за {GetCost()} монет",
                StartCostType.Ad => "Играть за рекламу",
                _ => "Играть"
            };
        }

        private int GetCost()
        {
            return 20;
        }

        private void OnStartClicked(StartCostType type)
        {
            Debug.Log($"Старт паззла: {Puzzle.Id.Value}, сложность: {SelectedDifficulty.Value}, тип: {type}");
        }
    }
}
