using UnityEngine;
using System.Collections;

public class ExtraUtils  {

    public static void MoveParticleSystem(GameObject go, Vector3 translation)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        if (ps == null)
            ps = go.GetComponentInChildren<ParticleSystem>();

        ParticleSystem.Particle[] parts = new ParticleSystem.Particle[ps.maxParticles];
        int iParts = ps.GetParticles(parts);
        for (int i = 0; i < iParts; i++)
            parts[i].position += translation;
        ps.SetParticles(parts, iParts);
    }
}
