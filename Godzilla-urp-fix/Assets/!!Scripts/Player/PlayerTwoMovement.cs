using System;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerTwoMovement : MonoBehaviour
{
    public GameObject playerObject;
    public float moveSpeed = 5f;
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    public Sprite[] chargeAttackSprites;
    public Sprite[] tailAttackSprites;
    public Sprite[] headbuttSprites;
    public Sprite[] playerHitSprites;
    public Sprite[] playerDeadSprites;
    public float idleAnimationSpeed = 0.1f;
    public float runningAnimationSpeed = 0.05f;
    public float chargeAttackDuration = 1.5f;
    public float tailAttackDuration = 1.0f;
    public float headbuttDuration = 0.5f;
    public float playerHitDuration = 0.5f;
    public float playerDeadDuration = 0.5f;
    public int laserFireFrame = 6;

    private Vector3 facingDirection = Vector3.right;

    private SpriteRenderer spriteRenderer;
    private Rigidbody rb;
    private Vector3 movement;
    private float animationTimer;
    private int currentSpriteIndex;
    private bool isRunning;
    private bool isCharging;
    private bool isTailAttacking;
    private bool isHeadbutting;
    private bool isPlayerHit;
    private bool isPlayerDead;
    private bool isChargeComplete;
    private float chargeTimer;
    private bool isHoldingSprite;

    private float spacebarHoldTimer = 0f;
    private bool chargeAttackStarted = false;


    private ChargeAttackHandler chargeAttackHandler;
    private HeadbuttAttackHandler headbuttAttackHandler;
    private AerialAttackHandler aerialAttackHandler;
    private DashAttackHandler dashAttackHandler;

    public Sprite[] aerialAttackSprites;
    public float aerialAttackDuration = 2.0f;
    private bool isAerialAttacking;
    private bool isAerialAttackCharging;
    private float aerialAttackChargeTimer;
    private bool isAerialAttackInputReset = false;

    private float lastSpacebarPressTime = -Mathf.Infinity;
    private float doubleTapTimeWindow = 0.3f;
    
    private bool isTailAttackOnCooldown;
    private float tailAttackCooldownTimer;

    public float tailAttackCooldown = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        chargeAttackHandler = GetComponent<ChargeAttackHandler>();
        headbuttAttackHandler = GetComponent<HeadbuttAttackHandler>();
        aerialAttackHandler = GetComponentInChildren<AerialAttackHandler>();
        dashAttackHandler = GetComponent<DashAttackHandler>();


        animationTimer = 0f;
        currentSpriteIndex = 0;
        isRunning = false;
        isCharging = false;
        isTailAttacking = false;
        isHeadbutting = false;
        isPlayerHit = false;
        isPlayerDead = false;
        isChargeComplete = false;
        chargeTimer = 0f;
        isHoldingSprite = false;

        EventsManager.Instance.OnPlayer2Hit.AddListener(OnPlayerHit);
        EventsManager.Instance.OnPlayer2Dead.AddListener(OnPlayerDead);
    }

