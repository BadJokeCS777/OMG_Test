using UnityEngine;

namespace Common
{
    //TODO: доработать до структур данных под каждую сложность
    [CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Game/Difficulty Config")]
    public class DifficultyConfig : ScriptableObject
    {
        [field: SerializeField] public int EasyCost { get; private set; }
        [field: SerializeField] public int NormalCost { get; private set; }
        [field: SerializeField] public int HardCost { get; private set; }

        [field: SerializeField] public int EasyGridSize { get; private set; } = 3;
        [field: SerializeField] public int NormalGridSize { get; private set; } = 4;
        [field: SerializeField] public int HardGridSize { get; private set; } = 5;

        public int GetCost(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => EasyCost,
                Difficulty.Normal => NormalCost,
                Difficulty.Hard => HardCost,
                _ => 0
            };
        }

        public int GetGridSize(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => EasyGridSize,
                Difficulty.Normal => NormalGridSize,
                Difficulty.Hard => HardGridSize,
                _ => 3
            };
        }
    }
}
