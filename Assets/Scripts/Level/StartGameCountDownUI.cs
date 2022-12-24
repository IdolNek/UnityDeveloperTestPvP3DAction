using System.Collections;
using TMPro;
using UnityEngine;

public class StartGameCountDownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCountDown;
    private float _secondForWait = 0.5f;
    public void StartCountDown()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(StartCountDownCoroutine());
    }
    private IEnumerator StartCountDownCoroutine()
    {
        UpdateTaxt("Loading");
        yield return new WaitForSeconds(_secondForWait);
        UpdateTaxt("3");
        yield return new WaitForSeconds(_secondForWait);
        UpdateTaxt("2");
        yield return new WaitForSeconds(_secondForWait);
        UpdateTaxt("1");
        yield return new WaitForSeconds(_secondForWait);
        UpdateTaxt("Begin");
        yield return new WaitForSeconds(_secondForWait);
        this.gameObject.SetActive(false);
    }
    private void UpdateTaxt(string text)
    {
        _textCountDown.text = text;
    }
}
