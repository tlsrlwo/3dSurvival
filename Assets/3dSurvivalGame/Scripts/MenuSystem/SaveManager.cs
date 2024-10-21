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

        // Json Project Save Path
        string jsonPathProject;

        // Json external/real save path (���带 �ϸ� ��������� ����� ������ ��ġ) (persistant = ���Ӽ�)
        string jsonPathPersistant;

        // Binary save path
        string binaryPath;

        // Json���� �������� binary�� �������� �ν�����â���� ����
        public bool isSavingToJson;
                
        string fileName = "SaveGame"; // binary�̵� json �̵� �������� �⺻������ �������� �����̸�




        private void Start()
        {
            jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
            jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
            binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;     // �̸� start�� ������ �س��� �ڵ��� ȿ���� ����
        }



        //--------------------------------------------------------------Saving
        #region Saving

        public void SaveGame(int slotNumber)
        {
            AllGameData data = new AllGameData();

            data.playerData = GetPlayerData();

            SavingTypeSwitch(data, slotNumber);
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

        public void SavingTypeSwitch(AllGameData gamedata, int slotNumber)
        {
            if (isSavingToJson)
            {
                SaveGameDataToJsonFile(gamedata, slotNumber);

            }
            else
            {
                SaveGameDataToBinaryFile(gamedata, slotNumber);
            }
        }
        #endregion Saving

        //--------------------------------------------------------------Loading
        #region Loading

        public AllGameData LoadingTypeSwitch(int slotNumber)
        {
            if (isSavingToJson)
            {
                AllGameData gamedata = LoadGameDataFromJsonFile(slotNumber);    // change it to Json;
                return gamedata;
            }
            else
            {
                AllGameData gamedata = LoadGameDataFromBinaryFile(slotNumber);
                return gamedata;
            }
        }

        public void LoadGame(int slotNumber)
        {
            // PlayerData
            SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);

            // EnvironmentData


        }

        private void SetPlayerData(PlayerData playerData)
        {
            // Set Player State
            PlayerState.Instance.currentHealth = playerData.playerStats[0];
            PlayerState.Instance.currentCalories = playerData.playerStats[1];
            PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

            // Set Player Position
            Vector3 loadedPosition;
            loadedPosition.x = playerData.playerPosAndRotation[0];
            loadedPosition.y = playerData.playerPosAndRotation[1];
            loadedPosition.z = playerData.playerPosAndRotation[2];

            PlayerState.Instance.playerBody.transform.position = loadedPosition;

            // Set Player Rotation (Vector3���� loadedRotation �� Quaternion�������� ��ȯ�ϱ� ���� Quaternion.Euler ���
            Vector3 loadedRotation;
            loadedRotation.x = playerData.playerPosAndRotation[3];
            loadedRotation.y = playerData.playerPosAndRotation[4];
            loadedRotation.z = playerData.playerPosAndRotation[5];

            PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
        }


        public void LoadGameWhenGameStarts(int slotNumber)
        {
            SceneManager.LoadScene("GameScene");

            // ���� ���۰� ���ÿ� ��ũ��Ʈ��� ������ �غ�Ǳ���� �ð�(���־ణ)�� �ʿ��ؼ� coroutine�� �ɾ���
            StartCoroutine(DelayedLoading(slotNumber));
        }

        private IEnumerator DelayedLoading(int slotNumber)
        {
            yield return new WaitForSeconds(1f);

            LoadGame(slotNumber);
        }


        #endregion

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
          */// ���� �⺻ ���� ( ��� �� �� ) 

        //--------------------------------------------------------------Binary
        #region BinaryGameData

        public void SaveGameDataToBinaryFile(AllGameData gamedata, int slotNumber)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            //string path = Application.persistentDataPath + "/save_game.bin"; binaryPath
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

            formatter.Serialize(stream, gamedata);
            stream.Close();

            //print("Data saved to" + Application.persistentDataPath + "/save_game.bin");  
            print("Data saved to" + binaryPath + fileName + slotNumber + ".bin");
        }

        public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
        {
            //string path = Application.persistentDataPath + "/save_game.bin";
            if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

                AllGameData data = formatter.Deserialize(stream) as AllGameData;
                stream.Close();

                //print("Data loaded from" + Application.persistentDataPath + "/save_game.bin");
                print("Data loaded from" + binaryPath + fileName + slotNumber + ".bin");

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

        public void SaveGameDataToJsonFile(AllGameData gamedata, int slotNumber)
        {
            // gamedata��ü�� string������ json���Ϸ� ����
            string json = JsonUtility.ToJson(gamedata);

            string encrypted = EncryptionDecryption(json);

            // StreamWriter�� ���Ϸ� ���� ��� jsonPathProject�� ��ġ�� 
            using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
            {
                //writer.Write(json);
                writer.Write(encrypted);
                print("Saved game to json file at :" + jsonPathProject + fileName + slotNumber + ".json");
            };
        }

        public AllGameData LoadGameDataFromJsonFile(int slotNumber)
        {
            using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
            {
                // jsonPathProject���� ������ �а� string���·� ������
                string json = reader.ReadToEnd();

                string decrypted = EncryptionDecryption(json);

                // string���� ������ ������ ������Ʈ(AllGameData)�� ��ȯ����
                AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
                print("Saved game loaded from json file at :" + jsonPathProject);

                return data;
            };
        }

        #endregion JsonGameData

        //--------------------------------------------------------------Settings(Volume)
        #region Settings

        #region Volume�κ�
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

            print("Saved to PlayerPref");
        }

        // VolumeSettings�� ����
        public VolumeSettings LoadVolumeSettings()
        {
            //**Json���� string ���� �޾ƿͼ� class ������ ��ȯ���ִ°Ű���. Json���� VolumeSettings �� �����ͼ� �ε��ϱ� ����**
            //VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            //var settings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));

            return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        }

        /*public float LoadMusicVolume()
        {
            var volumeSettings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
            return volumeSettings.music;

        }*/

        #endregion Volume�κ�


        #endregion Settings

        //--------------------------------------------------------------Encryption
        #region Encryption

        public string EncryptionDecryption(string jsonString)
        {
            // ��й�ȣ?
            string keyword = "1234567";
            string result = "";

            //�� jsonString�� ���� �ϳ��ϳ��� ������ �ɾ result �� ä������. ���� result �� "" �̶� �����ε� �װ� jsonString�� ���ڷ� ä���ִ� ���� ����
            for(int i = 0; i < jsonString.Length; i++)
            {
                result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
            }
            return result;
        }





        #endregion


        //--------------------------------------------------------------Utility
        #region Utility
        public bool DoesFileExists(int slotNumber)
        {
            if(isSavingToJson)
            {
                //json ������ ������ġ�� ������ �����ϴ���
                if(System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))        // ��ġ + savegame1.json
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //binary ������ ������ġ�� ������ �����ϴ���
                if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))             // ��ġ + savegame1.bin
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsSlotEmpty(int slotNumber)
        {
            if (DoesFileExists(slotNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DeselectButton()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        }

        #endregion

    }
}
