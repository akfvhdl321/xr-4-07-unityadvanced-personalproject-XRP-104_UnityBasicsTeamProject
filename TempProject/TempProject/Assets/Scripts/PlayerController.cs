using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private FireballPool _fireballPool;

    [Header("Fire Mode")]
    [Tooltip("Fire Mode 활성 여부")]
    [SerializeField] private bool _isFireMode = false;


    [SerializeField] private float _fireCooldown = 0.3f;

    private float _lastFireTime;


    public Rigidbody2D _rb { get; private set; }

    public Animator _animator {  get; private set; }

    public float _moveInput {  get; private set; }
    public bool _isGrounded {  get; private set; }

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] public float _jumpForce = 5f;

    // 코요테 타임(땅에서 떨어진 뒤 점프 허용 시간)
    [SerializeField] private float _coyoteTime = 0.15f;

    // 점프 입력시간
    [SerializeField] private float _jumpBufferTime = 0.15f;

    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    public IdleState Idle {  get; private set; }
    public MoveState Move { get; private set; }
    public JumpState Jump { get; private set; }

    private Vector3 _originalScale;
    private bool _isFacingRight = true;


    private void Awake()
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
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Jump.performed -= OnJump;

        _inputActions.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
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
        {
            Fire();
        }

        _stateMachine.Update();
    }

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerRespawn = GetComponent<PlayerRespawn>();
        GameManager.Instance.RegisterPlayer(_playerRespawn, _playerHealth);
        _stateMachine = new PlayerStateMachine();
        _inputActions = new InputSystem_Actions();
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

    public void Fire()
    {
        if (!_isFireMode) return;

        if (Time.time < _lastFireTime + _fireCooldown)
            return;

        _lastFireTime = Time.time;

        Fireball fb = _fireballPool.GetFireball();

        if (fb == null)
            return;

        fb.transform.position = transform.position;

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        fb.Init(dir);
    }

    // Fire Mode 활성화
    public void EnableFireMode()
    {
        _isFireMode = true;
    }

    // Fire Mode 비활성화
    public void DisableFireMode()
    {
        _isFireMode = false;
    }

    private void HandleCoyoteTime()
    {
        if (_isGrounded)
        {
            // 땅에 있으면 카운터 리셋
            _coyoteTimeCounter = _coyoteTime;
        }

        else
        {
            // 공중이면 시간 감소
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleJumpBuffer()
    {
        if (_jumpBufferCounter > 0)
        _jumpBufferCounter -= Time.deltaTime;
    }

    public bool CanJump()
    {
        bool result = _coyoteTimeCounter > 0 && _jumpBufferCounter > 0;

        return result;
    }

    public void ConsumeJump()
    {
        _coyoteTimeCounter = 0;
        _jumpBufferCounter = 0;
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

    private void Flip() // 방향전환
    {
        if(_moveInput > 0 && !_isFacingRight)
        {
            _isFacingRight = true;

            Vector3 scale = _originalScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        else if(_moveInput < 0 && _isFacingRight)
        {
            _isFacingRight = false;

            Vector3 scale = _originalScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _groundCheck.position,
            Vector2.down,
            _groundCheckDistance,
            _groudLayer);

        Debug.DrawRay(_groundCheck.position,
        Vector2.down * _groundCheckDistance,
        Color.green);

        _isGrounded = hit.collider != null;

        if (_isGrounded)
            Debug.Log("바닥 감지됨");
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        _moveInput = value.x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("점프 입력 감지됨");
            _jumpBufferCounter = _jumpBufferTime;
        }

        if (ctx.canceled)
        {
            CutJump();
        }

    }

    void Movement()
    {
        _rb.linearVelocity = new Vector2(_moveInput * _moveSpeed, _rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_groundCheck.position,
            _groundCheck.position + Vector3.down * _groundCheckDistance);
    }
}