void Update()
{
    if (rb.position.y > 1.325834f)
    {
        Vector3 clampedPosition = rb.position;
        clampedPosition.y = 1.325834f;
        rb.MovePosition(clampedPosition);
    }
    if (movement.x < 0)
    {
        spriteRenderer.flipX = true;
        facingDirection = Vector3.left;
    }
    else if (movement.x > 0)
    {
        spriteRenderer.flipX = false;
        facingDirection = Vector3.right;
    }
    else if (movement.z != 0)
    {
        facingDirection = new Vector3(0, 0, movement.z).normalized;
    }

    if (isTailAttackOnCooldown)
    {
        tailAttackCooldownTimer += Time.deltaTime;
        if (tailAttackCooldownTimer >= tailAttackCooldown)
        {
            isTailAttackOnCooldown = false;
            tailAttackCooldownTimer = 0f;
        }
    }

    if (!isCharging && !isTailAttacking && !isHeadbutting && !isPlayerHit && !isPlayerDead && !isAerialAttacking)
    {
        movement.x = Input.GetAxisRaw("Horizontal_P2");
        movement.z = Input.GetAxisRaw("Vertical_P2");
        isRunning = movement != Vector3.zero;

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.M) && !dashAttackHandler.isDashing)
        {
            if (Time.time - lastSpacebarPressTime <= doubleTapTimeWindow)
            {
                Vector3 dashDirection = facingDirection;

                dashAttackHandler.StartDash(dashDirection);
            }

            lastSpacebarPressTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Semicolon) && !aerialAttackHandler.isOnCooldown)
        {
            if (!isAerialAttackCharging)
            {
                isAerialAttackCharging = true;
                aerialAttackChargeTimer = 0f;
                currentSpriteIndex = 0;
                animationTimer = 0f;

                aerialAttackHandler.StartCharging();
                aerialAttackHandler.ExecuteAerialAttack();
            }
        }
        else
        {
            if (isAerialAttackCharging)
            {
                isAerialAttackCharging = false;
                aerialAttackHandler.CancelCharging();
            }
        }

        if (Input.GetKey(KeyCode.M) && !chargeAttackHandler.isOnCooldown && !isCharging)
        {
            spacebarHoldTimer += Time.deltaTime;

            if (spacebarHoldTimer >= 0.5f && !chargeAttackStarted)
            {
                isCharging = true;
                chargeTimer = 0f;
                currentSpriteIndex = 0;
                isChargeComplete = false;
                isHoldingSprite = false;
                EventsManager.Instance.TriggerChargeAttack();

                chargeAttackStarted = true;
            }
        }
        else
        {
            spacebarHoldTimer = 0f;
            chargeAttackStarted = false;
        }

        if (Input.GetKeyDown(KeyCode.Slash) && !isTailAttacking && !isTailAttackOnCooldown)
        {
            isTailAttacking = true;
            isTailAttackOnCooldown = true;
            currentSpriteIndex = 0;
            animationTimer = 0f;
            SFXManager.Instance.PlayAudioWithVolume("thud", 6f);
            EventsManager.Instance.TriggerTailAttack(playerObject);
        }

        if (Input.GetKeyDown(KeyCode.Period) && !headbuttAttackHandler.isOnCooldown && !isHeadbutting)
        {
            isHeadbutting = true;
            currentSpriteIndex = 0;
            animationTimer = 0f;
            SFXManager.Instance.PlayAudioWithVolume("thud", 6f);
            EventsManager.Instance.TriggerHeadbutt(playerObject);
        }
    }
    if (isAerialAttackCharging)
    {
        AnimateAerialAttackCharge();
    }
    else if (isAerialAttacking)
    {
        AnimateAerialAttack();
    }
    else if (isCharging)
    {
        AnimateChargeAttack();
    }
    else if (isTailAttacking)
    {
        AnimateTailAttack();
    }
    else if (isHeadbutting)
    {
        AnimateHeadbutt();
    }
    else if (isPlayerHit)
    {
        AnimatePlayerHit();
    }
    else if (isPlayerDead)
    {
        AnimatePlayerDead();
    }
    else if (isRunning)
    {
        AnimateRunning();
    }
    else
    {
        AnimateIdle();
    }
}

    void AnimateAerialAttackCharge()
    {
        if (isPlayerHit)
        {
            return;
        }
        animationTimer += Time.deltaTime;
        float animationInterval = aerialAttackDuration / aerialAttackSprites.Length;

        if (animationTimer > animationInterval)
        {
            animationTimer -= animationInterval;
            currentSpriteIndex++;
            if (currentSpriteIndex >= aerialAttackSprites.Length)
            {
                currentSpriteIndex = aerialAttackSprites.Length - 1;
                isAerialAttackCharging = false;
                isAerialAttacking = true;
                aerialAttackHandler.ExecuteAerialAttack();
                isAerialAttacking = false;
                isAerialAttackCharging = false;
            }
            else
            {
                spriteRenderer.sprite = aerialAttackSprites[currentSpriteIndex];
            }
        }
    }

    void AnimateAerialAttack()
    {
        if (isPlayerHit)
        {
            return;
        }
        spriteRenderer.sprite = aerialAttackSprites[aerialAttackSprites.Length - 1];
    }

    void FixedUpdate()
    {
        if (!isCharging && !isTailAttacking && !isHeadbutting && !isPlayerHit && !isPlayerDead && !isAerialAttacking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void AnimateIdle()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer > idleAnimationSpeed)
        {
            animationTimer = 0f;
            currentSpriteIndex++;
            if (currentSpriteIndex >= idleSprites.Length)
            {
                currentSpriteIndex = 0;
            }

            spriteRenderer.sprite = idleSprites[currentSpriteIndex];
        }
    }

    void AnimateRunning()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer > runningAnimationSpeed)
        {
            animationTimer = 0f;
            currentSpriteIndex++;
            if (currentSpriteIndex >= runningSprites.Length)
            {
                currentSpriteIndex = 0;
            }

            spriteRenderer.sprite = runningSprites[currentSpriteIndex];
        }
    }

    void AnimateChargeAttack()
    {

        chargeTimer += Time.deltaTime;
        float animationInterval = chargeAttackDuration / chargeAttackSprites.Length;

        if (chargeTimer > animationInterval)
        {
            chargeTimer -= animationInterval;
            currentSpriteIndex++;
            if (currentSpriteIndex >= chargeAttackSprites.Length)
            {
                currentSpriteIndex = chargeAttackSprites.Length - 1;
                isChargeComplete = true;
                isCharging = false;
                chargeAttackHandler.FireLaser();
            }
            else
            {
                spriteRenderer.sprite = chargeAttackSprites[currentSpriteIndex];
                if (currentSpriteIndex == laserFireFrame)
                {
                    chargeAttackHandler.FireLaser();
                }
            }
        }
    }

    void AnimateTailAttack()
    {
        if (!chargeAttackHandler.isOnCooldown)
        {
            animationTimer += Time.deltaTime;
            float animationInterval = tailAttackDuration / tailAttackSprites.Length;

            if (animationTimer > animationInterval)
            {
                animationTimer -= animationInterval;
                currentSpriteIndex++;
                if (currentSpriteIndex >= tailAttackSprites.Length)
                {
                    currentSpriteIndex = tailAttackSprites.Length - 1;
                    isTailAttacking = false;
                }
                else
                {
                    spriteRenderer.sprite = tailAttackSprites[currentSpriteIndex];
                }
            }
        }
    }

    void AnimateHeadbutt()
    {
        animationTimer += Time.deltaTime;
        float animationInterval = headbuttDuration / headbuttSprites.Length;

        if (animationTimer > animationInterval)
        {
            animationTimer -= animationInterval;
            currentSpriteIndex++;
            if (currentSpriteIndex >= headbuttSprites.Length)
            {
                currentSpriteIndex = headbuttSprites.Length - 1;
                isHeadbutting = false;
            }
            else
            {
                spriteRenderer.sprite = headbuttSprites[currentSpriteIndex];

                if (currentSpriteIndex == 3)
                {
                    headbuttAttackHandler.ActivateHeadbuttCollider();
                }
            }
        }
    }

    void AnimatePlayerHit()
    {
        animationTimer += Time.deltaTime;
        float animationInterval = playerHitDuration / playerHitSprites.Length;

        if (animationTimer > animationInterval)
        {
            animationTimer -= animationInterval;
            currentSpriteIndex++;
            if (currentSpriteIndex >= playerHitSprites.Length)
            {
                currentSpriteIndex = playerHitSprites.Length - 1;
                isPlayerHit = false;
            }
            else
            {
                spriteRenderer.sprite = playerHitSprites[currentSpriteIndex];
            }
        }
    }

    void AnimatePlayerDead()
    {
        animationTimer += Time.deltaTime;
        float animationInterval = playerDeadDuration / playerDeadSprites.Length;

        if (animationTimer > animationInterval)
        {
            animationTimer -= animationInterval;
            currentSpriteIndex++;
            if (currentSpriteIndex >= playerDeadSprites.Length)
            {
                spriteRenderer.sprite = playerDeadSprites[playerDeadSprites.Length - 1];
                this.enabled = false;
            }
            else
            {
                spriteRenderer.sprite = playerDeadSprites[currentSpriteIndex];
            }
        }
    }

    void OnPlayerHit(string eventName)
    {
        isPlayerHit = true;
        currentSpriteIndex = 0;
        animationTimer = 0f;

        isCharging = false;
        isTailAttacking = false;
        isHeadbutting = false;
        isAerialAttacking = false;
        isAerialAttackCharging = false;

        //chargeAttackHandler.CancelCharging();
        //headbuttAttackHandler.CancelAttack();
        aerialAttackHandler.CancelCharging();

        isAerialAttackInputReset = true;
    }


    void OnPlayerDead(string eventName)
    {
        isPlayerDead = true;
        currentSpriteIndex = 0;
        animationTimer = 0f;
    }
}
