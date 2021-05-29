using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class UIAttackPreview : MonoBehaviour
{
    public Text TextField;
    private string _baseText;

    public void ShowValues(float accuracy, int damage, AttackType type)
    {
        string typeDesc = string.Empty;
        string typeColor = "#FFFE84";

        switch (type)
        {
            case AttackType.Normal: typeDesc = AppLanguage.GetText(12); break;
            case AttackType.FromBehind: typeDesc = AppLanguage.GetText(13); typeColor = "#FF46FF"; break;
        }

        TextField.text = string.Format(GetBaseText(), accuracy, damage.ToString("N0"), typeColor, typeDesc);
        gameObject.SetActive(true);
    }

    private string GetBaseText()
    {
        if (string.IsNullOrEmpty(_baseText))
        {
            _baseText = string.Concat(AppLanguage.GetText(9),
                ": <color=#FFFE84>{0}%</color>\n",
                AppLanguage.GetText(10),
                ": <color=#FFFE84>{1}</color>\n",
                AppLanguage.GetText(11),
                ": <color={2}>{3}</color>");
        }

        return _baseText;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
