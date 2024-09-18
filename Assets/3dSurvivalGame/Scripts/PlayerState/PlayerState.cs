using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class PlayerState : MonoBehaviour
    {
        public static PlayerState Instance { get; set; }

        [Header("Player Health")]
        public float currentHealth;
        public float maxHealth;

        [Header("Player Calories")]
        public float currentCalories;
        public float maxCalories;

        float distanceTraveled = 0;
        Vector3 lastPosition;

        public GameObject playerBody;

        [Header("Player Hydration")]
        public float currentHydrationPercent;
        public float maxHydrationPercent;

        public bool isHydrationActive;

        public void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            currentCalories = maxCalories;
            currentHydrationPercent = maxHydrationPercent;

            StartCoroutine(DecreaseHydration());

        }
        public IEnumerator DecreaseHydration()
        {
            while (true)
            {
                currentHydrationPercent -= 1;
                yield return new WaitForSeconds(10f); 
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
            // reset the lastPos, now the lastPos is no longer the pos of the beginning
            lastPosition = playerBody.transform.position;

            // reset distanceTraveled
            if(distanceTraveled >= 5)
            {
                // 5만큼의 distanceToTravel을 움직이면 5마다 1칼로리 감소
                distanceTraveled = 0;
                currentCalories -= 1;
            }
        }

        public void setHealth(float newHealth)
        {
            currentHealth = newHealth;
        }
        public void setCalories(float newCalories)
        {
            currentCalories = newCalories;
        }
        public void setHydration(float newHydration)
        {
            currentHydrationPercent = newHydration;
        }

    }
}
