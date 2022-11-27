using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerData
{
    public string Name { get; private set; } = "";
    public int SceneBuildIndex { get; set; } = 0;  // saved level
    public float PositionAxisX { get; set; } = 0f;
    public float PositionAxisY { get; set; } = 0f;
    public int CoinCount { get; set; } = 0;
    public Dictionary<string, List<string>> CollectedCoins { get; private set; } = null;
    public int ArmorLevel { get; set; } = 0;
    public int BlasterLevel { get; set; } = 0;
    public int JetpackLevel { get; set; } = 0;
    public int FlamethrowerLevel { get; set; } = 0;
    
    public PlayerData(string playerName)
    {
        Name = playerName;
        ResetData();
    }

    public void ResetData()
    {
        SceneBuildIndex = 0;
        PositionAxisX = 0;
        PositionAxisY = 0;
        CoinCount = 0;
        CollectedCoins = new Dictionary<string, List<string>>();
        ArmorLevel = 1;
        BlasterLevel = 1;
        JetpackLevel = 1;
        FlamethrowerLevel = 1;
    }
    
    public void SaveNewData()
    {
        if (DatabaseManagement.EntryExists("PlayerData", Name))
        {
            Debug.LogWarning("WARNING: <PlayerData> - entry already exists.");
            return;
        }

        DatabaseManagement.InsertEntry("PlayerData", Name, 
            new Dictionary<string, string>()
            {
                {"SceneBuildIndex", SceneBuildIndex.ToString()},
                {"PositionAxisX", PositionAxisX.ToString()},
                {"PositionAxisY", PositionAxisY.ToString()},
                {"CoinCount", CoinCount.ToString()},
                {"CollectedCoins", JsonConvert.SerializeObject(CollectedCoins, Formatting.Indented)},
                {"ArmorLevel", ArmorLevel.ToString()},
                {"BlasterLevel", BlasterLevel.ToString()},
                {"JetpackLevel", JetpackLevel.ToString()},
                {"FlamethrowerLevel", FlamethrowerLevel.ToString()}
            });
    }

    public void UpdateData()
    {
        if (!DatabaseManagement.EntryExists("PlayerData", Name))
        {
            Debug.LogWarning("WARNING: <PlayerData> - entry does not exist.");
            return;
        }
        
        DatabaseManagement.UpdateEntryValues("PlayerData", Name, 
            new Dictionary<string, string>()
            {
                {"SceneBuildIndex", SceneBuildIndex.ToString()},
                {"PositionAxisX", PositionAxisX.ToString()},
                {"PositionAxisY", PositionAxisY.ToString()},
                {"CoinCount", CoinCount.ToString()},
                {"CollectedCoins", JsonConvert.SerializeObject(CollectedCoins, Formatting.Indented)},
                {"ArmorLevel", ArmorLevel.ToString()},
                {"BlasterLevel", BlasterLevel.ToString()},
                {"JetpackLevel", JetpackLevel.ToString()},
                {"FlamethrowerLevel", FlamethrowerLevel.ToString()}
            });
    }

    public void DeleteData()
    {
        if (!DatabaseManagement.EntryExists("PlayerData", Name))
        {
            Debug.LogWarning("WARNING: <PlayerData> - entry does not exist.");
            return;
        }
        
        DatabaseManagement.DeleteEntry("PlayerData", Name);
    }
    
    public void LoadData()
    {
        if (!DatabaseManagement.EntryExists("PlayerData", Name))
        {
            Debug.LogWarning("WARNING: <PlayerData> - entry does not exist.");
            return;
        }

        Dictionary<string, string> records = DatabaseManagement.GetEntry("PlayerData", Name);
        if (records is null)
        {
            Debug.LogWarning("WARNING: <PlayerData> - loadedData is null.");
            return;
        }

        foreach (KeyValuePair<string, string> record in records)
        {
            // (Player) "Name" corresponds to "id" database 
            if (record.Key == "id")
            {
                continue;
            }
            
            PropertyInfo property = this.GetType().GetProperty(record.Key);
            if (property is null)
            {
                Debug.LogWarning("WARNING: <PlayerData> - property is null.");
                continue;
            }
            
            if (record.Key == "CollectedCoins")
            {
                var propertyValue = 
                    JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(record.Value);
                if (propertyValue is null)
                {
                    Debug.LogWarning("WARNING: <PlayerData> - propertyValue is null.");
                    continue;
                }
                
                property.SetValue(this, propertyValue);
            }
            else
            { 
                var propertyValue = Convert.ChangeType(record.Value, property.PropertyType);
                if (propertyValue is null)
                {
                    Debug.LogWarning("WARNING: <PlayerData> - propertyValue is null.");
                    continue;
                }
                
                property.SetValue(this, propertyValue);
            }
        }
    }
}
