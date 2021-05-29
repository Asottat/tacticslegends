using Assets.Scripts.Enums;
using UnityEngine;

public class HexMark : MonoBehaviour
{
    public HexMarkType HexMarkType;
    public Transform DefaultMark;
    public Transform GreenMark;
    public Transform SelectionMark;
    public int CoordX;
    public int CoordY;

    private bool _selectedAnim = false;
    private float _selectionSize = 1f;
    private float _animDirection = 1f;
    private float _animSpeed = 0.25f;

    void Update()
    {
        if (_selectedAnim)
        {
            _selectionSize = Mathf.Clamp(_selectionSize + (Time.deltaTime * _animSpeed * _animDirection), 1f, 1.1f);

            if (_selectionSize >= 1.1f || _selectionSize <= 1f)
                _animDirection *= -1f;

            SelectionMark.transform.localScale = new Vector3(_selectionSize, 1f, _selectionSize);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (HexMarkType == HexMarkType.Attack
            || HexMarkType == HexMarkType.MoveOption
            || HexMarkType == HexMarkType.SetPosition)
        {
            _selectedAnim = isSelected;

            DefaultMark.gameObject.SetActive(!isSelected);
            GreenMark.gameObject.SetActive(isSelected);
            SelectionMark.gameObject.SetActive(isSelected);
        }
    }
}
