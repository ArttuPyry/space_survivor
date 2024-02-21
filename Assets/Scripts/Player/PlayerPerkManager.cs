using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerPerkManager : MonoBehaviour
{

    [SerializeField] private List<Perk> CommonPerkList;
    [SerializeField] private List<Perk> RarePerkList;
    [SerializeField] private List<Perk> LegendaryPerkList;
    public List<Perk> acquiredPerks;

    [SerializeField] private GameObject LevelUpMenu;

    [SerializeField] private GameObject perkChoiceOne;
    [SerializeField] private GameObject perkChoiceTwo;
    [SerializeField] private GameObject perkChoiceThree;

    [SerializeField] private int rare = 11;
    [SerializeField] private int legendary = 1;

    private void Awake()
    {
        LevelUpMenu = GameObject.Find("/Ui/LevelUpMenu");
        perkChoiceOne = GameObject.Find("/Ui/LevelUpMenu/Perk1");
        perkChoiceTwo = GameObject.Find("/Ui/LevelUpMenu/Perk2");
        perkChoiceThree = GameObject.Find("/Ui/LevelUpMenu/Perk3");
        LevelUpMenu.SetActive(false);
    }

    public void OpenLevelUpMenu()
    {
        Time.timeScale = 0f;

        RollPerks(perkChoiceOne);
        RollPerks(perkChoiceTwo);
        RollPerks(perkChoiceThree);

        LevelUpMenu.SetActive(true);
    }

    public void CloseLevelUpMenu(int buttonIndex, int rarity)
    {
        switch (rarity)
        {
            case 0:
                ApplyPerk(CommonPerkList[buttonIndex]);
                break;
            case 1:
                ApplyPerk(RarePerkList[buttonIndex]);
                break;
            case 2:
                ApplyPerk(LegendaryPerkList[buttonIndex]);
                break;
        }

        LevelUpMenu.SetActive(false);
        Time.timeScale = 1f;

        perkChoiceOne.GetComponent<Button>().onClick.RemoveAllListeners();
        perkChoiceTwo.GetComponent<Button>().onClick.RemoveAllListeners();
        perkChoiceThree.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    // Checks perk type and applies it to correct perk list
    private void ApplyPerk(Perk perk)
    {
        switch (perk.perkType)
        {
            case Perk.PerkType.ApplyOnce:
                perk.Apply(this.gameObject);
                break;
            case Perk.PerkType.Crit:
                this.GetComponent<PlayerBasicAttack>().acquiredCritPerks.Add(perk);
                break;
            case Perk.PerkType.DamageTaken:
                this.GetComponent<Player>().acquiredHitPerks.Add(perk);
                break;
            case Perk.PerkType.Timed:
                this.GetComponent<Player>().acquiredTimedPerks.Add(perk);
                break;
        }
    }


    private void RollPerks(GameObject perkChoice)
    {
        int randomIndex = Random.Range(1, 100);

        if (randomIndex >= 1 && randomIndex <= legendary)
        {
            int randomPerk = Random.Range(0, LegendaryPerkList.Count);
            perkChoice.transform.Find("PerkName").GetComponent<TMP_Text>().text = LegendaryPerkList[randomPerk].perkName.ToString();
            perkChoice.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = LegendaryPerkList[randomPerk].perkDescription.ToString();
            perkChoice.GetComponent<Button>().onClick.AddListener(()=>CloseLevelUpMenu(randomPerk, 2));
        }

        if (randomIndex > legendary && randomIndex <= rare)
        {
            int randomPerk = Random.Range(0, RarePerkList.Count);
            perkChoice.transform.Find("PerkName").GetComponent<TMP_Text>().text = RarePerkList[randomPerk].perkName.ToString();
            perkChoice.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = RarePerkList[randomPerk].perkDescription.ToString();
            perkChoice.GetComponent<Button>().onClick.AddListener(() => CloseLevelUpMenu(randomPerk, 1));
        }

        if (randomIndex > rare && randomIndex <= 100)
        {
            int randomPerk = Random.Range(0, CommonPerkList.Count);
            perkChoice.transform.Find("PerkName").GetComponent<TMP_Text>().text = CommonPerkList[randomPerk].perkName.ToString();
            perkChoice.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = CommonPerkList[randomPerk].perkDescription.ToString();
            perkChoice.GetComponent<Button>().onClick.AddListener(() => CloseLevelUpMenu(randomPerk, 0));
        }
    }


}
