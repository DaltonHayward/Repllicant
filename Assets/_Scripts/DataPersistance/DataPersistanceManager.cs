using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistance Manager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        // this decides where the data is stored, link to explanation of where this saves: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        // fetch all dataPersistanceObjects, then load the game with the data
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize a new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing to default values.");
            NewGame();
        }
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(ref gameData);
        }

        // save the data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        // find all the IDataPersistance objects
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        // return them in a list
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
