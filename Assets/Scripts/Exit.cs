using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
