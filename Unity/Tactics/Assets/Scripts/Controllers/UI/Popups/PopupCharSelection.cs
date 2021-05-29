using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

public class PopupCharSelection : BasePopup
{
    private Object _characterSelectionPrefab;
    private float _posX_col1 = 10f;
    private float _posX_col2 = 340f;
    private float _posY_start = -10f;
    private float _posY_increment = -110f;
    private int _currentRow = 0;

    private bool _isReady = false;
    private List<CharacterGameplay> _delayedChars;

    public event System.EventHandler OnSelected;

    public override void Start()
    {
        base.Start();
        _characterSelectionPrefab = Resources.Load(AppConts.UIComponentsPath.CHARACTER_SELECTION);
        _isReady = true;

        if (_delayedChars != null)
            SetCharacterData(_delayedChars);
    }

    public void SetCharacterData(List<CharacterGameplay> chars)
    {
        if (!_isReady)
        {
            _delayedChars = chars;
            return;
        }

        _currentRow = 0;
        ClearScrollContent();

        var cCount = 0;
        foreach (var c in chars)
        {
            cCount++;
            var posX = cCount % 2 == 0 ? _posX_col2 : _posX_col1;
            var posY = _posY_start + (_posY_increment * _currentRow);

            var go = Instantiate(_characterSelectionPrefab, ScrollContentArea) as GameObject;
            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(posX, posY);
            var ctrl = go.GetComponent<UICharacterSelection>();
            ctrl.SetCharacterData(c);
            ctrl.OnClicked += Ctrl_OnClicked;

            if (cCount % 2 == 0)
                _currentRow++;
        }
        _delayedChars = null;
        var scrollSize = Mathf.Abs(_posY_start + (_posY_increment * Mathf.Ceil((float)cCount / 2f)));
        SetScrollContentSize(scrollSize);
    }

    private void Ctrl_OnClicked(object sender, System.EventArgs e)
    {
        OnSelected?.Invoke(sender, e);
    }
}
