using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPTracker : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public SpaceshipStats stats;

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
            print(lerpTimer);
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
