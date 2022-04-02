using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextColor : MonoBehaviour
{
    [Header("Text Variables")]
    public TMP_Text text;

    public void OnSelect()
    {
        text.color = Color.white;
    }

    public void OnDeselect()
    {
        text.color = Color.black;
    }
}
