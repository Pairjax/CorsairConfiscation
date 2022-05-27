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
    private float lerpTimer;

    void Update()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = (float)stats.hp / (float)stats.maxhp;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

            /*IF statement to fix the bug appearing due to the health bar neededing to lerp before the 
            gameover screen is displayed*/
            if (GameOverScreen.instance.isGameOver)
            {
                frontHealthBar.fillAmount = 0;
                backHealthBar.fillAmount = 0;
            }
        }
        /*else if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }*/

        lerpTimer /= 2;
    }

}
