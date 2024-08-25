using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VidTools;
using VidTools.Vis;

public class FluidBehaviour : MonoBehaviour
{
    public int numPoints;
    public float particleSize;
    public float particleSpacing;
    public float bounceCoef;
    public Vector2 boundSize;
    public Vector2 gravityDir;
    public Vector2[] points;
    public Vector2[] velocities;

    private int n_points_priv;
    private static float min_vel_on_colision = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        n_points_priv = numPoints;
        points = new Vector2[n_points_priv];
        velocities = new Vector2[n_points_priv];

        int particlesPerRow = (int)Math.Sqrt(n_points_priv);
        int particlesPerCol = (n_points_priv - 1) / (particlesPerRow + 1);
        float spacing = particleSize * 2 + particleSpacing;

        for (int i = 0; i < n_points_priv; i++)
        {
            float x = (i % particlesPerRow - particlesPerRow / 2f + 0.5f) * spacing;
            float y = (i / particlesPerRow - particlesPerCol / 2f + 0.5f) * spacing;
            points[i] = new Vector2(x, y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Draw.BoxOutline(Vector2.zero, boundSize, 0.05f, Color.white);
        for (int i = 0; i < n_points_priv; i++)
        {
            updateParticle(i);
            Draw.Point(new Vector3(points[i].x, points[i].y, 0), particleSize, Color.white);

        }
        
    }

    void updateParticle (int particle_index)
    {
        //gravity effect
        velocities[particle_index] += gravityDir * -1;

        //other effects here

        //predict and resolve colision with wall
        Vector2 predictedPos = points[particle_index] + (velocities[particle_index] * Time.deltaTime);
        if (Math.Abs((predictedPos.x)) > 8)
        {
            float newX = 8 * Math.Sign(predictedPos.x);
            predictedPos.x = newX;
            velocities[particle_index].x *= -1 * bounceCoef;
            if (Math.Abs(velocities[particle_index].x) < min_vel_on_colision) velocities[particle_index].x = 0;
        }

        if(Math.Abs((points[particle_index] + (velocities[particle_index] * Time.deltaTime)).y) > 4.5f)
        {
            float newY = 4.5f * Math.Sign(predictedPos.y);
            predictedPos.y = newY;
            velocities[particle_index].y *= -1 * bounceCoef;
            if (Math.Abs(velocities[particle_index].y) < min_vel_on_colision) velocities[particle_index].y = 0;
        }

        //move particles
        points[particle_index] = predictedPos;
    }
}
