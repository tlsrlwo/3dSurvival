using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SUR
{
    public class MainMenuSaveManager : MonoBehaviour
    {
       public static MainMenuSaveManager Instance { get; set; }
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

        //--------------------------------------------------------------Volume(��� ����)
        #region("Volume.������")
        /*
        //--------------------------------------------------------------MusicVolume
        public void SaveMusicVolume(float volume)
        {
            // MusicVolume�̶�� float ���¸� volume�� ���� �޾ƿͼ� ����. 
            //PlayerPrefs �� ����Ƽ���� �����ϴ� ���� �� �ϳ�(���� ��ġ�� �����)
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }

        public float LoadMusicVolume()
        {
            // GetFloat �� ���
            return PlayerPrefs.GetFloat("MusicVolume");            
        }

        //--------------------------------------------------------------EffectsVolume

        public void SaveEffectsVolume(float volume)
        {
            // MusicVolume�̶�� float ���¸� volume�� ���� �޾ƿͼ� ����. 
            //PlayerPrefs �� ����Ƽ���� �����ϴ� ���� �� �ϳ�(���� ��ġ�� �����)
            PlayerPrefs.SetFloat("EffectsVolume", volume);
            PlayerPrefs.Save();
        }

        public float LoadEffectsVolume()
        {
            // GetFloat �� ���
            return PlayerPrefs.GetFloat("EffectsVolume");
        }

        //--------------------------------------------------------------
          */
        #endregion


        [System.Serializable]
        public class VolumeSettings
        {
            public float music;
            public float effects;
            public float masterVolume;
        }

        public void SaveVolumeSettings(float _music, float _effects, float _masterVol)
        {
            VolumeSettings volumeSettings = new VolumeSettings()
            {
                music = _music,
                effects = _effects,
                masterVolume = _masterVol
            };

            // SetString �� �ؼ� string���ĸ� ������ �� �ִµ� volumeSettings�� Ŭ������ Ŭ������ �����ϱ� ���� JsonUtility.ToJson() ���
            PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
            PlayerPrefs.Save();
        }

        // VolumeSettings�� ����
        public VolumeSettings LoadVolumeSettings()
        {
            //**Json���� string ���� �޾ƿͼ� class ������ ��ȯ���ִ°Ű���. Json���� VolumeSettings �� �����ͼ� �ε��ϱ� ����**
            //VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            //var settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));

            return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));            
        }

    }
}
