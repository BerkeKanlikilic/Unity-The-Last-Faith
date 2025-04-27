using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchController : MonoBehaviour
{
    public static ResearchController Instance { get; private set; }

    [SerializeField] private CardModule[] _cardModules;
    [SerializeField] private GameObject _researchCardPrefab;
    [SerializeField] private Transform _researchCardParent;

    [SerializeField] private CardModule[] _activeCardModules;

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
        // spawnResearchCard(0);
    }

    public void spawnResearchCard(int id){
        foreach(CardModule card in _cardModules){
            if(card.id == id){
                GameObject newCard = Instantiate(_researchCardPrefab, transform);
                newCard.transform.SetParent(_researchCardParent);
                newCard.GetComponent<ResearchCard>().setCard(card);
            }
        }
    }
}