using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name = "player";
    public int goldCount = 100;
    public float maxHp = 500;
    public float damageBase = 30;
}

public class DataManager : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private string keyName = "PlayerData";
    private string mainFile = "SaveData.txt";

    public PlayerData Data => data;

    public void DataSave()
    {
        ES3.Save(keyName, data, mainFile);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(mainFile))
        {
            try
            {
                // Load dữ liệu từ file vào data
                data = ES3.Load<PlayerData>(keyName, mainFile);
                Debug.Log("Data loaded successfully!");
                return;
            }
            catch
            {
                Debug.LogWarning("Main save file corrupted, creating new save...");
            }
        }
        else
        {
            Debug.Log("Save file not found. Creating new save...");
        }

        // Nếu file không tồn tại hoặc bị hỏng, tạo mới dữ liệu mặc định
        data = new PlayerData(); // khởi tạo dữ liệu mặc định
        DataSave(); // lưu lại file
    }

    public void DeleteAllData()
    {
        if (ES3.FileExists(mainFile)) ES3.DeleteFile(mainFile);
    }
}
