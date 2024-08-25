using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VidTools;
using VidTools.Vis;

public class FluidBehaviour : MonoBehaviour
{
    [Header("Initial Settings")]
    public int numPoints;
    public float particleSpacing;

    [Header("Visualization Settings")]
    public float particleSize;

    [Header("Simulation Settings")]
    [Range(0, 1)] public float bounceCoef;
    public float smoothingRadius;
    public float targetDensity;
    public Vector2 boundSize;
    public Vector2 gravityDir;

    [Header("Arrays (temporary)")]
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
        velocities[particle_index] += gravityDir * -1 * Time.deltaTime;

        //other effects here

        //resolve colision with wall
        points[particle_index] += (velocities[particle_index] * Time.deltaTime);

        Vector2 halfBounds = boundSize / 2 - Vector2.one * particleSize;
        if (Math.Abs((points[particle_index].x)) > halfBounds.x)
        {
            points[particle_index].x = halfBounds.x * Math.Sign(points[particle_index].x);
            velocities[particle_index].x *= -1 * bounceCoef;
        }

        if(Math.Abs((points[particle_index]).y) > halfBounds.y)
        {
            points[particle_index].y = halfBounds.y * Math.Sign(points[particle_index].y);
            velocities[particle_index].y *= -1 * bounceCoef;
        }
    }

    public float calculate_density (int particle_index)
    {
        float density = 0;
        const float particle_mass = 1.0f;

        for(int i = 0; i < n_points_priv; i++)
        {
            if (i == particle_index) continue;
            float dst = (points[particle_index] - points[i]).magnitude;
            if(dst > smoothingRadius) continue;

            density += smoothing_kernel(dst) * particle_mass;

        }

        return density;
    }

    private float smoothing_kernel (float dst)
    {
        float volume = (float)(Math.PI * Math.Pow(dst, 4) / 2);
        float value = Math.Max(0, dst);

        return value * value * value / volume;
    }
}
