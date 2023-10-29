using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviourPun
{
    [Header("Info")]
    public string npcName;
    public float moveSpeed;

    public int curHp;
    public int maxHp;
    public string objectToSpawnOnDeath;

    [Header("Components")]
    public HeaderInfo healthBar;
    public SpriteRenderer sr;
    public Rigidbody2D rig;


    void Start()
    {
        healthBar.Initialize(npcName, maxHp);
    }

   // [PunRPC]
    //public void Dialogue()
  //  {
       // GameUI.halsin_dialogue.SetActive(true);
   // }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;

        // update the health bar
        healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);

        if (curHp <= 0)
            Die();
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    void FlashDamage()
    {
        StartCoroutine(DamageFlash());

        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }

    void Die()
    {
        if (objectToSpawnOnDeath != string.Empty)
            PhotonNetwork.Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);

        // destroy the object across the network
        PhotonNetwork.Destroy(gameObject);
    }
}
