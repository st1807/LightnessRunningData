using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    Terrain terrain;
    public int dis =100;
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrain.detailObjectDistance = dis;   
    }

    // void Update()
    // {
    //     if (Input.GetKeyUp(KeyCode.Space))
    //     {
    //         terrain.detailObjectDistance = dis;  
    //     }
    // }
}
