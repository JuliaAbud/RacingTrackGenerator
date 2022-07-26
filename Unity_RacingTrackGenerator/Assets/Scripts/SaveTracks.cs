using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class SaveTracks : MonoBehaviour
{
    private StreamWriter _writer;
    private string _filePath;
    [SerializeField] private string _creator = "ML-projectJulia";
    [SerializeField] private string _fileName = "Saved_tracks";
    private Vector3[] _points; 
    private int _pointsNum = 20;

    public Vector3[] Points
    {
        get => _points;
        set => _points = value;
    }

    void Start()
    {
        //createCSV
        _filePath = Application.dataPath + "/CSV_savedTracks/" + _fileName +".csv";
        _writer = new StreamWriter(_filePath);
        CreateCSV();
    }

    void CreateCSV()
    {
        string first3lines = "Creation date,Creator,Number of points";
        string pointsNames = "";
        for (int i = 0; i < _pointsNum; i++)
        {
            pointsNames += ",P" + i + "_x";
            pointsNames += ",P" + i + "_y";
            pointsNames += ",P" + i + "_z";
        }
        _writer.WriteLine(first3lines + pointsNames);
    }

    public void SaveTrack()
    {
        string date = System.DateTime.Now.ToString("yyyy/MM/dd");//hh:mm:ss
        //SAVE TRACK
        string pointsString = "";
        for (int i = 0; i < 20; i++)
        {
            if (i < _points.Length)
            {
                pointsString += "," + _points[i].x.ToString();
                pointsString += "," + _points[i].y.ToString();
                pointsString += "," + _points[i].z.ToString();
            }
            else
            {
                pointsString += ",0,0,0";
            }
        }
        _writer.WriteLine(date + "," + _creator + "," + _points.Length.ToString() + pointsString);
    }

    void OnApplicationQuit()
    {
        _writer.Flush();
        _writer.Close();
    }

}
