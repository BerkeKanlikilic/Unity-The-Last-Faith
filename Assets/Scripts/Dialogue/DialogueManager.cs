using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Serializable]
    public class DialoguePage {
        public String dialogueText;

        public String[] choices;

        public bool lastPage = false;

        public bool fadeBackground = false;
    }

    [Header("Pages")]
    [SerializeField] private List<DialoguePage> _dialoguePages;

    [Header("Parameters")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] public string playerName;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject nameEntry;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject _popupBG;

    [Header("Tutorial")]
    [SerializeField] private GameObject firstPage;

    private int currentPage = 0;

    private bool dialogueStarted = false;
    private bool dialogueReading = false;

    private string nameText;    
    
    private void Start() {
        dialoguePanel.SetActive(false);
        _popupBG.SetActive(false);
        StartDialogue();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && dialogueReading)
        {
            dialogueReading = false;
        }
    }

    public void StartDialogue() {
        // Check if dialogue already started, or for out of bounds exception.
        if (dialogueStarted || currentPage >= _dialoguePages.Count) return;

        ResetChoices();
        
        SetPage();
        _popupBG.SetActive(true);
        dialoguePanel.SetActive(true);
        dialogueStarted = true;
    }

    private void SetPage() {
        StartCoroutine(DisplayLine(_dialoguePages[currentPage].dialogueText));
        StartCoroutine(SetChoices());
    }

    private void CloseDialogue() {
        firstPage.SetActive(true);
        dialoguePanel.SetActive(false);
        dialogueStarted = false;
    }

    private void ResetChoices() {
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
        nameEntry.SetActive(false);
    }

    private IEnumerator SetChoices() {
        while (dialogueReading)
        {
            yield return null;
        }
        
        int index = _dialoguePages[currentPage].choices.Length;

        for (int i = 0; i < index ; i++)
        {
            choices[i].SetActive(true);
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = "[ " + _dialoguePages[currentPage].choices[i] + " ]";
        }
        
        if(currentPage == 2)
            nameEntry.SetActive(true);

        currentPage++;
    }

    public void NextPage() {
        if (dialogueReading) return;
        
        // Check for out of bounds exception or the page before was a last page of a dialogue.
        if (currentPage >= _dialoguePages.Count || _dialoguePages[currentPage - 1].lastPage)
        {
            CloseDialogue();
            return;
        }
        
        if (_dialoguePages[currentPage].fadeBackground) {
            StartCoroutine(FadeBackground(true));
        }

        ResetChoices();
        SetPage();
    }

    private IEnumerator DisplayLine(String line) {
        dialogueReading = true;
        dialogueText.text = "";
        
        foreach (char letter in currentPage == 3 ? nameText.ToCharArray() : line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            if (!dialogueReading)
            {
                dialogueText.text = currentPage == 4 ? nameText : _dialoguePages[currentPage - 1].dialogueText;
                break;
            }
        }

        if (dialogueReading) dialogueReading = false;
    }

    public void GetName() {
        if (nameInput.text == "") return;
        playerName = nameInput.text;
        nameText = "Well met " + playerName + ". That is an interesting name. " + playerName + "_v3_Final(2).exe will be tested to accomplish the missing SDG's. " +
                   "If your program runs adequately, you will be assigned to protect other cities too.";
        NextPage();
    }

    IEnumerator FadeBackground(bool fadeAway) {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                _popupBG.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            _popupBG.SetActive(false);
        }
        else
        {
            _popupBG.GetComponent<CanvasGroup>().alpha = 0;
            _popupBG.SetActive(true);
            for (float i = 0; i <= 0; i += Time.deltaTime)
            {
                _popupBG.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
        }
    }
}