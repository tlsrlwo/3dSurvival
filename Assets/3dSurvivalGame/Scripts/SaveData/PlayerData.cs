using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [System.Serializable]
    public class PlayerData 
    {
        public float[] playerStats;  // [0] - health, [1] - calories, [2] - hydration
        public float[] playerPosAndRotation;  // [0] - transform.x, [1] - transform.y ....
        public string[] inventoryContent;
        public string[] quickSlotsContent;

        public PlayerData(float[] _playerStats, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotsContent)
        {
            playerStats = _playerStats;
            playerPosAndRotation = _playerPosAndRot;
            inventoryContent = _inventoryContent;
            quickSlotsContent = _quickSlotsContent;
        }
    }

    
}
