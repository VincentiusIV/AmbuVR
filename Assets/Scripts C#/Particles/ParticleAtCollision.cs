using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAtCollision : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public ParticleSystem particleSystemToSpawn;
    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void EmitAtLocation(ParticleCollisionEvent pce)
    {
        particleSystemToSpawn.transform.position = pce.intersection;
        particleSystemToSpawn.Emit(1);
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
