using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using JetBrains.Annotations;
using UnityEngine;

public static class DatabaseManagement
{
    private static IDbConnection DbConnection { get; set; }

    public static void InitialiseConnection(string databasePath)
    {
        DbConnection = new SqliteConnection("URI=file:" + databasePath);
        DbConnection.Open();
    }

    public static void CloseConnection()
    {
        if (DbConnection is null)
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - no connection is open.");
            return;
        }
        DbConnection.Close();
    }

    public static void CreateTable(DatabaseTable databaseTable)
    {
        string dbCommandText =
            $"CREATE TABLE IF NOT EXISTS {databaseTable.Name} " +
            $"(id TEXT PRIMARY KEY";
        foreach (string databaseTableField in databaseTable.Fields)
        {
            dbCommandText += $", {databaseTableField} TEXT";
        }
        dbCommandText += ")";

        IDbCommand dbCommand = DbConnection.CreateCommand();
        dbCommand.CommandText = dbCommandText;
        dbCommand.ExecuteReader();
    }

    public static void InsertEntry(string databaseTableName, string id, Dictionary<string, string> records)
    {
        if (EntryExists(databaseTableName, id))
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - entry already exists.");
            return;
        }

        string dbCommandText =
            $"INSERT INTO {databaseTableName} " +
            $"(id";
        foreach (string key in records.Keys)
        {
            dbCommandText += $", {key}";
        }
        dbCommandText += $") VALUES ('{id}'";
        foreach (string value in records.Values)
        {
            dbCommandText += $", '{value}'";
        }
        dbCommandText += $")";
        
        IDbCommand dbCommand = DbConnection.CreateCommand();
        dbCommand.CommandText = dbCommandText;
        dbCommand.ExecuteNonQuery();
    }
    
    public static void UpdateEntryValues(string databaseTableName, string id, Dictionary<string, string> records)
    {
        if (!EntryExists(databaseTableName, id))
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - entry does not exist.");
            return;
        }

        foreach (KeyValuePair<string, string> record in records)
        {
            UpdateEntryValue(databaseTableName, id, record.Key, record.Value);
        }
    }
    
    public static void UpdateEntryValue(string databaseTableName, string id, string field, string value)
    {
        if (!EntryExists(databaseTableName, id))
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - entry does not exist.");
            return;
        }
        
        string dbCommandText =
            $"UPDATE {databaseTableName} " +
            $"SET {field}='{value}' " +
            $"WHERE id='{id}'";
        
        IDbCommand dbCommand = DbConnection.CreateCommand();
        dbCommand.CommandText = dbCommandText;
        dbCommand.ExecuteNonQuery();
    }
    
    public static void DeleteEntry(string databaseTableName, string id)
    {
        if (!EntryExists(databaseTableName, id))
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - entry does not exist.");
            return;
        }
        
        string dbCommandText =
            $"DELETE FROM {databaseTableName} " +
            $"WHERE id='{id}'";
        
        IDbCommand dbCommand = DbConnection.CreateCommand();
        dbCommand.CommandText = dbCommandText;
        dbCommand.ExecuteNonQuery();
    }

    [CanBeNull]
    public static Dictionary<string, string> GetEntry(string databaseTableName, string id)
    {
        if (!EntryExists(databaseTableName, id))
        {
            Debug.LogWarning("WARNING: <DatabaseManagement> - entry does not exist.");
            return null;
        }
        
        Dictionary<string, string> entryData = new Dictionary<string, string>();
        
        string dbCommandText = 
            $"SELECT * " +
            $"FROM {databaseTableName} " +
            $"WHERE id='{id}'";

        IDbCommand dbCommand = DbConnection.CreateCommand();
        IDataReader dataReader;
        dbCommand.CommandText = dbCommandText;
        dataReader = dbCommand.ExecuteReader();

        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            entryData.Add(dataReader.GetName(i), dataReader[i].ToString());
        }

        return entryData;
    }
    
    public static List<Dictionary<string, string>> GetEntries(string databaseTableName)
    {
        List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();

        string dbCommandText = 
            $"SELECT * " +
            $"FROM {databaseTableName} ";

        IDbCommand dbCommand = DbConnection.CreateCommand();
        IDataReader dataReader;
        dbCommand.CommandText = dbCommandText;
        dataReader = dbCommand.ExecuteReader();
        
        while(dataReader.Read()) 
        {
            Dictionary<string, string> entryData = new Dictionary<string, string>();
            
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                entryData.Add(dataReader.GetName(i), dataReader[i].ToString());
            }
            
            entries.Add(entryData);
        }

        return entries;
    }

    public static bool EntryExists(string databaseTableName, string id)
    {
        IDbCommand dbCommand = DbConnection.CreateCommand();
        IDataReader dataReader;
        string dbCommandText = 
            $"SELECT EXISTS" +
            $"(SELECT 1 " +
            $"FROM {databaseTableName} " +
            $"WHERE id='{id}' " +
            $"LIMIT 1)";

        dbCommand.CommandText = dbCommandText;
        dataReader = dbCommand.ExecuteReader();

        if (dataReader[0].ToString() == "0")
        {
            return false;
        }
        return true;
    }
}

public class DatabaseTable
{
    public string Name { get; set; }
    public List<string> Fields { get; set; }

    public DatabaseTable(string name, List<string> fields)
    {
        Name = name;
        Fields = fields;
    }
}
