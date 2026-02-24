using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private PowerUpSo powerUpSo;
    private PlayerController _playerController;
    private SpriteRenderer _spriteRenderer;
    private float _timeLeft;

    private void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timeLeft = powerUpSo.GetTimeDuration();
    }

    private void Update()
    {
        CountDownTimer();
    }

    private void CountDownTimer()
    {
        if (_spriteRenderer.enabled) return;
        if (_timeLeft >= 0)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                _playerController.DeactivatePowerUp(powerUpSo);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var layerIndex = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer != layerIndex || !_spriteRenderer.enabled) return;
        _spriteRenderer.enabled = false;
        _playerController.ActivatePowerUp(powerUpSo);
    }
}
