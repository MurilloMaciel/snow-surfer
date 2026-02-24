using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1F;
    [SerializeField] private float baseSpeed = 15F;
    [SerializeField] private float boostSpeed = 20F;
    [SerializeField] private ParticleSystem powerUpParticles;
    [SerializeField] private ScoreManager scoreManager;
    
    private InputAction _moveAction;
    private Rigidbody2D _rigidBody2D;
    private Vector2 _moveVector2;
    private SurfaceEffector2D _surfaceEffector2D;
    private bool _canControlPlayer = true;
    private int _activePowerUpCount = 0;
    private float _previousRotation = 0F;
    private float _totalRotation = 0F;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _surfaceEffector2D = FindFirstObjectByType<SurfaceEffector2D>();
    }

    public void OnCrashPlayer()
    {
        _canControlPlayer = false;
    }

    private void Update()
    {
        if (_canControlPlayer)
        {
            RotatePlayer();
            BoostPlayer();
            CalculateFlips();
        }
    }
    
    private void RotatePlayer()
    {
        _moveVector2 = _moveAction.ReadValue<Vector2>();
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

    private void BoostPlayer()
    {
        _surfaceEffector2D.speed = _moveVector2.y > 0 ? boostSpeed : baseSpeed;
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
            baseSpeed += powerUpSo.GetValueChange();
            boostSpeed += powerUpSo.GetValueChange();
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
            baseSpeed -= powerUpSo.GetValueChange();
            boostSpeed -= powerUpSo.GetValueChange();
        } 
        else if (powerUpSo.GetPowerUpType() == "torque")
        {
            torqueAmount -= powerUpSo.GetValueChange();
        }
    }
}
