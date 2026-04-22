using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIPulse : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _pulseDuration;
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    private bool _isFirstColor = true;

    private void Start() => Pulse();

    private void Pulse()
    {
        Color newColor = _isFirstColor ? _secondColor : _firstColor;

        _text.DOColor(newColor, _pulseDuration)
            .OnComplete(Pulse);

        _isFirstColor = !_isFirstColor;
    }
}
