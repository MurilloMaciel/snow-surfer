using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 40F;
    [SerializeField] private ParticleSystem powerUpParticles;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float accelerationForce = 50f;
    [SerializeField] private float boostAccelerationForce = 1000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float boostMaxSpeed = 35f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3F;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 10f;

    private InputAction _jumpAction;
    private bool _jumpRequested;
    private bool _isBoosting;
    private bool _isGrounded;
    private InputAction _moveAction;
    private Rigidbody2D _rigidBody2D;
    private Vector2 _moveVector2;
    private bool _canControlPlayer = true;
    private int _activePowerUpCount;
    private float _previousRotation;
    private float _totalRotation;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void OnCrashPlayer()
    {
        _canControlPlayer = false;
    }
    
    private void Update()
    {
        _moveVector2 = _moveAction.ReadValue<Vector2>();
        CalculateFlips();
        if (_jumpAction.WasPressedThisFrame())
        {
            _jumpRequested = true;
        }
    }
    
    private void Jump()
    {
        if (!_isGrounded) return;
        _rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private void FixedUpdate()
    {
        if (!_canControlPlayer) return;
        CheckGround();
        MovePlayer();
        RotatePlayer();
        if (_jumpRequested)
        {
            Jump();
            _jumpRequested = false;
        }
    }
    
    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
    
    private void RotatePlayer()
    {
        switch (_moveVector2.x)
        {
            case < 0:
                _rigidBody2D.AddTorque(torqueAmount);
                break;
            case > 0:
                _rigidBody2D.AddTorque(-torqueAmount);
                break;
        }
    }
    
    private void MovePlayer()
    {
        if (!_isGrounded) return;

        float force = _moveVector2.y * accelerationForce;
        _rigidBody2D.AddForce(transform.right * force);

        float currentMaxSpeed = _isBoosting ? boostMaxSpeed : maxSpeed;

        var velocity = _rigidBody2D.linearVelocity;
        velocity = Vector2.ClampMagnitude(velocity, currentMaxSpeed);
        _rigidBody2D.linearVelocity = velocity;
    }

    private void CalculateFlips()
    {
        var currentRotation = transform.rotation.eulerAngles.z;
        _totalRotation += Mathf.DeltaAngle(_previousRotation, currentRotation);
        if (_totalRotation is > 340 or < -340)
        {
            _totalRotation = 0;
            scoreManager.AddScore(100);
        } 
        _previousRotation = currentRotation;
    }

    public void ActivatePowerUp(PowerUpSo powerUpSo)
    {
        powerUpParticles.Play();
        _activePowerUpCount++;
        if (powerUpSo.GetPowerUpType() == "speed")
        {
            _isBoosting = true;
            _rigidBody2D.AddForce(transform.right * (_moveVector2.y * boostAccelerationForce));
        } 
        else if (powerUpSo.GetPowerUpType() == "torque")
        {
            torqueAmount += powerUpSo.GetValueChange();
        }
    }

    public void DeactivatePowerUp(PowerUpSo powerUpSo)
    {
        if (--_activePowerUpCount == 0)
        {
            powerUpParticles.Stop();
        }
        if (powerUpSo.GetPowerUpType() == "speed")
        {
            _isBoosting = false;
        }
        else if (powerUpSo.GetPowerUpType() == "torque")
        {
            torqueAmount -= powerUpSo.GetValueChange();
        }
    }
}
