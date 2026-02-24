using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSo", menuName = "PowerUpSo")]
public class PowerUpSo : ScriptableObject
{
    [SerializeField] private string powerUpType;
    [SerializeField] private float valueChange;
    [SerializeField] private float timeDuration;
    
    public string GetPowerUpType() => powerUpType;

    public float GetValueChange() => valueChange;

    public float GetTimeDuration() => timeDuration;
}
