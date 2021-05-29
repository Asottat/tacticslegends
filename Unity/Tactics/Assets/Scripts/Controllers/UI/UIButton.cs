using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject ActiveMark;
    public Image Icon;
    public GameObject LockIcon;

    public event System.EventHandler OnClicked;
    public bool IsEnabled { get; private set; } = true;

    private Color _iconColor;
    private bool _isReady = false;

    void Start()
    {
        _iconColor = Icon.color;
        _isReady = true;

        if (!IsEnabled)
            SetEnabled(false);
    }

    public void SetEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;

        if (_isReady)
        {
            ActiveMark.SetActive(IsEnabled);
            _iconColor.a = IsEnabled ? 1f : 0.25f;
            Icon.color = _iconColor;

            if (LockIcon != null)
                LockIcon.SetActive(!IsEnabled);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEnabled)
            OnClicked?.Invoke(null, System.EventArgs.Empty);
    }
}
