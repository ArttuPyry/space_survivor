using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerPerkManager : MonoBehaviour
{

    [SerializeField] private List<Perk> perkList;
    public List<Perk> acquiredPerks;

    [SerializeField] private GameObject LevelUpMenu;

    [SerializeField] private GameObject perkChoiceOne;
    [SerializeField] private GameObject perkChoiceTwo;
    [SerializeField] private GameObject perkChoiceThree;

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

        // Setup perk names
        perkChoiceOne.transform.Find("PerkName").GetComponent<TMP_Text>().text = perkList[0].perkName.ToString();
        perkChoiceTwo.transform.Find("PerkName").GetComponent<TMP_Text>().text = perkList[1].perkName.ToString();
        perkChoiceThree.transform.Find("PerkName").GetComponent<TMP_Text>().text = perkList[2].perkName.ToString();

        // Setup perk sprites

        // Setup perk Descriptions
        perkChoiceOne.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = perkList[0].perkDescription.ToString();
        perkChoiceTwo.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = perkList[1].perkDescription.ToString();
        perkChoiceThree.transform.Find("PerkDescription").GetComponent<TMP_Text>().text = perkList[2].perkDescription.ToString();


        LevelUpMenu.SetActive(true);
    }

    public void CloseLevelUpMenu(int buttonIndex)
    {
        perkList[buttonIndex].Apply(this.gameObject);
        LevelUpMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
