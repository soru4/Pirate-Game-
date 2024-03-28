using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int MaxHP; //max value for healthbar
    public int CurHp;  // value for health bar to display
    public Slider HPSlider;

    public void Update()
    {
        HPSlider.maxValue = MaxHP;
        HPSlider.value = CurHp;
    }

    

}
