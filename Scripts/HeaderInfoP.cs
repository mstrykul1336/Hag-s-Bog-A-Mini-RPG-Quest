using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeaderInfoP : MonoBehaviour
{
   
    public TextMeshProUGUI characterText;

    void Start(){
        characterText.text = PlayerController.character_name;
       
    }
    

}