using System;
using UnityEngine;

[CreateAssetMenu]
public class DialoguePage : ScriptableObject
{
    public String dialogueText;

    public String[] choices;

    public bool lastPage = false;
}
