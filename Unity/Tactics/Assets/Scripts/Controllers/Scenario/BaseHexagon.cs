using Assets.Scripts.Entities.LocalData;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseHexagon : MonoBehaviour, IPointerClickHandler
{
    public BattleSceneController BattleSceneController;
    public HexData HexData;

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleSceneController.HexagonClicked(HexData);
    }
}
