using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPage : MonoBehaviour
{
    [SerializeField] private bool eventTriggered;
    [SerializeField] private bool processFull;
    [SerializeField] private GameObject nextPage;
    

    private void OnEnable()
    {
        if (eventTriggered)
        {
            StartCoroutine(CheckProcessPower());
        }
    }

    private IEnumerator CheckProcessPower()
    {
        while (true)
        {
            if (processFull)
            {
                if (GameController.Instance.totalProcessPower > 5)
                {
                    nextPage.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            
            if (GameController.Instance.totalProcessPower < 1)
            {
                nextPage.SetActive(true);
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    public void NextPage(GameObject nextPage) {
        if (gameObject.activeSelf)
        {
            nextPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    
    public void NextPage() {
        if (gameObject.activeSelf && nextPage != null)
        {
            nextPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ClosePage()
    {
        gameObject.SetActive(false);
    }
}
