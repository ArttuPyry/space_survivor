using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Shield
    public int currentShieldAmount;
    [SerializeField] private int maxShieldAmount;
    [SerializeField] private GameObject[] shields;

    // Flash effect and iframes
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private float duration;
    public float iFramesLengthInSec = 1f;
    private float iFrameTime;

    // Experience
    public float currentExperience;
    private float maxExperience = 100;
    private int level = 1;
    public float expMultiplier = 1;

    // Luck
    public int luck;

    // Perks
    public List<Perk> acquiredHitPerks;
    public List<Perk> acquiredTimedPerks;

    private float perkTime;
    public float perkTimeCooldown = 10.0f;

    void Start()
    {
        UpdateShield();
    }


    private void Update()
    {
        iFrameTime -= Time.deltaTime;
        perkTime -= Time.deltaTime;

        if (perkTime <= 0.0f)
        {
            perkTime = perkTimeCooldown;
            ActivateTimedPerks();
        }

    }

    private void ActivateTimedPerks()
    {
        if (acquiredTimedPerks.Count > 0)
        {
            for (int i = 0; i < acquiredTimedPerks.Count; i++)
            {
                acquiredTimedPerks[i].Apply(this.gameObject);
            }
        }
    }

    public void UpdateShield()
    {
        if (currentShieldAmount > maxShieldAmount)
        {
            currentShieldAmount = maxShieldAmount;
        }

        for (int i = 0; i < shields.Length; i++)
        {
            if (i < currentShieldAmount)
            {
                shields[i].SetActive(true);
            }
        }
    }

    public void TakeHit()
    {
        if (iFrameTime <= 0.0f)
        {

            // Activates hit perks
            if (acquiredHitPerks.Count > 0)
            {
                for (int i = 0; i < acquiredHitPerks.Count; i++)
                {
                    acquiredHitPerks[i].Apply(this.gameObject);
                }
            } 

            // Disables shield icons
            if (currentShieldAmount > 0)
            {
                for (int i = maxShieldAmount - 1; i != (currentShieldAmount - 2); --i)
                {
                    shields[i].SetActive(false);
                }
            }

            // iFrames
            iFrameTime = iFramesLengthInSec;
            StartCoroutine(FlashRoutine());
            StartCoroutine(IframeFlash());

            // Removes shield or kills 
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

    // iFrames and flash effect
    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = defaultMaterial;
    }
    private IEnumerator IframeFlash()
    {
        yield return new WaitForSeconds(duration);
        while (iFrameTime > 0.0f)
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

    private void HandleExperienceChange(float NewExperience)
    {
        currentExperience += NewExperience * expMultiplier;
        if (currentExperience >= maxExperience)
        {
            level++;
            float tmpInt = (maxExperience / 2) + maxExperience;
            maxExperience += tmpInt;
            this.GetComponent<PlayerPerkManager>().OpenLevelUpMenu();
        }
    }
}
