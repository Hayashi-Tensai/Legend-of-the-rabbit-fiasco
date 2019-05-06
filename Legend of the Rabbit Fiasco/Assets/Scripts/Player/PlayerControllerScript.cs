using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    //! Player Stats
    public int maxHealth = 100;
    public int currentHealth;
    private Vector2 jumpHeight = new Vector2(0, 15.0f);
    private float moveSpeed = 4.5f;

    //! Player Movement
    float lastADPress = -0.1f;
    float adDoubleTapDelay = 0.2f;
    //float dashCooldown = 0.0f;
    public float dashDistance = 2.667f;
    float maxHorizontalScreenBoundary = 8.375f;
    float maxVerticalScreenBoundary = 4.45f;

    //! Player Attack
    public GameObject iceProjectile;
    public GameObject meleeAttackHitbox;
    public GameObject firstAbilityAttackHitbox;
    [HideInInspector]
    public float meleeAttackTimer;
    [HideInInspector]
    public float projectileAttackTimer;
    [HideInInspector]
    public float firstAbilityTimer;
    public float meleeAttackCooldown;
    public float projectileAttackCooldown;
    public float firstAbilityCooldown;
    private bool meleeInCooldown = false;
    private bool rangedInCooldown = false;
    private bool canUseFirstAbility = true;
    private bool isFirstAbiCooldown = false;

    //! Colliders
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private float colliderOffTime;

    //! boolean checkers
    private bool isGrounded = false;
    private bool isDash = false;
    public bool canPlayMoveAnimation = true;

    public static PlayerControllerScript instance { get; set; }

    private enum AttackType { IDLE , MELEE_ATK, RANGED_ATK, ABILITY1_ATK, ABILITY2_ATK }

    private AttackType currentAttackType;
    private Animator anim;
    private MeleeAttackHitboxScript childMelee;

    public float wait;

    public Image melee;
    public Image ranged;
    public Image teleport;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    
    void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        childMelee = transform.GetChild(0).GetComponent<MeleeAttackHitboxScript>();

        currentAttackType = AttackType.IDLE;
        currentHealth = maxHealth;
	}
	
	void Update ()
    {
        if(Time.timeScale != 0)
        {
            TurnAround();

            if (currentAttackType != AttackType.ABILITY1_ATK)
                Move();

            if (currentAttackType == AttackType.IDLE)
            {
                Attack();
                wait += Time.deltaTime;
            }

            CalculateMeleeCooldown();
            CalculateProjectileCooldown();
            CalculateFirstAbilityCooldown();
            HealthCheck();
        }   
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            StartCoroutine(JumpDelay(0.3f));
        }
    }
    
    //Controls dropping down platforms. rotationalOffset is reverted to 0 in PlatformScript.cs
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
            {
                collision.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
            }
        }
    }

    //Flip sprite and attackHitbox around based on mouse position and current player position
    private void TurnAround()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
            meleeAttackHitbox.transform.localPosition = new Vector2(1, meleeAttackHitbox.transform.localPosition.y);
        }
        else
        {
            spriteRenderer.flipX = true;
            meleeAttackHitbox.transform.localPosition = new Vector2(-2.5f, meleeAttackHitbox.transform.localPosition.y);
        }
    }

    //Method involving moving left and right, jumping and dashing
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Translate(moveHorizontal * moveSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x < -maxHorizontalScreenBoundary)
        {
            transform.position = new Vector3(-maxHorizontalScreenBoundary, transform.position.y);
        }
        else if (transform.position.x > maxHorizontalScreenBoundary)
        {
            transform.position = new Vector3(maxHorizontalScreenBoundary, transform.position.y);
        }

        if (transform.position.y > maxVerticalScreenBoundary)
        {
            transform.position = new Vector3(transform.position.x, maxVerticalScreenBoundary);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !Input.GetKey(KeyCode.S))
        {
            rb2D.AddForce(jumpHeight, ForceMode2D.Impulse);
            isGrounded = false;
            anim.Play("Jump");
        }

        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isDash && canPlayMoveAnimation)
        {
            anim.Play("Move");
        }

        if (Input.GetKeyDown(KeyCode.A) && isGrounded)
        {
            StartCoroutine(Dash(dashDistance));
        }

        if (Input.GetKeyDown(KeyCode.D) && isGrounded)
        {
            StartCoroutine(Dash(-dashDistance));
        }
    }

    //All attacks
    private void Attack()
    {
        /*
        if (Input.GetMouseButtonDown(0) && currentAttackType == AttackType.IDLE)
        {
            canPlayMoveAnimation = false;
            anim.Play("Ranged");
            ranged.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 0.75f);
            wait = 0.0f;

        }
        else if (!isDash && isGrounded && !Input.GetMouseButton(0) && !Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Q) 
            && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && wait > 0.5f)
        {
            anim.Play("Idle");
            ranged.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 1.0f);
            melee.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 1.0f);
            teleport.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 1.0f);
        }
        */

        if(!Input.GetMouseButton(0))
        {
            canPlayMoveAnimation = true;
        }

        if (Input.GetMouseButtonDown(0) && currentAttackType == AttackType.IDLE && !rangedInCooldown)
        {
            rangedInCooldown = true;
            anim.Play("Ranged");
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
            {
                Instantiate(iceProjectile, transform.position + new Vector3(0.85f, 0.7f, 0.0f), Quaternion.identity);
            }
            else
            {

                Instantiate(iceProjectile, transform.position + new Vector3(-0.85f, 0.7f, 0.0f), Quaternion.identity);
            }

            ranged.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 0.75f);
            currentAttackType = AttackType.RANGED_ATK;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ranged.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 1.0f);
        }

        if (Input.GetMouseButtonDown(1) && currentAttackType == AttackType.IDLE && isGrounded && !meleeInCooldown)
        {
            meleeAttackHitbox.SetActive(true);
            currentAttackType = AttackType.MELEE_ATK;
            melee.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 0.75f);
            canPlayMoveAnimation = false;
            anim.Play("Melee");
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentAttackType == AttackType.IDLE && isGrounded)
        {
            if (canUseFirstAbility == true)
            {
                anim.Play("Slam");
                firstAbilityAttackHitbox.SetActive(true);
                currentAttackType = AttackType.ABILITY1_ATK;
                canUseFirstAbility = false;
            }
        }
    }

    //Dash method
    private IEnumerator Dash(float dashDistance)
    {
        teleport.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 0.75f);
        if ((Time.time - lastADPress) < adDoubleTapDelay && currentAttackType != AttackType.MELEE_ATK /* && dashCooldown <= 0.0f */)
        {
            anim.Play("Teleport");
            isDash = true;
            yield return new WaitForSeconds(0.12f);
            transform.position = new Vector2(transform.position.x - dashDistance, transform.position.y);
            isDash = false;
            //dashCooldown = 10.0f;
        }

        lastADPress = Time.time;
        yield return null;
    }

    private IEnumerator JumpDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isGrounded = true;
        yield return null;
    }

    public void ChangeBacktoIdle()
    {
        currentAttackType = AttackType.IDLE;
    }

    private void CalculateMeleeCooldown()
    {
        if (currentAttackType == AttackType.MELEE_ATK && !meleeInCooldown)
        {
            meleeInCooldown = true;
            meleeAttackTimer = meleeAttackCooldown;
        }

        if (meleeAttackTimer > 0.0f)
        {
            meleeAttackTimer -= Time.deltaTime;
            ChangeBacktoIdle();
        }
        else if (meleeAttackTimer <= 0.0f)
        {
            melee.color = new Color(ranged.color.r, ranged.color.g, ranged.color.b, 1.0f);
            meleeInCooldown = false;
        }
    }

    private void CalculateProjectileCooldown()
    {
        if (currentAttackType == AttackType.RANGED_ATK && !rangedInCooldown)
        {
            //rangedInCooldown = true;
            projectileAttackTimer = projectileAttackCooldown;
        }

        if (projectileAttackTimer > 0.0f)
        {
            projectileAttackTimer -= Time.deltaTime;
            ChangeBacktoIdle();
        }
        else if (projectileAttackTimer <= 0.0f)
        {
            rangedInCooldown = false;
        }
    }

    private void CalculateFirstAbilityCooldown()
    {
        if (!canUseFirstAbility && !isFirstAbiCooldown)
        {
            firstAbilityTimer = firstAbilityCooldown;
            isFirstAbiCooldown = true;
        }

        if (firstAbilityTimer > 0.0f)
        {
            firstAbilityTimer -= Time.deltaTime;
        }
        else
        {
            isFirstAbiCooldown = false;
            canUseFirstAbility = true;
        }
    }

    private void HealthCheck()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (RabbitSpawnScript.instance.elaspedTime >= 60.0f)
        {
            Win();
        }
    }

    private void Die()
    {
        SceneManagerScript.instance.LoadScene(6);
    }

    private void Win()
    {
        SceneManagerScript.instance.LoadScene(7);
    }
}
