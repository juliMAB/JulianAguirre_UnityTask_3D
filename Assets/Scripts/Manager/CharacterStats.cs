using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] Slider visualSliderHungry;
    [SerializeField] Slider visualSliderHealt;

    [SerializeField,Range(0,0.1f)] private float hungryDecay;

    private float hungry = 1f;
    private float healt = 0.5f;

    public void Initialize(out Action<float> addHealt,out Action<float> addHungry)
    {
        addHealt = AddHealt;
        addHungry = AddHealtHungry;
    }
    private void Update()
    {
        if (hungryDecay>0)
        {
            hungry -= Time.deltaTime * hungryDecay;
            visualSliderHungry.value = hungry;
        }
    }
    private void AddHealt(float q)
    {
        healt += q;
        visualSliderHealt.value = healt;
    }
    private void AddHealtHungry(float q)
    {
        hungry += q;
        visualSliderHungry.value = healt;
    }
}
