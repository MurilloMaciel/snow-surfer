using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashingDetector : MonoBehaviour
{
    [SerializeField] private float resetSceneDelay = 1F;
    [SerializeField] private ParticleSystem finishParticles;
    PlayerController _playerController;

    private void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (other.gameObject.layer == layerIndex)
        {
            _playerController.OnCrashPlayer();
            finishParticles.Play();
            Invoke(nameof(ResetScene), resetSceneDelay);
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
