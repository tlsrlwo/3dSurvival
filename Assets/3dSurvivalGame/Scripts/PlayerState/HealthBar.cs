using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;
        public Text healthCounter;

        public GameObject playerState;

        private float currentHealth, maxHealth;

        // Start is called before the first frame update
        void Awake()
        {
            slider = GetComponent<Slider>();
        
        }

        // Update is called once per frame
        void Update()
        {
            currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
            maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

            float fillValue = currentHealth / maxHealth;    // slider �� 0���� 1 ������ ������ �����Ǳ� ����
            slider.value = fillValue;

            healthCounter.text = currentHealth + "/" +  maxHealth;
        }
    }
}
