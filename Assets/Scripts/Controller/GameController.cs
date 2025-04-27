using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Weeks _weeks;

    [Header("Cards")] 
    [SerializeField] private CardModule[] _cardModules;

    [SerializeField] private List<GameObject> _activeAllocationCards;
    [SerializeField] private List<GameObject> _activeResearchCards;

    [Header("Game Settings")]
    [SerializeField] private int _startProcessPower = 5;
    [SerializeField] private int _startScience = 5;
    [SerializeField] private int _startIdea = 0;
    
    [Header("UI References")]
    public TMP_Text _totalProcessPowerText;
    [SerializeField] private TMP_Text _totalScienceText;
    [SerializeField] private Transform _researchCardParent;
    [SerializeField] private Transform _allocationCardParent;
    [SerializeField] private Slider _sdg13Bar;
    [SerializeField] private TMP_Text _totalIdeaText;

    [Header("Prefabs")]
    [SerializeField] private GameObject _researchCardPrefab;
    [SerializeField] private GameObject _allocationCardPrefab;

    [Header("Tutorial")]
    [SerializeField] private GameObject newResourcePage;
    [SerializeField] private GameObject newSDGPage;
    
    //Resources
    public int totalProcessPower { get; private set; }
    public float totalScience { get; private set; }
    public float totalIdea { get; private set; }

    private float totalSDG13;
    
    //Other
    public int processCap { get; private set; }
    public float scienceCap { get; private set; }
    public float weeklyTotalScienceAmount { get; private set; }
    public float weeklyTotalSDG13Amount { get; private set; }
    public float weeklyTotalIdeaAmount { get; private set; }

    public int _toAddScienceGoal;
    public int _toAddIdeaGoal;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        processCap = _startProcessPower;
        totalProcessPower = _startProcessPower;
        scienceCap = _startScience;

        updateUI();
        
        spawnAllocationCard(1);
        spawnResearchCard(2);
    }

    public void LastSDGFound()
    {
        newSDGPage.SetActive(true);
    }

    public void UpdateTotalScience(float value) {
        totalScience += value;

        if (totalScience > scienceCap)
            totalScience = scienceCap;
        
        updateUI();
    }
    
    public bool CheckScienceEnough(float value) {
        if(totalScience + value < 0)
            return false;
        return true;
    }

    public void UpdateTotalIdea(float value)
    {

        totalIdea += value;
        
        updateUI();
    }

    public void UpdateMemory(float value)
    {
        scienceCap += value;

        updateUI();
    }
    
    public bool CheckIdeaEnough(float value) {
        if(totalIdea + value < 0)
            return false;
        return true;
    }
    
    public void addProcessingPower(int value)
    {
        StartCoroutine(ChangeProcessTextColor());
        processCap += value;
        totalProcessPower += value;
        updateUI();
    }

    private IEnumerator ChangeProcessTextColor()
    {
        _totalProcessPowerText.color = Color.green;
        yield return new WaitForSeconds(0.25f);
        _totalProcessPowerText.color = Color.white;
    }

    //Spawn Cards
    public void spawnAllocationCard(int id)
    {
        foreach (CardModule card in _cardModules){
            if(card.id == id){
                GameObject newCard = Instantiate(_allocationCardPrefab, transform);
                newCard.transform.SetParent(_allocationCardParent);
                newCard.GetComponent<Allocation>().setCard(card);
                _activeAllocationCards.Add(newCard);
                
                if(card.id == 62)
                    newResourcePage.SetActive(true);
                return;
            }
        }
    }
    
    public void spawnResearchCard(int id){
        foreach(CardModule card in _cardModules){
            if(card.id == id){
                GameObject newCard = Instantiate(_researchCardPrefab, transform);
                newCard.transform.SetParent(_researchCardParent);
                newCard.GetComponent<ResearchCard>().setCard(card);
                _activeResearchCards.Add(newCard);
                return;
            }
        }
    }

    public void removeResearchCard(int id)
    {
        foreach (GameObject card in _activeResearchCards)
        {
            if (card.GetComponent<ResearchCard>()._cardModule.id == id)
            {
                _activeResearchCards.Remove(card);
                Destroy(card);
                return;
            }
        }
    }

    public void researchCard(int id)
    {
        foreach (GameObject card in _activeResearchCards)
        {
            if (card.GetComponent<ResearchCard>()._cardModule.id == id)
            {
                _activeResearchCards.Remove(card);
                Destroy(card);
                spawnAllocationCard(id);
                return;
            }
        }
    }
    
    //Process Week

    public void ProcessWeek()
    {
        _weeks.Processing();
        StartCoroutine(addResources(weeklyTotalScienceAmount, weeklyTotalIdeaAmount));
        foreach (GameObject allocationCard in _activeAllocationCards)
        {
            allocationCard.GetComponent<Allocation>().StartProcessingGoal();
        }
    }
    
    private IEnumerator addResources(float science, float idea)
    {
        float time = 0;
        float scaleModifierScience = 0.1f;
        float scaleModifierIdea = 0.1f;
        float startValue = 0.0f;
        float startAmountScience = totalScience;
        float startAmountIdea = totalIdea;
        float targetScience = science;
        float targetIdea = idea;
        float timeToLerp = 1f;
        
        if(weeklyTotalScienceAmount > 0)
            _totalScienceText.color = Color.green;
        
        if(weeklyTotalIdeaAmount > 0)
            _totalIdeaText.color = Color.green;

        while (time < timeToLerp)
        {
            scaleModifierScience = Mathf.Lerp(startValue, targetScience, time / timeToLerp);
            scaleModifierIdea = Mathf.Lerp(startValue, targetIdea, time / timeToLerp);
            if (totalScience < scienceCap)
                totalScience = startAmountScience + scaleModifierScience;
            else {
                totalScience = scienceCap;
                _totalScienceText.color = Color.white;
            }
            
            if (totalIdea < totalIdea + targetIdea)
                totalIdea = startAmountIdea + scaleModifierIdea;
            else {
                _totalScienceText.color = Color.white;
            }

            time += Time.deltaTime;
            updateUI();
            yield return null;
        }

        if (startAmountScience + science + _toAddScienceGoal < scienceCap)
            totalScience = startAmountScience + science + _toAddScienceGoal;
        else
            totalScience = scienceCap;
        
        totalIdea = startAmountIdea + idea + _toAddIdeaGoal;

        updateUI();
        UpdateBars();
        
        _totalIdeaText.color = Color.white;
        _totalScienceText.color = Color.white;
        _weeks.Ready();
        _toAddScienceGoal = 0;
        _toAddIdeaGoal = 0;
    }

    public void UpdateBars()
    {
        float tempAIFitness = 0;
        float tempSVPositive = 0;
        float tempSVNegative = 0;
        float tempPOPositive = 0;
        float tempPONegative = 0;

        foreach (GameObject card in _activeAllocationCards)
        {
            CardModule cardModule = card.GetComponent<Allocation>()._cardModule;
            
            if (weeklyTotalSDG13Amount != 0)
            {
                totalSDG13 += weeklyTotalSDG13Amount;
                weeklyTotalSDG13Amount = 0;
                _sdg13Bar.value = totalSDG13 / 100;
            }
            
            tempAIFitness += cardModule._aifEffect;
            tempSVPositive += cardModule._soPositiveEffect;
            tempSVNegative += cardModule._soNegativeEffect;
            tempPOPositive += cardModule._poPositiveEffect;
            tempPONegative += cardModule._poNegativeEffect;
        }
        
        BarController barController = BarController.Instance;
        barController.AIFitnessScoreUpdate(tempAIFitness);
        barController.addSVPositive(tempSVPositive);
        barController.addSVNegative(tempSVNegative);
        barController.addPOPositive(tempPOPositive);
        barController.addPONegative(tempPONegative);
    }

    public void updateWeeklyResources()
    {
        float tempTotalWeeklyScience = 0;
        float tempTotalWeeklySDG13 = 0;
        float tempTotalWeeklyIdea = 0;
        
        foreach (GameObject card in _activeAllocationCards)
        {
            Allocation allocation = card.GetComponent<Allocation>();
            float cardWeeklyScience = allocation._cardModule.weeklyScience;
            float cardWeeklySDG13 = allocation._cardModule.weeklySDG13;
            float cardWeeklyIdea = allocation._cardModule.weeklyIdea;

            if (allocation._selfAllocation != 0)
            {
                if (cardWeeklyScience > 0)
                    tempTotalWeeklyScience += cardWeeklyScience * allocation._selfAllocation;
                if (cardWeeklySDG13 > 0)
                    tempTotalWeeklySDG13 += (cardWeeklySDG13 / 10) * allocation._selfAllocation;
                if (cardWeeklyIdea > 0)
                    tempTotalWeeklyIdea += cardWeeklyIdea * allocation._selfAllocation;
            }
                
        }

        weeklyTotalIdeaAmount = tempTotalWeeklyIdea;
        weeklyTotalSDG13Amount = tempTotalWeeklySDG13;
        weeklyTotalScienceAmount = tempTotalWeeklyScience;
        updateUI();
    }

    public int getRemainingAlocation()
    {
        return totalProcessPower;
    }

    public void reduceAlocation()
    {
        totalProcessPower--;
        updateWeeklyResources();
    }

    public void increaseAlocation()
    {
        totalProcessPower++;
        updateWeeklyResources();
    }

    private void updateUI()
    {
        _totalProcessPowerText.text = totalProcessPower+"/"+processCap;
        _totalIdeaText.text = totalIdea.ToString("0.0")+"(+"+weeklyTotalIdeaAmount.ToString("0.0")+")";
        _totalScienceText.text = totalScience.ToString("0.0") + "/" + scienceCap+"(+"+weeklyTotalScienceAmount.ToString("0.0")+")";
        _sdg13Bar.value = totalSDG13 / 100;
    }
}
