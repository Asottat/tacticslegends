using UnityEngine;
using UnityEngine.UI;

public class UIToast : MonoBehaviour
{
    public RectTransform ThisTransform;
    public Text TextField;
    public Image Background;

    private float? _timeToMove;
    private bool _isMoving = false;
    private float _factorY = 0;
    private Color _bgColor;
    private float _alpha = 1;

    void Start()
    {
        _bgColor = Background.color;
    }

    void Update()
    {
        if (_timeToMove.HasValue && _timeToMove.Value <= Time.time)
        {
            _timeToMove = null;
            _isMoving = true;
        }

        if (_isMoving)
        {
            var posY = ThisTransform.anchoredPosition.y;
            if (posY < 250f)
            {
                _factorY = Mathf.Lerp(_factorY, 100f, 0.1f * Time.deltaTime);
                ThisTransform.anchoredPosition = new Vector2(0, posY + (_factorY / 10f));

                if (_alpha > 0)
                {
                    _alpha = Mathf.Clamp(1f - (posY / 200f), 0f, 1f);

                    _bgColor.a = _alpha;
                    Background.color = _bgColor;

                    TextField.canvasRenderer.SetAlpha(_alpha);
                }
            }
            else
            {
                _isMoving = false;
                Destroy(gameObject);
            }
        }
    }

    public void SetText(string text, bool isStatic = false)
    {
        TextField.text = text;
        var size = Mathf.Clamp(TextField.preferredWidth * 1.2f, 200f, 500f);
        ThisTransform.sizeDelta = new Vector2(size, ThisTransform.sizeDelta.y);

        if (!isStatic)
            _timeToMove = Time.time + 1f;
    }

    public void ReleaseText()
    {
        _timeToMove = 0;
    }
}
