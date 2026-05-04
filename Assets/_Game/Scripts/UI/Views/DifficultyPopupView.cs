using Common;
using TMPro;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class DifficultyPopupView : ViewBase<DifficultyPopupViewModel>
    {
        [SerializeField] private Image _previewImage;
        [SerializeField] private PuzzleGridView _gridView;

        [Header("Difficulty Toggles")]
        [SerializeField] private ToggleGroup _difficultyToggleGroup;
        [SerializeField] private Toggle _easyToggle;
        [SerializeField] private Toggle _normalToggle;
        [SerializeField] private Toggle _hardToggle;

        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _startButtonText;

        [Header("Currency")]
        [SerializeField] private TMP_Text _coinsText;

        public override void BindViewModel(DifficultyPopupViewModel viewModel)
        {
            base.BindViewModel(viewModel);

            Subscribe(ViewModel.Puzzle.Picture, sprite => _previewImage.sprite = sprite);
            Subscribe(ViewModel.GridSize, size => _gridView.SetGrid(size, size));

            _easyToggle.onValueChanged.AddListener(isOn => { OnDifficultyChanged(isOn, Difficulty.Easy); });
            _normalToggle.onValueChanged.AddListener(isOn => { OnDifficultyChanged(isOn, Difficulty.Normal); });
            _hardToggle.onValueChanged.AddListener(isOn => { OnDifficultyChanged(isOn, Difficulty.Hard); });

            _easyToggle.group = _difficultyToggleGroup;
            _normalToggle.group = _difficultyToggleGroup;
            _hardToggle.group = _difficultyToggleGroup;
            _easyToggle.Select();

            Subscribe(ViewModel.StartButtonText, text => _startButtonText.text = text);
            Subscribe(ViewModel.CoinsText, text => _coinsText.text = text);
            Subscribe(ViewModel.CanStart, can => _startButton.interactable = can);
            
            _startButton.onClick.AddListener(() => ViewModel.TryStart());
            _closeButton.onClick.AddListener(() => ViewModel.Close());

            Subscribe(ViewModel.OnCloseClicked, _ => gameObject.SetActive(false));
        }

        protected override void OnDestroy()
        {
            _easyToggle.onValueChanged.RemoveAllListeners();
            _normalToggle.onValueChanged.RemoveAllListeners();
            _hardToggle.onValueChanged.RemoveAllListeners();
            _startButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        private void OnDifficultyChanged(bool isOn, Difficulty difficulty)
        {
            if (isOn) ViewModel.SelectDifficulty(difficulty);
        }
    }
}
