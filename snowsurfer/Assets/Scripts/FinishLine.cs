using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float resetSceneDelay = 1F;
    [SerializeField] private ParticleSystem finishParticles;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == layerIndex)
        {
            finishParticles.Play();
            Invoke(nameof(ResetScene), resetSceneDelay);
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
