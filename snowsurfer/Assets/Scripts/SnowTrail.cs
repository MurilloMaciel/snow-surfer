using System;
using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    private int _layerIndex;
    [SerializeField] private ParticleSystem snowTrailParticles;

    private void Start()
    {
        _layerIndex = LayerMask.NameToLayer("Floor");
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            snowTrailParticles.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            snowTrailParticles.Play();
        }
    }
}
