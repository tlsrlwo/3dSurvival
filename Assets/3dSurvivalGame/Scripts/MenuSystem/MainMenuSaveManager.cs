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

        //--------------------------------------------------------------Volume(사용 안함)
        #region("Volume.사용안함")
        /*
        //--------------------------------------------------------------MusicVolume
        public void SaveMusicVolume(float volume)
        {
            // MusicVolume이라는 float 형태를 volume의 값을 받아와서 저장. 
            //PlayerPrefs 는 유니티에서 저장하는 형식 중 하나(값의 수치만 저장됨)
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }

        public float LoadMusicVolume()
        {
            // GetFloat 를 사용
            return PlayerPrefs.GetFloat("MusicVolume");            
        }

        //--------------------------------------------------------------EffectsVolume

        public void SaveEffectsVolume(float volume)
        {
            // MusicVolume이라는 float 형태를 volume의 값을 받아와서 저장. 
            //PlayerPrefs 는 유니티에서 저장하는 형식 중 하나(값의 수치만 저장됨)
            PlayerPrefs.SetFloat("EffectsVolume", volume);
            PlayerPrefs.Save();
        }

        public float LoadEffectsVolume()
        {
            // GetFloat 를 사용
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

            // SetString 을 해서 string형식만 저장할 수 있는데 volumeSettings는 클래스라서 클래스를 저장하기 위해 JsonUtility.ToJson() 사용
            PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
            PlayerPrefs.Save();
        }

        // VolumeSettings를 리턴
        public VolumeSettings LoadVolumeSettings()
        {
            //**Json에서 string 값을 받아와서 class 값으로 변환해주는거같음. Json에서 VolumeSettings 를 가져와서 로드하기 위함**
            //VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            //var settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));

            return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));            
        }

    }
}
