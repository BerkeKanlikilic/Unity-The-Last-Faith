using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ResearchCard : MonoBehaviour
{
    public CardModule _cardModule { get; private set; }
    [SerializeField] private TMP_Text _researchNameText;
    [SerializeField] private TMP_Text _researchDescriptionText;
    [SerializeField] private TMP_Text _sdgNumberText;
    [SerializeField] private UnityEngine.UI.RawImage _sdgColorImage;
    
    [Header("Weekly Gain")]
    [SerializeField] private GameObject Science;
    [SerializeField] private TMP_Text _scienceGainText;
    [SerializeField] private GameObject SDG13;
    [SerializeField] private TMP_Text _sdg13Text;
    [SerializeField] private GameObject SDG17;
    [SerializeField] private TMP_Text _sdg17Text;

    [Header("Output")]
    [SerializeField] private TMP_Text _outputText;
    [SerializeField] private TMP_Text _outputScienceText;
    [SerializeField] private GameObject _outputScience;
    [SerializeField] private TMP_Text _outputProcessText;
    [SerializeField] private GameObject _outputProcess;
    [SerializeField] private TMP_Text _outputIdeaText;
    [SerializeField] private GameObject _outputIdea;

    public void setCard(CardModule card){
        _cardModule = card;
        _researchNameText.text = _cardModule.cardName;
        _researchDescriptionText.text = _cardModule.cardDescription;
        _sdgNumberText.text = _cardModule.sdgNumber;
        _sdgColorImage.color = _cardModule.sdgColor;
        _outputText.text = _cardModule.unlockText;

        if(_cardModule.weeklyScience > 0){
            Science.SetActive(true);
            _scienceGainText.text = "+" + _cardModule.weeklyScience;
        }
        else{
            Science.SetActive(false);
        }

        if (_cardModule.weeklySDG13 > 0)
        {
            SDG13.SetActive(true);
            _sdg13Text.text = "+" + _cardModule.weeklySDG13;
        } else {
            SDG13.SetActive(false);
        }

        if (_cardModule.weeklySDG17 > 0)
        {
            SDG17.SetActive(true);
            _sdg17Text.text = "+" + _cardModule.weeklySDG17;
        } else {
            SDG17.SetActive(false);
        }

        if (_cardModule._science != 0)
        {
            _outputScience.SetActive(true);
            _outputScienceText.text = _cardModule._science.ToString("0");
            
            if (_cardModule._science > 0) {
                _outputScienceText.color = Color.green;
            } else {
                _outputScienceText.color = Color.red;
            }
        }
        
        if(_cardModule._idea != 0)
        {
            _outputIdea.SetActive(true);
            _outputIdeaText.text = _cardModule._idea.ToString("0");
            if (_cardModule._idea > 0)
            {
                _outputIdeaText.color = Color.green;
            }
            else
            {
                _outputIdeaText.color = Color.red;
            }
        }

        if (_cardModule._processPower != 0)
        {
            _outputProcess.SetActive(true);
            _outputProcessText.text = _cardModule._processPower.ToString("0");

            if (_cardModule._processPower > 0) {
                _outputProcessText.color = Color.green;
            } else {
                _outputProcessText.color = Color.red;
            }
        }
    }

    public void Research()
    {
        if (_cardModule.id == 11)
            GameController.Instance.LastSDGFound();
        
        bool isNotEnough = false;
        if (_cardModule._science < 0)
        {
            isNotEnough = !GameController.Instance.CheckScienceEnough(_cardModule._science);
        }

        if (_cardModule._idea < 0)
        {
            isNotEnough = !GameController.Instance.CheckIdeaEnough(_cardModule._idea);
        }

        if (!isNotEnough) {
            if(_cardModule._science != 0)
                GameController.Instance.UpdateTotalScience(_cardModule._science);
            if(_cardModule._idea != 0)
                GameController.Instance.UpdateTotalIdea(_cardModule._idea);
            
            GameController.Instance.spawnAllocationCard(_cardModule._allocationCard);
            foreach (int _cardId in _cardModule._researchCardIds)
            {
                GameController.Instance.spawnResearchCard(_cardId);
            }

            GameController.Instance.removeResearchCard(_cardModule.id);
            
            if (_cardModule._processPower != 0)
                GameController.Instance.addProcessingPower(_cardModule._processPower);

            if(_cardModule._memory != 0)
                GameController.Instance.UpdateMemory(_cardModule._memory);
        }
    }

    public void SkipTutorial()
    {
        GameObject tutPage = GameObject.FindGameObjectWithTag("Tag");
        if (tutPage)
        {
            tutPage.GetComponent<TutorialPage>().NextPage();
        }
    }
}