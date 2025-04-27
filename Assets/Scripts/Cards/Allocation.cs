using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Allocation : MonoBehaviour
{
    public CardModule _cardModule { get; private set; }

    private GameController _gameController;
    
    [Header("References")]
    [SerializeField] private TMP_Text _researchNameText;
    [SerializeField] private TMP_Text _sdgNumberText;
    [SerializeField] private RawImage _sdgColorImage;
    [SerializeField] private TMP_Text _allocationText;
    [SerializeField] private GameObject _scienceWeeklyObj;
    [SerializeField] private GameObject _sdg13WeeklyObj;
    [SerializeField] private GameObject _sdg17WeeklyObj;
    [SerializeField] private GameObject _ideaWeeklyObj;
    [SerializeField] private GameObject _goalProcessObj;
    [SerializeField] private GameObject _goalScienceObj;
    [SerializeField] private GameObject _goalIdeaObj;

    [Header("Weekly References")]
    [SerializeField] private TMP_Text _scienceWeeklyText;
    [SerializeField] private TMP_Text _sdg13WeeklyText;
    [SerializeField] private TMP_Text _sdg17WeeklyText;
    [SerializeField] private TMP_Text _ideaWeeklyText;
    
    [Header("Goal References")]
    [SerializeField] private TMP_Text _goalName;
    [SerializeField] private TMP_Text _startingGoal;
    [SerializeField] private TMP_Text _goalProcessText;
    [SerializeField] private TMP_Text _goalScienceText;
    [SerializeField] private TMP_Text _goalIdeaText;
    [SerializeField] private Image _goalPrizeBG;

    public int _selfAllocation { get; private set; }
    public int _goalTarget { get; private set; }
    public int _goalAmount { get; private set; }

    private int _nextGoalModifier;

    private void Awake()
    {
        _gameController = GameController.Instance;
    }

    private void Start()
    {
        _allocationText.text = _selfAllocation.ToString();
        _goalTarget = _cardModule._startingGoal;
        _nextGoalModifier = _cardModule._startingGoalModifier;
    }
    
    public void setCard(CardModule card){
        _cardModule = card;
        _researchNameText.text = _cardModule.cardName;
        _sdgNumberText.text = _cardModule.sdgNumber;
        _sdgColorImage.color = _cardModule.sdgColor;
        _goalName.text = _cardModule._goalName;
        _startingGoal.text = "0/" + _cardModule._startingGoal;

        if(_cardModule.weeklyScience > 0){
            _scienceWeeklyObj.SetActive(true);
            _scienceWeeklyText.text = "+" + _cardModule.weeklyScience;
        }

        if (_cardModule.weeklySDG13 > 0) {
            _sdg13WeeklyObj.SetActive(true);
            _sdg13WeeklyText.text = "+" + _cardModule.weeklySDG13;
        }

        if (_cardModule.weeklySDG17 > 0)
        {
            _sdg17WeeklyObj.SetActive(true);
            _sdg17WeeklyText.text = "+" + _cardModule.weeklySDG17;
        }

        if (_cardModule.weeklyIdea > 0) {
            _ideaWeeklyObj.SetActive(true);
            _ideaWeeklyText.text = "+" + _cardModule.weeklyIdea;
        }

        if (_cardModule._goalProcess > 0) {
            _goalProcessObj.SetActive(true);
            _goalProcessText.text = "+" + _cardModule._goalProcess;
        }

        if (_cardModule._goalScience > 0) {
            _goalScienceObj.SetActive(true);
            _goalScienceText.text = "+" + _cardModule._goalScience;
        }

        if (_cardModule._goalIdea > 0) {
            _goalIdeaObj.SetActive(true);
            _goalIdeaText.text = "+" + _cardModule._goalIdea;
        }
    }

    public void StartProcessingGoal()
    {
        if(_selfAllocation > 0)
            StartCoroutine(ProcessGoal());
    }

    private IEnumerator ProcessGoal()
    {
        float time = 0;
        int scaleModifier = 1;
        float startValue = scaleModifier;
        int startAmount = _goalAmount;
        float targetScience = _selfAllocation;
        float timeToLerp = 1f;

        while (time < timeToLerp)
        {
            scaleModifier = (int)Mathf.Lerp(startValue, targetScience + 1, time / timeToLerp);
            if (_goalAmount < _goalTarget) {
                _goalAmount = startAmount + scaleModifier;
                _startingGoal.text = _goalAmount.ToString("0") + "/" + _goalTarget;
            }
            else
            {
                _goalTarget += _nextGoalModifier;
                _goalAmount = startAmount + scaleModifier;
                _nextGoalModifier += _nextGoalModifier / 2;
                _startingGoal.text = _goalAmount.ToString("0") + "/" + _goalTarget;
                addGoalPrize();
                StartCoroutine(GoalPrizeTextColor());
            }
            
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void addGoalPrize() {
        if (_cardModule._goalProcess > 0)
        {
            _gameController.addProcessingPower(_cardModule._goalProcess);
        }

        if (_cardModule._goalScience > 0 &&_gameController.totalScience < _gameController.scienceCap)
        {
            _gameController._toAddScienceGoal += _cardModule._goalScience;
        }

        if (_cardModule._goalIdea > 0)
        {
            _gameController._toAddIdeaGoal += _cardModule._goalIdea;
        }
    }

    private IEnumerator GoalPrizeTextColor()
    {
        _goalPrizeBG.enabled = true;

        yield return new WaitForSeconds(0.25f);

        _goalPrizeBG.enabled = false;
    }

    public void raiseAllocation()
    {
        if(_gameController.getRemainingAlocation() == 0)
            return;
        _selfAllocation++;
        _allocationText.text = _selfAllocation.ToString();
        _gameController.reduceAlocation();
    }

    public void lowerAllocation()
    {
        if(_selfAllocation == 0)
            return;
        _selfAllocation--;
        _allocationText.text = _selfAllocation.ToString();
        _gameController.increaseAlocation();
    }
}
