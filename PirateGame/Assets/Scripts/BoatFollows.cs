using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ArchimedsLab;

public class BoatFollows : MonoBehaviour
{
    public int offset = 3;
    public GameObject boat;
    float getHeight(Vector3 pos)
    {
        const float eps = 0.5f;
        return (OceanAdvanced.GetWaterHeight(pos + new Vector3(-eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(0F, 0F, eps))) / 3F;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 final = new Vector3(transform.position.x, getHeight(transform.position) + offset, transform.position.z);

       
        transform.position = Vector3.Lerp(transform.position, final, 0.9f);
        boat.transform.LookAt(transform);
    }
}
