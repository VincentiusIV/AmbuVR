using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public Gradient particleColorGradient;
    public ParticleDecalPool splatDecalPool;

    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            LaunchParticle();
        }
    }

    public void LaunchParticle()
    {
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
        particleLauncher.Emit(1);
    }

    void EmitAtLocation(ParticleCollisionEvent pce)
    {
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        if (other.CompareTag("Burn"))
            other.GetComponent<IA_Area>().ApplyMed(IA_Tags.Water);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            splatDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
            //EmitAtLocation(collisionEvents[i]);
        }
    }

}
