using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int currentShieldAmount;
    [SerializeField] private int maxShieldAmount;
    [SerializeField] private GameObject[] shields;
    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private float duration;

    [SerializeField] private float iFramesLengthInSec;
    private float time;

    private int currentExperience;
    private int maxExperience = 100;
    private int level;
    

    void Start()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i < currentShieldAmount)
            {
                shields[i].SetActive(true);
            }
        }
    }


    private void Update()
    {
        time -= Time.deltaTime;
    }

    public void TakeHit()
    {
        if (time <= 0.0f)
        {

            if (currentShieldAmount > 0)
            {
                for (int i = maxShieldAmount - 1; i != (currentShieldAmount - 2); --i)
                {
                    shields[i].SetActive(false);
                }
            }

            time = iFramesLengthInSec;
            StartCoroutine(FlashRoutine());
            StartCoroutine(IframeFlash());
            if (currentShieldAmount > 0)
            {
                currentShieldAmount -= 1;
            }
            else
            {
                print("die");
            }
        }
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = defaultMaterial;
    }
    private IEnumerator IframeFlash()
    {
        yield return new WaitForSeconds(duration);
        while (time > 0.0f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.02f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleExperienceChange(int NewExperience)
    {
        currentExperience += NewExperience;
        if (currentExperience >= maxExperience)
        {
            level++;
        }
    }
}
