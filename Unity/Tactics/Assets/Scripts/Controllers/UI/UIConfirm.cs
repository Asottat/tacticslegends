using UnityEngine;
using UnityEngine.UI;

public class UIConfirm : MonoBehaviour
{
    public Text TitleTextField;
    public Text MessageTextField;
    public UISimpleTrigger ConfirmButton;
    public UISimpleTrigger CancelButton;

    public event System.EventHandler OnCofirm;
    public event System.EventHandler OnCancel;

    void Start()
    {
        ConfirmButton.OnClicked += Confirmed;
        CancelButton.OnClicked += Canceled;
    }

    public void SetData(string title, string message)
    {
        TitleTextField.text = title;
        MessageTextField.text = message;
    }

    private void Confirmed(object sender, System.EventArgs e)
    {
        OnCofirm?.Invoke(sender, e);
    }

    private void Canceled(object sender, System.EventArgs e)
    {
        OnCancel?.Invoke(sender, e);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        TitleTextField.text = string.Empty;
        MessageTextField.text = string.Empty;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
