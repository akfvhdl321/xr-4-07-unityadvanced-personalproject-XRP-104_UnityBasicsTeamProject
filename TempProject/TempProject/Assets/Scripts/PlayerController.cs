using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    PlayerStateMachine _stateMachine;
    InputSystem_Actions _inputActions;

    private PlayerHealth _playerHealth;
    private PlayerRespawn _playerRespawn;

    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckDistance = 0.1f;
    [SerializeField] LayerMask _groudLayer;

    [Header("Fireball Pool")]
    [SerializeField] private FireballPool _fireballPoolPrefab;
    private FireballPool _fireballPool;

    [Header("Fire Mode")]
    [SerializeField] private bool _isFireMode = false;

    [Header("Jump Boost")]
    [SerializeField] private float _jumpBoostMultiplier = 1.5f;
    [SerializeField] private float _jumpBoostDuration = 5f;

    [Header("Death")]
    [SerializeField] private float _deathY = -10f;

    [Header("Hit Knockback")]
    [SerializeField] private float _knockbackForceX = 5f;
    [SerializeField] private float _knockbackForceY = 3f;
    [SerializeField] private float _knockbackDuration = 0.2f;

    private bool _isKnockback;

    private bool _isJumpBoost;
    private float _jumpBoostEndTime;

    [SerializeField] private float _fireCooldown = 0.3f;
    private float _lastFireTime;

    // 현재 밟고 있는 플랫폼
    private Rigidbody2D _currentPlatform;

    // 플랫폼 이전 위치 (플랫폼 이동량 계산)
    private Vector2 _lastPlatformPosition;

    public Rigidbody2D _rb { get; private set; }
    public Animator _animator { get; private set; }

    public float _moveInput { get; private set; }
    public bool _isGrounded { get; private set; }

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] public float _jumpForce = 5f;

    [SerializeField] private float _coyoteTime = 0.15f;
    [SerializeField] private float _jumpBufferTime = 0.15f;

    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    public IdleState Idle { get; private set; }
    public MoveState Move { get; private set; }
    public JumpState Jump { get; private set; }

    private Vector3 _originalScale;
    private bool _isFacingRight = true;

    // private bool _isDead;


    // ===============================
    // UI 접근용 프로퍼티
    // ===============================
    public bool IsJumpBoostActive => _isJumpBoost;
    public bool IsFireModeActive => _isFireMode;

    public float JumpBoostRemainingTime =>
        Mathf.Max(0f, _jumpBoostEndTime - Time.time);

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        // FireballPool 자동 생성
        if (_fireballPoolPrefab != null)
        {
            _fireballPool = Instantiate(_fireballPoolPrefab);
        }
        
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        if (_inputActions == null) return;

        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Jump.performed -= OnJump;

        _inputActions.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
        ApplyPlatformMovement();
    }

    private void Update()
    {
        IsGround();
        Flip();
        HandleCoyoteTime();
        HandleJumpBuffer();

        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VelocityY", _rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.F))
            Fire();

        // JumpBoost 시간 만료 처리
        if (_isJumpBoost && Time.time >= _jumpBoostEndTime)
            DisableJumpBoost();

        _stateMachine.Update();

        CheckFallDeath();

    }

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        
        _stateMachine = new PlayerStateMachine();
        

        _originalScale = transform.localScale;

        Idle = new IdleState(this);
        Move = new MoveState(this);
        Jump = new JumpState(this);

        _stateMachine.ChangeState(Idle);

    }

    public void ChangeState(IPlayerState newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public void ApplyKnockback(Vector2 attackerPosition)
    {
        if (_isKnockback) return;

        float dir = transform.position.x > attackerPosition.x ? 1f : -1f;

        _rb.linearVelocity = new Vector2(
            dir * _knockbackForceX,
            _knockbackForceY
        );

        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        _isKnockback = true;

        yield return new WaitForSeconds(_knockbackDuration);

        _isKnockback = false;
    }

    // ===============================
    // Fire
    // ===============================
    public void Fire()
    {
        Debug.Log(" Fire() 호출됨");
        if (!_isFireMode)
        {
            Debug.Log("FireMode 꺼져있음");
            return;
        }
        if (Time.time < _lastFireTime + _fireCooldown) return;

        _lastFireTime = Time.time;

        Fireball fb = _fireballPool.GetFireball();
        if (fb == null)
        {
            Debug.Log("Fireball null 반환됨");
            return;
        }

        fb.transform.position = transform.position;

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // 팀 정보 전달
        fb.Init(dir, TeamType.Player);
    }

    private void CheckFallDeath()
    {
        if (transform.position.y < _deathY)
        {
            _playerHealth.TakeDamage(999, transform.position);
        }
    }

    public void EnableFireMode()
    {
        _isFireMode = true;
        Debug.Log("Fire Mode 활성화됨");
    }

    public void DisableFireMode() => _isFireMode = false;

    // ===============================
    // Jump Boost 시스템
    // ===============================
    public void EnableJumpBoost()
    {
        _isJumpBoost = true;
        _jumpBoostEndTime = Time.time + _jumpBoostDuration; // 갱신 방식

        Debug.Log("점프 부스트 활성화");
    }

    public void DisableJumpBoost()
    {
        if (!_isJumpBoost) return;

        _isJumpBoost = false;

        Debug.Log($"점프 부스트 해제 (지속시간: {_jumpBoostDuration}초)");
    }

    public float GetFinalJumpForce()
    {
        return _isJumpBoost
            ? _jumpForce * _jumpBoostMultiplier
            : _jumpForce;
    }

    // ===============================
    // Jump 처리
    // ===============================
    private void HandleCoyoteTime()
    {
        if (_isGrounded)
            _coyoteTimeCounter = _coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;
    }

    private void HandleJumpBuffer()
    {
        if (_jumpBufferCounter > 0)
            _jumpBufferCounter -= Time.deltaTime;
    }

    public bool CanJump()
    {
        return _coyoteTimeCounter > 0 && _jumpBufferCounter > 0;
    }

    public void ConsumeJump()
    {
        _coyoteTimeCounter = 0;
        _jumpBufferCounter = 0;
    }

    public void PerformJump()
    {
        float finalJumpForce = GetFinalJumpForce();

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocity.x,
            finalJumpForce);
    }

    private void CutJump()
    {
        if (_rb.linearVelocity.y > 0)
        {
            _rb.linearVelocity = new Vector2(
                _rb.linearVelocity.x,
                _rb.linearVelocity.y * 0.5f);
        }
    }

    // ===============================
    // 이동
    // ===============================
    void Movement()
    {
        if (_isKnockback)
            return;

        _rb.linearVelocity = new Vector2(
            _moveInput * _moveSpeed,
            _rb.linearVelocity.y);
    }

    // ===============================
    // 방향 전환
    // ===============================
    private void Flip()
    {
        if (_moveInput > 0 && !_isFacingRight)
        {
            _isFacingRight = true;
            SetScaleDirection(1);
        }
        else if (_moveInput < 0 && _isFacingRight)
        {
            _isFacingRight = false;
            SetScaleDirection(-1);
        }
    }

    private void SetScaleDirection(int dir)
    {
        Vector3 scale = _originalScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    // ===============================
    // Ground 체크
    // ===============================
    void IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _groundCheck.position,
            Vector2.down,
            _groundCheckDistance,
            _groudLayer);

        Debug.DrawRay(
            _groundCheck.position,
            Vector2.down * _groundCheckDistance,
            Color.green);

        _isGrounded = hit.collider != null;

        if (_isGrounded)
        {
            Rigidbody2D platformRb = hit.collider.GetComponent<Rigidbody2D>();

            if (platformRb != null)
            {
                if (_currentPlatform != platformRb)
                {
                    _currentPlatform = platformRb;
                    _lastPlatformPosition = platformRb.position;
                }
            }
            else
            {
                _currentPlatform = null;
            }
        }
        else
        {
            _currentPlatform = null;
        }
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        _moveInput = value.x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _jumpBufferCounter = _jumpBufferTime;

        if (ctx.canceled)
            CutJump();
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            _groundCheck.position,
            _groundCheck.position + Vector3.down * _groundCheckDistance);
    }

    private void ApplyPlatformMovement()
    {
        if (!_isGrounded) return;
        if (_currentPlatform == null) return;

        Vector2 platformDelta = _currentPlatform.position - _lastPlatformPosition;

        // 플레이어 위치에 플랫폼 이동량 적용
        _rb.position += platformDelta;

        _lastPlatformPosition = _currentPlatform.position;
    }
}