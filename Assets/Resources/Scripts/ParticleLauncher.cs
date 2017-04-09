using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public ParticleSystem splatterParticles;

    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    private void Update()
    {
        if(Input.GetButton("Fire1"))
            particleLauncher.Emit(1);


    }

    void EmitAtLocation(ParticleCollisionEvent pce)
    {
        splatterParticles.transform.position = pce.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation(pce.normal);
        splatterParticles.Emit(1);
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            EmitAtLocation(collisionEvents[i]);
        }
        
    }
}
