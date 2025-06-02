using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ISavable
{
    public PlayerMovementData Data { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerAnimation Anim { get; private set; }
    public PlayerAction PlayerAction { get; private set; }
    public PlayerStat PlayerStat { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }


    void Awake()
    {
        Data = GetComponent<PlayerMovementData>();
        Input = GetComponent<PlayerInput>();
        Anim = GetComponent<PlayerAnimation>();
        PlayerAction = GetComponent<PlayerAction>();
        PlayerStat = GetComponent<PlayerStat>();
        PlayerHealth = GetComponent<PlayerHealth>();

        Init();
    }

    void OnEnable()
    {
        RegisterSave();
    }
    void OnDisable()
    {
        DeregisterSave();
    }

    void Init()
    {
        Anim.Init(this);
        PlayerAction.Init(this, Data, PlayerStat, Anim);
        PlayerHealth.Init(PlayerStat, Anim);
    }

    #region Save

    public void RegisterSave()
    {
        SaveLoadManager.Instance.Register(this);
    }
    public void DeregisterSave()
    {
        SaveLoadManager.Instance.Deregister(this);
    }
    public object CaptureData()
    {
        PlayerSaveData data = new PlayerSaveData();
        data.currentScene = SceneManager.GetActiveScene().name;
        data.posX = transform.position.x;
        data.posY = transform.position.y;

        /* Stat */

        data.level = PlayerStat.Level;
        data.currentHp = PlayerStat.CurrentHp;
        data.currentExp = PlayerStat.CurrentExp;
        data.currentSoul = PlayerStat.CurrentSoul;
        data.currentShield = PlayerStat.CurrentShield;

        return data;
    }

    public  void RestoreData(object loadedData)
    {
        PlayerSaveData data = loadedData as PlayerSaveData;

        if (data != null)
        {
            transform.position = new Vector2(data.posX, data.posY);

            PlayerStat.Level = data.level;
            PlayerStat.CurrentHp = data.currentHp;
            PlayerStat.CurrentExp = data.currentExp;
            PlayerStat.CurrentSoul = data.currentSoul;
            PlayerStat.CurrentShield = data.currentShield;
        }
    }
    #endregion
}
