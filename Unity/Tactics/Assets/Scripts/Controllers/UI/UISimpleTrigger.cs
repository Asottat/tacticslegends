using UnityEngine;
using UnityEngine.EventSystems;

public class UISimpleTrigger : MonoBehaviour, IPointerClickHandler
{
    public event System.EventHandler OnClicked;
    public bool IsEnabled { get; private set; } = true;

    public void SetEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEnabled)
            OnClicked?.Invoke(null, System.EventArgs.Empty);
    }
}
