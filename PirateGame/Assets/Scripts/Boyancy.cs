using UnityEngine;

//  --- How to setup ---
//
//   -Add box Collider on Water Body and set it as Trigger.
//   -Also Add "WaterBody" component On it (optionally you can change surface level property if needed)
//   -Give water object Tag as "Water" (or you can change "waterVolumeTag" property accordingly)
//   -Add "Buoyancy" component on a Rigidbody Object that you want to float on WaterBody.
//
//
//
//  --- What this Dose ---
//
//   -Rigidbody with "Buoyency" Component floats on "Water" Tagged body.
//   -Check if Rigidbody is inside X - Z bound of Water body and gives force on Y Upwards.
//   -Buoyant force (Upwards force) increases as rigibody dive deep underwater. (can be variable by depthPowerLimit property)
//
//
//
//  --- Limitations ---
//
//   -This is clearly NOT real world Physics, just simple Up force.
//   -No additional forces or drag or waves.
//   -As this component is limited to Y axis buoyancy, make sure your Y is up, however feel free to modify.
//   -Other collider types will work as long as water surface level stay flat, as this is designed on checking collider bounding box.
//   -Rigidbody never fall Asleep if inside water body.
//   -Don't stack water Bodies on top or duplicate on same location.

using ArchimedsLab;


[RequireComponent(typeof(Rigidbody))]
public class Boyancy : MonoBehaviour
{
    //  ▀▄▀▄▀▄ Editor Variables ▄▀▄▀▄▀

    [SerializeField, Tooltip("increase value to make object more buoyant, default 8.")]
    float buoyantForce = 8f;
    [SerializeField, Tooltip("value 0 mean no additional Buoyant Force underwater, 1 mean Double buoyant Force underwater (underwater pressure)"),
    Range(0f, 1f)]
    float depthPower = 1f;
    [SerializeField, Tooltip("Center of Mass on Y axis (kind of), default 0.")]
    float offsetY = 0f;
    [SerializeField, Tooltip("Tag of the Water Body")]
    string waterVolumeTag = "Water";


    //  ▀▄▀▄▀▄ Private Variables ▄▀▄▀▄▀

    private Rigidbody rb;
    private Collider coll;
    private WaterBody waterBody;
    private float yBound;
    private bool isWaterBodySet;
    private int waterCount;

    public float shipOffsetGrounnd = 10;

    //  ▀▄▀▄▀▄ Core Functions ▄▀▄▀▄▀


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (waterCount == 0)
        {
            waterBody = null;
            isWaterBodySet = false;
        }
    }



    //  ▀▄▀▄▀▄ Shared Functions ▄▀▄▀▄▀


    //Set and Get for Under water Buoyancy (depth pressure)(0 to 1 range).
    public void SetDepthPower(in float value)
    {
        if (value >= 0f && value <= 1f) depthPower = value;
    }

    public float GetDepthPower() => depthPower;



    //  ▀▄▀▄▀▄ Trigger Functions ▄▀▄▀▄▀



    public static float getHeight (Vector3 pos)
    {
       
        const float eps = 0.5f;
        return (OceanAdvanced.GetWaterHeight(pos + new Vector3(-eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(0F, 0F, eps))) / 3F;
    }
    private void FixedUpdate()
    {
        
        if (getHeight(transform.position) > transform.position.y )
        {

            float objectYValue = getHeight(transform.position);
            // we are underwater
            float buoyantForceMass = buoyantForce * rb.mass;
            float underWaterBuoyantForce =  objectYValue * depthPower;
            float buoyency = buoyantForceMass + (buoyantForceMass * underWaterBuoyantForce); 
            rb.AddForce(0f, buoyency, 0f );
        }

        rb.AddForce(0f, -9.8f, 0f);
    }
    /*
    private void OnTriggerStay(Collider water)
    {
        //if this object inside Water, it object start floating,

        if (water.CompareTag(waterVolumeTag))
        {
            if (transform.position.x < water.bounds.max.x
            && transform.position.z < water.bounds.max.z
            && transform.position.x > water.bounds.min.x
            && transform.position.z > water.bounds.min.z)
            {
                if (waterBody != null && !ReferenceEquals(waterBody.gameObject, water.gameObject))
                {
                    waterBody = null;
                    isWaterBodySet = false;
                }

                if (!isWaterBodySet)
                {
                    waterBody = water.GetComponent<WaterBody>();
                    if (waterBody != null) isWaterBodySet = true;
                }
                else
                {
                    float objectYValue = coll.bounds.center.y + offsetY;
                    yBound = waterBody.GetYBound();
                    if (objectYValue < yBound)
                    {
                        float buoyantForceMass = buoyantForce * rb.mass;
                        float underWaterBuoyantForce = Mathf.Clamp01((yBound - objectYValue) * depthPower); //can be inline below
                        float buoyency = buoyantForceMass + (buoyantForceMass * underWaterBuoyantForce); //can be inline below
                        rb.AddForce(0f, buoyency, 0f);
                    }
                }
            }
        }
    }
    */

}