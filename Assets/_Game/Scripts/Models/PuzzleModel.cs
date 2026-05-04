using Common;
using UniRx;
using UnityEngine;

namespace Models
{
    public class PuzzleModel
    {
        public StringReactiveProperty Id { get; } = new();
        public ReactiveProperty<Sprite> Picture { get; } = new();
        public BoolReactiveProperty Locked { get; } = new();
        public FloatReactiveProperty Progress { get; } = new();
        public ReactiveProperty<Difficulty> LastStartedDifficulty { get; } = new(Difficulty.None);
        public BoolReactiveProperty UseRotation { get; } = new();
        public IntReactiveProperty FreeAttemptsLeft { get; } = new();
    }
}
