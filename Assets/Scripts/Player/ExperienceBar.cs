using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float minimum;
    [SerializeField] private float maximum;
    [SerializeField] private Image mask;
    private float current;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
    }

    private void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
    }
    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleExperienceChange(float NewExperience)
    {
        current += NewExperience * player.GetComponent<Player>().expMultiplier;
        if (current >= maximum)
        {
            minimum = maximum;
            float tmpInt = (maximum / 2) + maximum;
            maximum += tmpInt;
        }
        GetCurrentFill();
    }
}
