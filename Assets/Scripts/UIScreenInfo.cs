using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class UIScreenInfo
{
    public UIManager.ScreenTypes scrType;
    public Ease animType;
    public GameObject screenRef;
    public float animDuration;
}