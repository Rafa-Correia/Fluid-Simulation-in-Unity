using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VidTools;
using VidTools.Vis;

public class FluidBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Draw.Point(Vector3.zero, 0.0f, Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        Draw.Point(Vector3.zero, 1.0f, Color.white);
    }
}
