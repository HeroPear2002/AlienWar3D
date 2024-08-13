using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
[System.Serializable]
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject pSetting;
    private bool isSetting = true;
    [SerializeField] private SaveGamePlayer saveGamePlayer;
    [SerializeField] private BinaryFormatter formatter;
    private FileStream file;
    private PlayerMoverment pm;
    //-------------------------------
    [SerializeField] private GameObject pMenu;
    [SerializeField] private GameObject pSettingInGame;
    [SerializeField] private GameObject CStore;
    [SerializeField] private GameObject pGameOver;
    private bool isSettingInGame = true;
    private bool bstore = false;
    private static bool isContinue;
    private HeadController headController;  
    private GunsMenu gunsMenu;
    private List<string> destroyedObjects = new List<string>();
    private Dictionary<string, int> Gates = new Dictionary<string, int>();
    public Dictionary<string, Vector3> MonsterP = new Dictionary<string, Vector3>();
    private PlayerAudio pa;
    private void Start()
    {
        pa = FindObjectOfType<PlayerAudio>();
        gunsMenu = FindObjectOfType<GunsMenu>();
        pm = FindObjectOfType<PlayerMoverment>();
        headController = FindObjectOfType<HeadController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        formatter = new BinaryFormatter();
    }
    void Update()
    { 
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            TogglePauseGame();
        }
        
        if (isContinue)
        {
            file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            saveGamePlayer = (SaveGamePlayer)formatter.Deserialize(file);
            file.Close();
            pm.SetTranformPlayer(saveGamePlayer.playerPosition.ToVector3());
            headController.Killcoin = saveGamePlayer.score;
            headController.Head = saveGamePlayer.head;
            gunsMenu.BuyGuns = saveGamePlayer.buyGuns;
            pm.gunDictionary = saveGamePlayer.gundictionary;
            List<string> destroyedObjects = saveGamePlayer.destroyedObjects;
            ProcessDestroyedObjects(destroyedObjects);
            UpdateGates(Gates);
            /*Dictionary<string, SerializableVector3> monsterp = saveGamePlayer.monster;
            UpdateMonster(monsterp);*/
            isContinue = false;
        }

    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        isContinue = true;
        SceneManager.LoadScene(1);
    }
    public void Setting()
    {
        if(!isSetting)
        {
            pSetting.SetActive(isSetting);
            isSetting = true;
        }
        else
        {
            pSetting.SetActive(isSetting);
            isSetting = false;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Donttrigger()
    {
        if(pSetting != null)
        {
            pSetting.SetActive(false);
            isSetting = true;
        }
    }
    public void PlayinGame()
    {
        TogglePauseGame();
    }
    public void SettinginGame()
    {
        if (isSettingInGame)
        {
            pMenu.SetActive(!isSettingInGame);
            pSettingInGame.SetActive(isSettingInGame);
            isSettingInGame = false;
        }
        else
        {
            pSettingInGame.SetActive(isSettingInGame);
            pMenu.SetActive(!isSettingInGame);
            isSettingInGame = true;
        }
    }
    public void ExitinGame()
    {
        Vector3 playerPosition = pm.GetTranform();
        float score = headController.Killcoin;
        float heath = headController.Head;
        Dictionary<int, bool> buyGuns = gunsMenu.BuyGuns;
        Dictionary<int, Gun> gundictionary = pm.gunDictionary;
        List<string> destroyedObject = GetDestroyedObjects();
        Dictionary<string, int> gateOnMonster = GetGates();
        /*Dictionary<string, SerializableVector3> Monsterp = GetMonster();*/
        saveGamePlayer = new SaveGamePlayer(new SerializableVector3(playerPosition), score, heath, buyGuns, gundictionary, destroyedObject, gateOnMonster/*, Monsterp*/);
        file = File.Create(Application.persistentDataPath + "/save.dat");
        formatter.Serialize(file, saveGamePlayer);
        file.Close();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ExitStore()
    {

        CStore.SetActive(false);
        TogglePauseGame();
    }
    public void TogglePauseGame()
    {
        // Kiểm tra xem trò chơi đang dừng hay không
        if (Time.timeScale == 0 || bstore)
        {
            // Nếu đang dừng, tiếp tục trò chơi
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pMenu.SetActive(false);
            CStore.SetActive(false);
            bstore = false;

        }
        else
        {
            // Nếu không, dừng trò chơi
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; // Cho phép con trỏ chuột tự do di chuyển
            Cursor.visible = true; // Hiển thị con trỏ chuột trở lại
            pMenu.SetActive(true);
        }
    }
    public void OpenStore()
    {
        if (Time.timeScale != 0 && !bstore)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CStore.SetActive(true);
            bstore = true;
        }
    }
    public void PlayAgain()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        pGameOver.SetActive(false);
        
        file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        // Ghi mảng byte rỗng vào tệp tin để xóa trắng nội dung
        byte[] emptyBytes = new byte[0];
        file.Write(emptyBytes, 0, emptyBytes.Length);

        // Đóng tệp tin
        file.Close();
        SceneManager.LoadScene(1);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pGameOver.SetActive(true);
    }
    public void ExitGameOver()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        // Ghi mảng byte rỗng vào tệp tin để xóa trắng nội dung
        byte[] emptyBytes = new byte[0];
        file.Write(emptyBytes, 0, emptyBytes.Length);

        // Đóng tệp tin
        file.Close();
        SceneManager.LoadScene(0);
    }
    
    
    public void OnObjectDestroyed(GameObject obj)
    {
        destroyedObjects.Add(obj.name);
        Debug.Log("goi vao day" + obj);
    }

    public List<string> GetDestroyedObjects()
    {
        return destroyedObjects;
    }
    private void ProcessDestroyedObjects(List<string> destroyedObjectNames)
    {
        foreach (string objname in destroyedObjectNames)
        {
            GameObject obj = GameObject.Find(objname);
            if (obj != null)
            {
                Destroy(obj);
            }
            else
            {
                Debug.Log("Đối tượng " + objname + " không tìm thấy hoặc đã bị hủy.");
            }
        }
    }
    public void GateOnMonster(GameObject obj, int monstercount)
    {
        if (Gates.ContainsKey(obj.name))
        {
            Gates[obj.name] = monstercount;
        }
        else
        {
            Gates.Add(obj.name, monstercount);
        }
    }
    public Dictionary<string, int> GetGates()
    {
        return Gates;
    }
    public void UpdateGates(Dictionary<string, int> gates)
    {
        var keys = new List<string>(gates.Keys);
        var values = new List<int>(gates.Values);
        for (int i = 0; i < keys.Count; i++)
        {
            GameObject obj = GameObject.Find(keys[i]);
            GateController gate = obj.GetComponent<GateController>();
            if (obj != null)
            {
                gate.monsterCount = values[i];
            }
            else
            {
                Debug.Log("Đối tượng " + keys[i] + " không tìm thấy hoặc đã bị hủy.");
            }
        }
    }
    /*public void MonsterOn(string namemonster, Vector3 po)
    {
        if (MonsterP.ContainsKey(namemonster))
        {
            MonsterP[namemonster] = po;
        }
        else
        {
            MonsterP.Add(namemonster, po);
        }
        Debug.Log(namemonster);
    }
    public Dictionary<string, SerializableVector3> GetMonster()
    {
        Dictionary<string, SerializableVector3> monsterpp = new Dictionary<string, SerializableVector3>();
        var keys = new List<string>(MonsterP.Keys);
        var values = new List<Vector3>(MonsterP.Values);
        for (int i = 0; i < keys.Count; i++)
        {
            monsterpp.Add(keys[i], new SerializableVector3(values[i]));
        }
        return monsterpp;
    }
    public void UpdateMonster(Dictionary<string, SerializableVector3> monsterp)
    {
        Quaternion ro = Quaternion.identity;
        var keys = new List<string>(monsterp.Keys);
        var values = new List<SerializableVector3>(monsterp.Values);
        for (int i = 0; i < keys.Count; i++)
        {

            GameObject prefab = Resources.Load<GameObject>("Asset/Game/Prefab/" + keys[i]);
            GameObject monster = Instantiate(prefab, values[i].ToVector3(), ro);
        }
    }*/
    public void CallFun()
    {
        pa.PlayButtonClickSound();
    }
}
