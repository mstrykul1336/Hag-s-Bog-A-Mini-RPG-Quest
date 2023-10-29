using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public int gold;
    public int curHp;
    public int maxHp;
    public bool dead;
    public static string character_name;

    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    public Rigidbody2D rig;
    public Player photonPlayer;
    public SpriteRenderer sr;
    public Animator weaponAnim;
    public HeaderInfo headerInfo;

    [Header("Sprite")]
    public Sprite skin1;
    public Sprite skin2;
    public Sprite skin3;
    public Sprite skin4;
    public GameObject eye1;
    public GameObject eye2;
    public GameObject eye3;
    public GameObject hair1;
    public GameObject hair2;
    public GameObject hair3;
    public GameObject hair4;
    public GameObject hair5;
    public GameObject hair6;
    public GameObject hair7;
    public Sprite default1;
    public static int skin_number;
    public static int eye_number;
    public static int hair_number;
    // local player
    public static PlayerController me;
    public GameObject shopUI;
    public GameObject purchased;
    public int shopItem;


    // Start is called before the first frame update
    void Start()
    {
        SetSkinTone(skin_number);
        SetEyeColor(eye_number);
        SetHairColor(hair_number);

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        Move();

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate)
            Attack();

        if(Input.GetMouseButtonDown(1))
        {
            TryToChat();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I inpuit");
            ShopUIOn();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            shopUI.SetActive(false);
        }

        float mouseX = (Screen.width / 2) - Input.mousePosition.x;

        if (mouseX < 0)
            weaponAnim.transform.parent.localScale = new Vector3(1, 1, 1);
        else
            weaponAnim.transform.parent.localScale = new Vector3(-1, 1, 1);
    }

    void Move()
    {
        // get the horizontal and vertical inputs
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // apply that to our velocity
        rig.velocity = new Vector2(x, y) * moveSpeed;
    }

    public void TryToChat()
    {
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);
         if (hit.collider != null && hit.collider.gameObject.CompareTag("NPC"))
        {
            // get the enemy and damage them
            //NPC NPCchar = hit.collider.GetComponent<NPC>();
            GameUI.halsinquest = true;
        }
    }

    public void ShopPurchases(int shopItem)
    {
        if(gold >= 50 && shopItem == 1)
        {
            gold -= 50;
            CollectArmor(2);
            Debug.Log("Purchased1");
        }

        else if (gold >= 100 && shopItem == 2)
        {
            gold -= 100;
            CollectArmor(4);
        }
    }

    public void ShopUIOn()
    {
        Debug.Log("UI active");
        shopUI.SetActive(true);
    }

    public void Item1()
    {
        shopItem = 1;
        purchased.SetActive(true);
        StartCoroutine(ShopTimer());
        purchased.SetActive(false);
        ShopPurchases(shopItem);
    }

    public void Item2()
    {
        shopItem = 2;
        purchased.SetActive(true);
        StartCoroutine(ShopTimer());
        purchased.SetActive(false);
        ShopPurchases(shopItem);
    }

    IEnumerator ShopTimer()
    {    
        yield return new WaitForSeconds(3);
    }

    // melee attacks towards the mouse
    void Attack()
    {
        lastAttackTime = Time.time;

        // calculate the direction
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

        // shoot a raycast in the direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);

        // did we hit an enemy?
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            // get the enemy and damage them
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
        }

        else if (hit.collider != null && hit.collider.gameObject.CompareTag("Hag"))
        {
            Hag hag = hit.collider.GetComponent<Hag>();
            hag.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
        }
        else if (hit.collider != null && hit.collider.gameObject.CompareTag("NPC"))
        {
            NPC NPCchar = hit.collider.GetComponent<NPC>();
            NPCchar.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
        }

        // play attack animation
        weaponAnim.SetTrigger("Attack");
    }

    //[PunRPC]
    public void SetEyeColor(int eye_number)
    {
        Debug.Log("Setting eye color!");
        if(eye_number == 1)
        {
            eye1.SetActive(true);
        }
        else if(eye_number == 2)
        {
            eye2.SetActive(true);
        }
        else if(eye_number == 3)
        {
            eye3.SetActive(true);
        }
        else if(eye_number == 0)
        {
            Debug.Log("Default set.");
        }
        
    }

    public void SetSkinTone(int skin_number)
    {
        if(skin_number == 1)
        {
            sr.sprite = skin1;
        }
        else if(skin_number == 2)
        {
            sr.sprite = skin2;
        }
        else if(skin_number == 3)
        {
            sr.sprite = skin3;
        }
        else if(skin_number == 4)
        {
            sr.sprite = skin4;
        }
        else if(skin_number == 0)
        {
            sr.sprite = default1;
        }
    }

    public void SetHairColor(int hair_number)
    {
        Debug.Log("Setting hair color!");
        if(hair_number == 1)
        {
            hair1.SetActive(true);
        }
        else if(hair_number == 2)
        {
            hair2.SetActive(true);
        }
        else if(hair_number == 3)
        {
            hair3.SetActive(true);
        }
        else if(hair_number == 4)
        {
            hair4.SetActive(true);
        }
        else if(hair_number == 5)
        {
            hair5.SetActive(true);
        }
        else if(hair_number == 6)
        {
            hair6.SetActive(true);
        }
        else if(hair_number == 7)
        {
            hair7.SetActive(true);
        }
        else if(hair_number == 0)
        {
            Debug.Log("Default set.");
        }
        
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;

        // update the health bar
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);

        if (curHp <= 0)
            Die();
        else
        {
            StartCoroutine(DamageFlash());
            IEnumerator DamageFlash()
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.05f);
                sr.color = Color.white;
            }
        }
    }

    void Die()
    {
        dead = true;
        rig.isKinematic = true;

        transform.position = new Vector3(0, 99, 0);

        Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;

        StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    IEnumerator Spawn(Vector3 spawnPos, float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);

        dead = false;
        transform.position = spawnPos;
        curHp = maxHp;
        rig.isKinematic = false;

        // update the health bar
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
    }

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;

        // initialize the health bar
        headerInfo.Initialize(player.NickName, maxHp);

        if (player.IsLocal)
            me = this;
        else
            rig.isKinematic = true; // turn off physics on other players so we don't process their collisions
    }

    [PunRPC]
    void Heal(int amountToHeal)
    {
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);

        // update the health bar
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
    }

    [PunRPC]
    void GiveGold(int goldToGive)
    {
        gold += goldToGive;

        // update the ui
        GameUI.instance.UpdateGoldText(gold);
    }

    [PunRPC]
    void CollectArmor(int armor_type)
    {
        // helmet of strength
        if (armor_type == 1)
        {
            damage += 5;
        }
        //boots of speed
        else if (armor_type == 2)
        {
            moveSpeed += 2;
        }
        //chestplate of health
        else if (armor_type == 3)
        {
            maxHp += 40;
        }
        //cape of attack range and speed
        else if (armor_type == 4)
        {
            attackRange += 2;
            attackRate += 1;
        }
    }


    
}
