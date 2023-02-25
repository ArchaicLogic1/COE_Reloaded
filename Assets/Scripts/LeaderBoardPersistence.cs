using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class LeaderBoardPersistence : MonoBehaviour
{
    public static LeaderBoardPersistence instance;
    [SerializeField] List<NameAndScore> leaderBoard = new List<NameAndScore>();
    [SerializeField] string filename;
   [SerializeField] List<TMPro.TMP_Text> leaderBoardNames = new List<TMPro.TMP_Text>();
   [SerializeField] List<TMPro.TMP_Text> LeaderBoardScores = new List<TMPro.TMP_Text>();


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //      if (instance == null)
        //   {
        //       instance = this;
        //
        //   }
        //   else
        //   {
        //       Destroy(gameObject);
        //   }
        LoadLeaderBoardFromSaveFile();
    }

 
    private void LoadLeaderBoardFromSaveFile()
    {
        leaderBoard = ReadFromJson(filename);
        leaderBoard.Sort((L1, L2) => L2.score.CompareTo(L1.score));
        Debug.Log(leaderBoard.Count);
       
        // prevents argument out of range
       int _count = leaderBoard.Count > leaderBoardNames.Count ? leaderBoardNames.Count : leaderBoard.Count; 
        for (int i = 0; i < _count; i++)
        {
           
            leaderBoardNames[i].text = leaderBoard[i].name;
            if(i<4)
            {

            LeaderBoardScores[i].text = leaderBoard[i].score.ToString();
            }
            else
            {
                LeaderBoardScores[i].text = "Last";

            }

        }
    }
    public void CreateNewNameandScore(int _score, string _name )
    {
        if (_name == "" || name == " ")
        {
            _name = "namelessPotato";
        }
        Debug.Log("create NEw name and score called");
        leaderBoard.Add( new NameAndScore(_name, _score));
        Debug.Log(leaderBoard[0].name + " " + leaderBoard[0].score);
        SaveToJSON<NameAndScore>(leaderBoard, filename);
    }


// save and load learderboard in Json 

    public  void SaveToJSON<T>(List<T> toSave, string filename)
    {
        Debug.Log(GetPath(filename));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(filename), content);
    }
    public List<NameAndScore> ReadFromJson(string filename)
    {
        string content = ReadFile(GetPath(filename));
        if(string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<NameAndScore>();
        }
        List<NameAndScore> res = JsonHelper.FromJson<NameAndScore>(content).ToList();
        return res;
    }
    private string GetPath(string filename)
    {
        return  Application.persistentDataPath + "/" + filename;
    }


    private static string ReadFile(string path)
    {
        if(File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        return "";
    }

    private void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream (path, FileMode.Create);
        using( StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }


}

[Serializable]
public class NameAndScore
{
    public string name;
    public int score;
    public NameAndScore(string _name, int _score)
    {
        name = _name;
        score = _score;
    }
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

