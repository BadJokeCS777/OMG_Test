using Common;
using Models;
using Services;
using UI.ViewModels;
using UI.Views;
using UnityEngine;

namespace Scripts
{
    public class DifficultyPopupTest : MonoBehaviour
    {
        [SerializeField] private DifficultyPopupView _popupPrefab;
        [SerializeField] private Transform _canvas;
        [SerializeField] private Sprite[] _testSprites;
        [SerializeField] private DifficultyConfig _difficultyConfig;

        private void Start()
        {
            var puzzleModel = new PuzzleModel
            {
                Id =
                {
                    Value = "puzzle_01"
                },
                Picture =
                {
                    Value = _testSprites[Random.Range(0, _testSprites.Length)]
                },
                FreeAttemptsLeft =
                {
                    Value = 1
                }
            };

            var currency = new CurrencyServiceStub();
            var adService = new AdServiceStub();

            var vm = new DifficultyPopupViewModel(puzzleModel, _difficultyConfig, currency, adService);
            var popup = Instantiate(_popupPrefab, _canvas);
            popup.BindViewModel(vm);
        }
    }
}
