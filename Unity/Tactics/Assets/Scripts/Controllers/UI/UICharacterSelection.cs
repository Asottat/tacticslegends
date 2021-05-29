using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelection : MonoBehaviour, IPointerClickHandler
{
    public RectTransform PortraitField;
    public Text NameField;
    public Text ClassField;
    public Text PowerField;
    public Text LevelField;
    public event System.EventHandler OnClicked;

    private long _characterId;

    public void SetCharacterData(CharacterGameplay cg)
    {
        _characterId = cg.BaseInfo.Id;
        NameField.text = cg.BaseInfo.Name;
        LevelField.text = cg.BaseInfo.Level.ToString();
        PowerField.text = string.Format("Power: <color=#FFFE84>{0}</color>", cg.Power.ToString("N0"));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(_characterId, System.EventArgs.Empty);
    }
}
