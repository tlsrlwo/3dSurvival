using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace SUR
{
    public class SaveManager : MonoBehaviour
    {
       public static SaveManager Instance { get; set; }
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

            DontDestroyOnLoad(gameObject);
        }

        // Json으로 저장할지 binary로 저장할지 인스펙터창에서 조절
        public bool isSavingToJson;

        //--------------------------------------------------------------Saving
        #region Saving

        public void SaveGame()
        {
            AllGameData data = new AllGameData();

            data.playerData = GetPlayerData();

            SavingTypeSwitch(data);
        }

        private PlayerData GetPlayerData()
        {
            float[] playerStats = new float[3];

            playerStats[0] = PlayerState.Instance.currentHealth;
            playerStats[1] = PlayerState.Instance.currentCalories;
            playerStats[2] = PlayerState.Instance.currentHydrationPercent;

            float[] playerPosAndRot = new float[6];

            playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
            playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
            playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

            playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
            playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
            playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

            return new PlayerData(playerStats, playerPosAndRot);
        }

        public void SavingTypeSwitch(AllGameData gamedata)
        {
            if(isSavingToJson)
            {
                //SaveGameDataToJsonFile(gamedata);

            }
            else
            {
                SaveGameDataToBinaryFile(gamedata);
            }
        }
        #endregion Saving


        //--------------------------------------------------------------Loading
        #region Loading

        public AllGameData LoadingTypeSwitch()
        {
            if (isSavingToJson)
            {
                AllGameData gamedata = LoadGameDataToBinaryFile();    // change it to Json;
                return gamedata;
            }
            else
            {
                AllGameData gamedata = LoadGameDataToBinaryFile();
                return gamedata;
            }
        }

        public void LoadGame()
        {
            // PlayerData
            SetPlayerData(LoadingTypeSwitch().playerData);

            // EnvironmentData
           

        }

        private void SetPlayerData(PlayerData playerData)
        {
            // Set Player State
            PlayerState.Instance.currentHealth = playerData.playerStats[0];
            PlayerState.Instance.currentCalories = playerData.playerStats[1];
            PlayerState.Instance.currentHydrationPercent= playerData.playerStats[2];

            // Set Player Position
            Vector3 loadedPosition;
            loadedPosition.x = playerData.playerPosAndRotation[0];
            loadedPosition.y = playerData.playerPosAndRotation[1];
            loadedPosition.z = playerData.playerPosAndRotation[2];

            PlayerState.Instance.playerBody.transform.position = loadedPosition;

            // Set Player Rotation (Vector3형식 loadedRotation 을 Quaternion형식으로 변환하기 위해 Quaternion.Euler 사용
            Vector3 loadedRotation;
            loadedRotation.x = playerData.playerPosAndRotation[3];
            loadedRotation.y = playerData.playerPosAndRotation[4];
            loadedRotation.z = playerData.playerPosAndRotation[5];

            PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
        }


        public void LoadGameWhenGameStarts()
        {
            SceneManager.LoadScene("GameScene");

            // 게임 시작과 동시에 스크립트들과 정보가 준비되기까지 시간(아주약간)이 필요해서 coroutine을 걸어줌
            StartCoroutine(DelayedLoading());
        }

        private IEnumerator DelayedLoading()
        {


            yield return new WaitForSeconds(1f);

            LoadGame();            
        }


        #endregion

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
          */// 저장 기본 로직 ( 사용 안 함 ) 

        //--------------------------------------------------------------Binary
        #region BinaryGameData

        public void SaveGameDataToBinaryFile(AllGameData gamedata)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/save_game.bin";
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, gamedata);
            stream.Close();

            print("Data saved to" + Application.persistentDataPath + "/save_game.bin");  
        }

        public AllGameData LoadGameDataToBinaryFile()
        {
            string path = Application.persistentDataPath + "/save_game.bin";
            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                AllGameData data = formatter.Deserialize(stream) as AllGameData;
                stream.Close();


                print("Data loaded from" + Application.persistentDataPath + "/save_game.bin");

                return data;
            }
            else
            {
                return null;
            } 
        }

        #endregion BinaryGameData

        //--------------------------------------------------------------Json
        #region JsonGameData

        public void SaveGameDataToJsonFile(AllGameData gamedata)
        {



        }

        public AllGameData LoadGameDataToJsonFile()
        {



            return null;
        }

        #endregion JsonGameData

        //--------------------------------------------------------------Settings(Volume)
        #region Settings

        #region Volume부분
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

            print("Saved to PlayerPref");
        }

        // VolumeSettings를 리턴
        public VolumeSettings LoadVolumeSettings()
        {
            //**Json에서 string 값을 받아와서 class 값으로 변환해주는거같음. Json에서 VolumeSettings 를 가져와서 로드하기 위함**
            //VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            //var settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));

            return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));            
        }

        /*public float LoadMusicVolume()
        {
            var volumeSettings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            return volumeSettings.music;

        }*/

    }
    #endregion Volume부분


    #endregion Settings





}
