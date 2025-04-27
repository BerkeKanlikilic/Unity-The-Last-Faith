using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weeks : MonoBehaviour
{
    
    [Header("Game Variables")]
    [SerializeField] private int _remainingWeeks = 700;
    [SerializeField] private int _currentWeek = 0;
    
    [Header("UI References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _unspentBtn;
    [SerializeField] private GameObject _processingBtn;
    [SerializeField] private TMP_Text _topBarText;
    [SerializeField] private TMP_Text _currentWeekText;

    private static string ROTATING = "Rotating";
    
    private void Start()
    {
        StartCoroutine(UpdateUI());
    }

    public void EndWeek()
    {
        GameController.Instance.ProcessWeek();
    }

    public void Ready()
    {
        _animator.SetBool(ROTATING, false);
        _processingBtn.SetActive(false);
    }

    public void Processing()
    {
        _animator.SetBool(ROTATING, true);
        _processingBtn.SetActive(true);
        if (_remainingWeeks > 0) {
            _remainingWeeks--;
            _currentWeek++;
        }
    }

    private IEnumerator UpdateUI()
    {
        while (true)
        {
            if (GameController.Instance.totalProcessPower > 0)
                _unspentBtn.SetActive(true);
            else
                _unspentBtn.SetActive(false);

            _currentWeekText.text = "WEEK: " + _currentWeek;
            _topBarText.text = _remainingWeeks + " weeks left until the end of 2030";
            yield return null;
        }
    }
}
