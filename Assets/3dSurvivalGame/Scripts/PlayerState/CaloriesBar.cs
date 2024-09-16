using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class CaloriesBar : MonoBehaviour
    {
        private Slider slider;
        public Text caloriesCounter;

        public GameObject playerState;

        private float currentCalories, maxCalories;

        // Start is called before the first frame update
        void Awake()
        {
            slider = GetComponent<Slider>();

        }

        // Update is called once per frame
        void Update()
        {
            currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
            maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

            float fillValue = currentCalories / maxCalories;    // slider �� 0���� 1 ������ ������ �����Ǳ� ����
            slider.value = fillValue;

            caloriesCounter.text = currentCalories + "/" + maxCalories;
        }
    }
}
