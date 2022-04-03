using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Fade Screen Variables")]
    public Image fadeScreen;
    public float fadeSpeed;
    [HideInInspector] public bool fadeFromBlack;

    [Header("Gotcha Counter")]
    public TMP_Text gotchaCounter;
    [HideInInspector] public int gotchas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        FadeFromBlack();
        UpdateGotchaCounterUI(0);
    }

    void Update()
    {
        //Make the fade screen fade from its current value to 0 aka making it transparent
        if (fadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
            Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    public void UpdateGotchaCounterUI(int gotchasToAdd)
    {
        gotchas += gotchasToAdd;
        gotchaCounter.text = "Gotchas: " + gotchas.ToString("F0");
    }

    public void FadeFromBlack()
    {
        fadeFromBlack = true;
    }
}
