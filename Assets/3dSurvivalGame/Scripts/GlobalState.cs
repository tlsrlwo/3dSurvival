using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class GlobalState : MonoBehaviour
    {
        public static GlobalState Instance { get; set; }

        [Header("Health")]
        public float resourceHealth;
        public float resourceMaxHealth;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
