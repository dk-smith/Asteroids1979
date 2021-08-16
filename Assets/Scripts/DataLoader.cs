using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataLoader
{
    public static void SaveScore(int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Data.dat");
        SaveData data = new SaveData(score);
        bf.Serialize(file, data);
        file.Close();
    }

    public static int LoadScore()
    {
        var score = 0;
            
        if (File.Exists(Application.persistentDataPath + "/Data.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                File.Open(Application.persistentDataPath + "/Data.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            score = data.GetScore();
        }

        return score;
    }
        
}
    
[System.Serializable]
public class SaveData
{
    int score;

    public SaveData(int score)
    {
        this.score = score;
    }

    public int GetScore() { return score; }
}