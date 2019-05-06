using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAIScript : MonoBehaviour
{
    public enum RabbitType
    {
       NORMAL_RABBIT,
       CARROT_RABBIT,
       BIG_RABBIT,
    }

    //! stats
    public int health;
    public float speed;
    public int damage;
    public float damageInterval;
    public float redTintInterval;

    //! AI
    private Vector3 direction;
    private float gravitySpeed = 6.0f;
    private float jumpSpeed = 12.0f;
    private float jumpingChance = 5;
    public RaycastHit2D leftAttack, RightAttack, UpAttack;

    //! Boolean checks & prev frame checkers
    private bool isJump;
    private bool isAttack;
    public bool isHit;
    private bool isDeath;
    private string prevHitUp; //! used for jump purposes
    private Vector3 lastPos;

    //! Components
    private GameObject player;
    public GameObject carrot;
    public RabbitType rabbitType;
    public BoxCollider2D atttackCollider;
    private Animator anim;

    private float intervalTime;

    public int scoreAmount;

    public GameObject spawnedPlatform; //! used to keep track which platform the carrot rabbit spawned on

	void Start ()
    {
        if (Random.Range(0, 2) == 0)
            direction = Vector3.left;
        else
            direction = Vector3.right;

        isJump = false;
        isHit = false;
        isDeath = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = transform.GetChild(0).GetComponent<Animator>();
	}

    void Update()
    {
        if (!isDeath && Time.timeScale != 0)
        {
            HealthCheck();
            AIChecking();
            ArtificialGravity();

            intervalTime += Time.deltaTime;
            redTintInterval += Time.deltaTime;
        }
	}

    void AIChecking()
    {
        Vector2 raycastStartPos = new Vector2(transform.position.x, transform.position.y + 0.2f);
        leftAttack = Physics2D.Raycast(raycastStartPos, Vector2.left, 17.0f, LayerMask.GetMask("Player"));
        RightAttack = Physics2D.Raycast(raycastStartPos, Vector2.right, 17.0f, LayerMask.GetMask("Player"));
        UpAttack = Physics2D.Raycast(raycastStartPos, Vector2.up, 5.0f, LayerMask.GetMask("Player"));

        if (leftAttack || RightAttack || UpAttack)
        {
            AttackAI();
            if (rabbitType == RabbitType.BIG_RABBIT)
                MovementAI();
        }
        else
        {
            if (rabbitType != RabbitType.CARROT_RABBIT)
            {
                MovementAI();
                isAttack = false;
                atttackCollider.enabled = false;
            }
            else
            {
                AttackAI();
            }

        }
    }

    void MovementAI()
    {
        if (transform.position.x <= -8.5f)
            direction = Vector3.right;
        else if (transform.position.x >= 8.5f)
            direction = Vector3.left;

        if (rabbitType != RabbitType.CARROT_RABBIT)
        {
            if (lastPos.x < transform.position.x)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        lastPos = transform.position;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void AttackAI()
    {
        isAttack = true;
        if(rabbitType == RabbitType.NORMAL_RABBIT)
        {
            //! Chasing Player
            if (leftAttack)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                transform.position = Vector3.MoveTowards(transform.position, PlayerControllerScript.instance.transform.position 
                    + new Vector3(0.5f, 0.0f), speed * Time.deltaTime);
                atttackCollider.enabled = true;
            }
            else if(RightAttack)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, PlayerControllerScript.instance.transform.position
                    + new Vector3(-0.5f, 0.0f), speed * Time.deltaTime);
                atttackCollider.enabled = true;
            }
            else if (UpAttack)
            {
                isJump = true;
            }
        }

        if(rabbitType == RabbitType.CARROT_RABBIT && intervalTime >= damageInterval)
        {
            anim.Play("Idle No Carrot");
            if (leftAttack)
            {
                carrot.GetComponent<CarrotScript>().InitSpawn(Vector3.left);
                carrot.GetComponent<SpriteRenderer>().flipX = false;
                Instantiate(carrot, transform.position + new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(RightAttack)
            {
                carrot.GetComponent<CarrotScript>().InitSpawn(Vector3.right);
                carrot.GetComponent<SpriteRenderer>().flipX = true;
                Instantiate(carrot, transform.position + new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                int random = Random.Range(0, 2);
                if(random == 1)
                {
                    carrot.GetComponent<CarrotScript>().InitSpawn(Vector3.right);
                    carrot.GetComponent<SpriteRenderer>().flipX = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    carrot.GetComponent<CarrotScript>().InitSpawn(Vector3.left);
                    carrot.GetComponent<SpriteRenderer>().flipX = false;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                }

                Instantiate(carrot, transform.position + new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity);
            }

            intervalTime = 0.0f;
        }

        if (rabbitType != RabbitType.CARROT_RABBIT)
        {
            //! Dealing Attacks
            if (intervalTime >= damageInterval && isHit)
            {
                PlayerControllerScript.instance.currentHealth -= damage;
                intervalTime = 0.0f;

                if (rabbitType == RabbitType.NORMAL_RABBIT)
                    anim.Play("Attack");
            }
        }
            
    }

    void ArtificialGravity()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 9.5f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 5.0f, LayerMask.GetMask("Ground"));

        if(rabbitType == RabbitType.NORMAL_RABBIT)
        {
            if (hitDown.distance > 0.0f && !isJump)
            {
                transform.Translate(Vector2.down * gravitySpeed * Time.deltaTime);
                if(!isAttack)
                    anim.Play("Landing");
            }
            else if (hitDown.distance <= 0.0f && !isJump && !isAttack)
            {
                anim.Play("Move");
            }

            if (hitUp.collider != null && !isAttack)
            {
                int randomChance = Random.Range(0, 100);
                if (randomChance <= jumpingChance)
                {
                    anim.Play("Jump");
                    isJump = true;
                    prevHitUp = hitUp.collider.name;
                }
            }

            if (isJump)
            {
                if (hitUp == false || prevHitUp == hitDown.collider.name)
                {
                    anim.Play("Move");
                    isJump = false;
                    prevHitUp = null;
                }
                transform.Translate(Vector2.up * jumpSpeed * Time.deltaTime);
            }
        }
       
    }

    void HealthCheck()
    {
        if(redTintInterval > 1.5f)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void TakeDamage(int damageReceived)
    {
        health -= damageReceived;
        redTintInterval = 0.0f;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;

        if (health <= 0 && !isDeath)
        {
            switch (rabbitType)
            {
                case RabbitType.NORMAL_RABBIT:
                    RabbitSpawnScript.instance.spawedNumberList[0]--;
                    break;
                case RabbitType.CARROT_RABBIT:
                    RabbitSpawnScript.instance.spawedNumberList[1]--;
                    break;
                case RabbitType.BIG_RABBIT:
                    RabbitSpawnScript.instance.spawedNumberList[2]--;
                    break;
                default:
                    break;
            }

            ScoreScript.instance.IncreaseScore(scoreAmount);
            isDeath = true;
            StartCoroutine(DeathAnimation());
        }
    }

    IEnumerator DeathAnimation()
    {
        anim.Play("Death");
        yield return new WaitForSeconds(0.5f);
        if(rabbitType == RabbitType.CARROT_RABBIT)
            RabbitSpawnScript.instance.platformListCopy.Add(spawnedPlatform);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rabbitType == RabbitType.NORMAL_RABBIT && collision.tag == "Player")
            anim.Play("Attack");
            
    }
}
