using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Save(string key,int value)
    {
        PlayerPrefs.SetInt(key,value);
        PlayerPrefs.Save();
    }

    

    public void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key,0);
    }

    public float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, 0);
    }

    public string GetString(string key)
    {
        return PlayerPrefs.GetString(key,"NULL");
    }

    public bool KeyExists(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    public void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
