using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    //TODO: class only for test, not production
    //Replace on Sprite swap
    public class PuzzleGridView : MonoBehaviour
    {
        [SerializeField] private Image _gridImage;
        [SerializeField] private Color _lineColor = Color.white;
        [SerializeField] private int _lineThickness = 2;
        [SerializeField] private int _textureSize = 1024;

        private Texture2D _texture;
        private Sprite _sprite;

        private void Awake()
        {
            _texture = new Texture2D(_textureSize, _textureSize, TextureFormat.RGBA32, false);
            _sprite = Sprite.Create(_texture, new Rect(0, 0, _textureSize, _textureSize), new Vector2(0.5f, 0.5f), 100f);
            _gridImage.sprite = _sprite;
        }

        public void SetGrid(int columns, int rows)
        {
            int w = _textureSize;
            int h = _textureSize;

            Color32 clear = new Color32(0, 0, 0, 0);
            Color32 line = _lineColor;

            Color32[] pixels = new Color32[w * h];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = clear;

            for (int i = 0; i <= rows; i++)
            {
                int y = Mathf.RoundToInt((h / (float)rows) * i);
                for (int x = 0; x < w; x++)
                {
                    for (int t = 0; t < _lineThickness; t++)
                    {
                        int py = Mathf.Clamp(y + t - _lineThickness / 2, 0, h - 1);
                        pixels[py * w + x] = line;
                    }
                }
            }

            for (int i = 0; i <= columns; i++)
            {
                int x = Mathf.RoundToInt((w / (float)columns) * i);
                for (int y = 0; y < h; y++)
                {
                    for (int t = 0; t < _lineThickness; t++)
                    {
                        int px = Mathf.Clamp(x + t - _lineThickness / 2, 0, w - 1);
                        pixels[y * w + px] = line;
                    }
                }
            }

            _texture.SetPixels32(pixels);
            _texture.Apply();
        }

        private void OnDestroy()
        {
            if (_sprite != null)
                Destroy(_sprite);

            if (_texture != null)
                Destroy(_texture);
        }
    }
}
