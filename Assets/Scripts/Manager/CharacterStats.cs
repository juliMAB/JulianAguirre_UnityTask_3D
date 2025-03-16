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
        if (hungry>0)
        {
            hungry -= Time.deltaTime * hungryDecay;
            visualSliderHungry.value = hungry;
        }
    }
    private void AddHealt(float q)
    {
        if (healt < 0 && q < 0)
        {
            return;
        }
        if (healt>1 && q>0)
        {
            return;
        }
        healt += q;
        visualSliderHealt.value = healt;
    }
    private void AddHealtHungry(float q)
    {
        if (hungry<0 && q<0)
        {
            return;
        }
        if (hungry > 1 && q > 0)
        {
            return;
        }
        
        hungry += q;
        visualSliderHungry.value = healt;
    }
    public void Reset()
    {
        hungry = 1f;
        healt = 0.5f;
    }
}
