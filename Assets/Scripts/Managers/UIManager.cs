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

    public void FadeFromBlack()
    {
        fadeFromBlack = true;
    }
}
