using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class HydrationBar : MonoBehaviour
    {
        private Slider slider;
        public Text hydrationCounter;

        public GameObject playerState;

        private float currentHydration, maxHydration;

        // Start is called before the first frame update
        void Awake()
        {
            slider = GetComponent<Slider>();

        }

        // Update is called once per frame
        void Update()
        {
            currentHydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
            maxHydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;

            float fillValue = currentHydration / maxHydration;    // slider �� 0���� 1 ������ ������ �����Ǳ� ����
            slider.value = fillValue;

            hydrationCounter.text = currentHydration + "%";
        }
    }
}
