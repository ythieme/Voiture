using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("Data")]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public AgentManager agentManager;
    public string path;

    XmlSerializer serializer = new XmlSerializer(typeof(Data));
    Encoding encoding = Encoding.UTF8;

    public void Awake()
    {
        instance = this;
        SetPath();
    }

    public void Save(Data data)
    {
        StreamWriter streamWriter = new StreamWriter(path, false, encoding);

        serializer.Serialize(streamWriter, data);
    }

    public Data Load()
    {
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);

            return serializer.Deserialize(fileStream) as Data;
        }

        return null;
    }

    public void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
    }
}