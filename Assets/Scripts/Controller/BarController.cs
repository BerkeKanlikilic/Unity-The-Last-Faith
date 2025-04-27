using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public static BarController Instance { get; private set; }
    
    [Header("Game Options")]
    [SerializeField] private float _fitnessStartingPercent;
    [SerializeField] private float _defaultFitnessScoreReduce;

    [Header("References")]
    [SerializeField] private Slider _aiFitnessScoreBar;
    
    [Header("SO Bar References")]
    [SerializeField] private Slider _svOpinionPositiveBar;
    [SerializeField] private Slider _svOpinionNegativeBar;
    [SerializeField] private TMP_Text _svOpinionPositiveText;
    [SerializeField] private TMP_Text _svOpinionNeutralText;
    [SerializeField] private TMP_Text _svOpinionNegativeText;
    
    [Header("PO Bar References")]
    [SerializeField] private Slider _pOpinionPositiveBar;
    [SerializeField] private Slider _pOpinionNegativeBar;
    [SerializeField] private TMP_Text _pOpinionPositiveText;
    [SerializeField] private TMP_Text _pOpinionNeutralText;
    [SerializeField] private TMP_Text _pOpinionNegativeText;
    
    //AI Fitness Score
    private float _fitnessPercent;
    
    //Supervisor Opinion
    private float _svPositive;
    private float _svNeutral = 100f;
    private float _svNegative;
    
    //Public Opinion
    private float _poPositive;
    private float _poNeutral = 100f;
    private float _poNegative;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _fitnessPercent = _fitnessStartingPercent;
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AIFitnessScoreUpdate(5);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AIFitnessScoreUpdate(-5);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AIFitnessScoreUpdate(0);
        }
    }

    private void UpdateUI()
    {
        //Fitness
        _aiFitnessScoreBar.value = _fitnessPercent / 100;
        
        //Supervisor
        _svOpinionPositiveBar.value = _svPositive / 100;
        _svOpinionNegativeBar.value = _svNegative / 100;
        _svOpinionPositiveText.text = _svPositive.ToString("0.0") + "%";
        _svOpinionNeutralText.text = _svNeutral.ToString("0.0") + "%";
        _svOpinionNegativeText.text = _svNegative.ToString("0.0") + "%";
        
        //Public
        _pOpinionPositiveBar.value = _poPositive / 100;
        _pOpinionNegativeBar.value = _poNegative / 100;
        _pOpinionPositiveText.text = _poPositive.ToString("0.0") + "%";
        _pOpinionNeutralText.text = _poNeutral.ToString("0.0") + "%";
        _pOpinionNegativeText.text = _poNegative.ToString("0.0") + "%";
    }

    public void AIFitnessScoreUpdate(float value) {
        _fitnessPercent += value - _defaultFitnessScoreReduce;
        _aiFitnessScoreBar.value = _fitnessPercent / 100;
    }

    public void addSVPositive(float percent)
    {
        if (_svNeutral > percent) {
            _svNeutral -= percent;
            _svOpinionNeutralText.text = _svNeutral.ToString("0.0") + "%";
        } else
        {
            float remaining = Mathf.Abs(_svNeutral - percent);
            _svNeutral = 0;
            _svOpinionNeutralText.text = "0.0%";
            if (_svNegative - remaining > 0) {
                _svNegative -= remaining;
                _svOpinionNegativeText.text = _svNegative.ToString("0.0") + "%";
                _svOpinionNegativeBar.value = _svNegative / 100;
            } else {
                _svNegative = 0;
                _svOpinionNegativeText.text = _svNegative.ToString("0.0") + "%";
                _svOpinionNegativeBar.value = _svNegative / 100;
            }
        }

        if (_svPositive < 100 - percent) {
            _svPositive += percent;
            _svOpinionPositiveBar.value = _svPositive / 100;
            _svOpinionPositiveText.text = _svPositive.ToString("0.0") + "%";
        } else {
            _svPositive = 100;
            _svOpinionPositiveBar.value = 1;
            _svOpinionPositiveText.text = "100.0%";
        }
    }

    public void addSVNegative(float percent)
    {
        if (_svNeutral > percent) {
            _svNeutral -= percent;
            _svOpinionNeutralText.text = _svNeutral.ToString("0.0") + "%";
        } else {
            float remaining = Mathf.Abs(_svNeutral - percent);
            _svNeutral = 0;
            _svOpinionNeutralText.text = "0.0%";
            if (_svPositive - remaining > 0) {
                _svPositive -= remaining;
                _svOpinionPositiveText.text = _svPositive.ToString("0.0") + "%";
                _svOpinionNegativeBar.value = _svPositive / 100;
            } else {
                _svPositive = 0;
                _svOpinionPositiveText.text = _svPositive.ToString("0.0") + "%";
                _svOpinionPositiveBar.value = _svPositive / 100;
            }
        }

        if (_svNegative < 100 - percent) {
            _svNegative += percent;
            _svOpinionNegativeBar.value = _svNegative / 100;
            _svOpinionNegativeText.text = _svNegative.ToString("0.0") + "%";
        } else {
            _svNegative = 100;
            _svOpinionNegativeBar.value = 1;
            _svOpinionNegativeText.text = "100.0%";
        }
    }
    
    public void addPOPositive(float percent)
    {
        if (_poNeutral > percent) {
            _poNeutral -= percent;
            _pOpinionNeutralText.text = _poNeutral.ToString("0.0") + "%";
        } else
        {
            float remaining = Mathf.Abs(_poNeutral - percent);
            _poNeutral = 0;
            _pOpinionNeutralText.text = "0.0%";
            if (_poNegative - remaining > 0) {
                _poNegative -= remaining;
                _pOpinionNegativeText.text = _poNegative.ToString("0.0") + "%";
                _pOpinionNegativeBar.value = _poNegative / 100;
            } else {
                _poNegative = 0;
                _pOpinionNegativeText.text = _poNegative.ToString("0.0") + "%";
                _pOpinionNegativeBar.value = _poNegative / 100;
            }
        }

        if (_poPositive < 100 - percent) {
            _poPositive += percent;
            _pOpinionPositiveBar.value = _poPositive / 100;
            _pOpinionPositiveText.text = _poPositive.ToString("0.0") + "%";
        } else {
            _poPositive = 100;
            _pOpinionPositiveBar.value = 1;
            _pOpinionPositiveText.text = "100.0%";
        }
    }

    public void addPONegative(float percent)
    {
        if (_poNeutral > percent) {
            _poNeutral -= percent;
            _pOpinionNeutralText.text = _poNeutral.ToString("0.0") + "%";
        } else {
            float remaining = Mathf.Abs(_poNeutral - percent);
            _poNeutral = 0;
            _pOpinionNeutralText.text = "0.0%";
            if (_poPositive - remaining > 0) {
                _poPositive -= remaining;
                _pOpinionPositiveText.text = _poPositive.ToString("0.0") + "%";
                _pOpinionNegativeBar.value = _poPositive / 100;
            } else {
                _poPositive = 0;
                _pOpinionPositiveText.text = _poPositive.ToString("0.0") + "%";
                _pOpinionPositiveBar.value = _poPositive / 100;
            }
        }

        if (_poNegative < 100 - percent) {
            _poNegative += percent;
            _pOpinionNegativeBar.value = _poNegative / 100;
            _pOpinionNegativeText.text = _poNegative.ToString("0.0") + "%";
        } else {
            _poNegative = 100;
            _pOpinionNegativeBar.value = 1;
            _pOpinionNegativeText.text = "100.0%";
        }
    }
}
