using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPTracker : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public PlayerStats stats;

    [SerializeField] private float _chipSpeed = 1.75f;
    [SerializeField] private float endLimit = 0.002f;
    private float lerpTimer;

    void Update()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = (float)stats._hp / (float)stats._maxHP;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

            if (Mathf.Abs(fillB - hFraction) < endLimit)
                backHealthBar.fillAmount = hFraction;

            /*IF statement to fix the bug appearing due to the health bar neededing to lerp before the 
            gameover screen is displayed*/
            if (GameOverScreen.instance.isGameOver)
            {
                frontHealthBar.fillAmount = 0;
                backHealthBar.fillAmount = 0;
            }
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);

            if (Mathf.Abs(fillF - hFraction) < endLimit)
                frontHealthBar.fillAmount = hFraction;
        }

        lerpTimer /= 2;
    }

}
