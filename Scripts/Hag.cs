using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hag : MonoBehaviourPun
{
   [Header("Info")]
    public string hagName;
    public float moveSpeed;

    public int curHp;
    public int maxHp;

    public float chaseRange;
    public float attackRange;
    public string hagclonePrefabPath;
    public float spawnRadius;

    private PlayerController targetPlayer;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    public string objectToSpawnOnDeath;
    public static int hagDeathCount;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public int abilityUsed = 3;

    [Header("Components")]
    public HeaderInfo healthBar;
    public SpriteRenderer sr;
    public Rigidbody2D rig;


    void Start()
    {
        healthBar.Initialize(hagName, maxHp);
    }

    void Update()
    {
         if (!PhotonNetwork.IsMasterClient)
            return;

        if (targetPlayer != null)
        {
            // calculate the distance
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
            Debug.Log("dist to player = " + dist);

            // if we're able to attack, do so
            if (dist < attackRange && Time.time - lastAttackTime >= attackRange)
                Attack();
            // otherwise, do we move after the player?
            else if (dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }
            else
            {
                rig.velocity = Vector2.zero;
            }
        }

        DetectPlayer();
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        targetPlayer.photonView.RPC("TakeDamage", targetPlayer.photonPlayer, damage);
    }

    void DetectPlayer()
    {
        if(Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            // loop through all the players
            foreach (PlayerController player in GameManager.instance.players)
            {
                // calculate distance between us and the player
                float dist = Vector2.Distance(transform.position, player.transform.position);

                if (player == targetPlayer)
                {
                    if (dist > chaseRange)
                        targetPlayer = null;
                }
                else if (dist < chaseRange)
                {
                    if (targetPlayer == null)
                        targetPlayer = player;
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        
        if(abilityUsed != 0){
            SpecialAbility();
        }
        // update the health bar
        healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);

        if (curHp <= 0)
            Die();
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }
    }

    //[PunRPC]
    public void SpecialAbility()
    {
        Vector3 randomInCircle = Random.insideUnitCircle * spawnRadius;
        GameObject hagclone = PhotonNetwork.Instantiate(hagclonePrefabPath, transform.position + randomInCircle, Quaternion.identity);
        abilityUsed -= 1;
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
        hagDeathCount += 1;
    }
}
