using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CardModule : ScriptableObject
{
    public int id;
    public string sdgNumber;
    public Color sdgColor;
    public string cardName;
    public string cardDescription;
    public string unlockText;

    [Header("Research Unlocks")]
    //Resource
    public int _processPower;
    public float _science;
    public float _idea;
    public float _memory;
    
    [Header("Allocation Card")]
    public int _allocationCard;

    [Header("Research Cards")]
    public int[] _researchCardIds;

    [Header("Weekly Gain")]
    public float weeklyScience;
    public float weeklySDG13;
    public float weeklySDG17;
    public float weeklyIdea;

    [Header("Goal")]
    public string _goalName;
    public int _startingGoal;
    public int _startingGoalModifier;

    [Header("Goal Reward")]
    public int _goalProcess;
    public int _goalScience;
    public int _goalIdea;

    [Header("Bar Effects")]
    public float _aifEffect;
    public float _soPositiveEffect;
    public float _soNegativeEffect;
    public float _poPositiveEffect;
    public float _poNegativeEffect;
}
