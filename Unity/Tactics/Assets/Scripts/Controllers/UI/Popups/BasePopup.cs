using Assets.Scripts.Enums;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    [Header("Base Popup Settings")]
    public RectTransform ContentArea;
    public RectTransform ScrollContentArea;
    public GameObject LoadObject;
    public PopupType PopupType;
    public UISimpleTrigger CloseButton;

    public event System.EventHandler OnUserClose;

    public virtual void Start()
    {
        CloseButton.OnClicked += (s, e) => UserClose();
    }

    public void SetLoading(bool active)
    {
        LoadObject.SetActive(active);
    }

    private void UserClose()
    {
        OnUserClose?.Invoke(null, System.EventArgs.Empty);
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    protected void ClearScrollContent()
    {
        if (ScrollContentArea != null)
        {
            var cc = ScrollContentArea.childCount;

            if (cc > 0)
            {
                for (var i = (cc - 1); i >= 0; i--)
                    Destroy(ScrollContentArea.GetChild(i).gameObject);
            }
        }
    }

    protected void CalculateScrollContentSize()
    {
        if (PopupType == PopupType.VerticalScroll)
        {
            //TODO: Efetuar
        }
    }

    protected void SetScrollContentSize(float size)
    {
        if (PopupType == PopupType.VerticalScroll)
            ScrollContentArea.sizeDelta = new Vector2(ScrollContentArea.sizeDelta.x, size);
        else if (PopupType == PopupType.VerticalScroll)
            ScrollContentArea.sizeDelta = new Vector2(size, ScrollContentArea.sizeDelta.y);
    }
}
