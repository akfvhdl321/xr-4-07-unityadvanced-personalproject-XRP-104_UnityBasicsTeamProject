using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerStateMachine _stateMachine;
    InputSystem_Actions _inputActions;

    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckDistance = 0.01f;
    [SerializeField] LayerMask _groudLayer;


    public Rigidbody2D _rb { get; private set; }

    public Animator _animator {  get; private set; }

    public float _moveInput {  get; private set; }
    public bool _isGrounded {  get; private set; }

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] public float _jumpForce = 5f;

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
        _stateMachine.Update();
    }

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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

    private void Flip()
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

        if (hit.collider != null)
        {
            _isGrounded = true;
        }

        else
        {
            _isGrounded = false;
        }
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        _moveInput = value.x;
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (!_isGrounded) return;

        _stateMachine.ChangeState(Jump);
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
