using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour
{
    public Image hpFillImage;
    public float displayDuration = 2.0f; // Adjust the duration as needed
    private float currentDisplayTime;
    public GameObject bar;

    private void Start()
    {
        // Initially, hide the HP bar
        bar.gameObject.SetActive(false);
    }

    public void UpdateHPBar(int currentHealth, int maxHealth)
    {
        // Calculate the fill amount based on current health and max health
        float fillAmount = (float)currentHealth / maxHealth;
        hpFillImage.fillAmount = fillAmount;

        // Show the HP bar
        Debug.Log("Activating bar GameObject");
        bar.gameObject.SetActive(true);

        // Reset the display timer
        currentDisplayTime = 0.0f;

        // Start a coroutine to hide the HP bar after a certain duration
        StartCoroutine(HideHPBar());
    }

    private IEnumerator HideHPBar()
    {
        while (currentDisplayTime < displayDuration)
        {
            currentDisplayTime += Time.deltaTime;
            yield return null;
        }
        
        // Hide the HP bar when the duration is over
        bar.gameObject.SetActive(false);
    }
}
