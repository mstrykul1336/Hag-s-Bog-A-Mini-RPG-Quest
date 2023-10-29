using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Photon.Pun;
//using Photon.Realtime;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI dialogue_halsin_Text;

    // instance
    public static GameUI instance;
    public static int quest_number;
    public bool player_option1;
    public static bool halsinquest = false;
    public GameObject halsin_dialogue;
    public GameObject shop_UI;
    void Awake()
    {
        instance = this;
    }


    public void Halsin_Text_Change()
    {
        halsin_dialogue.SetActive(true);
        dialogue_halsin_Text.text = "Goblins are attempting to overtake the Grove again! Will you help us?";
    }
   
    public void Option1_Yes()
    {
        dialogue_halsin_Text.text = "Thank you! Hurry quickly, before more goblins come!";
        StartCoroutine(Timer());


    }
    public void UpdateGoldText(int gold)
    {
        goldText.text = "<b>Gold:</b> " + gold;
    }

    public void UpdateQuestText(int quest_number)
    {
        if(quest_number == 1)
        {
            questText.text = "Defeat the Goblins!";
        }
        else if(quest_number == 2)
        {
            questText.text = "Defeat the Hag!";
        }
        else if(quest_number == 3)
        {
            questText.text = "Victory! Collect Loot!";
        }
    }

    public void Update()
    {
        if(halsinquest == true)
        {
            Halsin_Text_Change();
            UpdateQuestText(1);
            halsinquest = false;
        }
        if (Enemy.goblinDeathCount == 10)
        {
            UpdateQuestText(2);
        }
        else if(Hag.hagDeathCount == 4)
        {
            UpdateQuestText(3);
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        halsin_dialogue.SetActive(false);
    }
}