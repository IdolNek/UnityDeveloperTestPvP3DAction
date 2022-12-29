using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIInvincibleState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthBarImage;
    [SerializeField] private Color _blinkColor;
    [Range(1,5)][SerializeField] private float _blinkSpeed =3f;
    private Color _startColor = Color.red;
    private bool _isDamaged;
    private float _pinPongLenght = 1f;
    public void OnDamaged(float isDamaged—ountdownTime)
    {
        gameObject.SetActive(true);
        _isDamaged = true;
        StartCoroutine(DamagedCountdownTime(isDamaged—ountdownTime));

    }
    private void Update()
    {
        if (!_isDamaged) return;
        _healthBarImage.color = Color.Lerp(_startColor, _blinkColor, Mathf.PingPong(Time.time * _blinkSpeed, _pinPongLenght));
    }
    private IEnumerator DamagedCountdownTime(float isDamaged—ountdownTime)
    {
        yield return new WaitForSeconds(isDamaged—ountdownTime);
        _isDamaged = false;
        _healthBarImage.color = _startColor;
        gameObject.SetActive(false);
    }
}
