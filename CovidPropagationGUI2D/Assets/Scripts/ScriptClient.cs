using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Text;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ScriptClient : MonoBehaviour
{
    //public GameObject ChangingText;
    public NamedPipeClientStream pipeClient;
    public StreamString ss;
    public Text dataReceived;
    public Text dataReceived1;
    public Text dataReceived2;
    public Text dataReceived3;

    DataPopulation populationDatas;
    DataSites sitesDatas;
    string resultIteration;

    string resultPopulation;
    string resultSites;
    DataIteration iterationDatas;

    private ManagerScript mngScript;
    //public List<int> testTransfer;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Pipe Opening Process Started");
        pipeClient = new NamedPipeClientStream(".", "SimulationToUnity", PipeDirection.In, PipeOptions.Asynchronous);

        Debug.Log("Connecting to server...\n");

        ConnectToServer();

        mngScript = GetComponent<ManagerScript>();
        //string result = ss.ReadString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pipeClient.IsConnected)
        {
            ConnectToServer();
        }
    }
    private void ConnectToServer()
    {
        pipeClient.Connect();
        if (pipeClient.IsConnected)
        {
            ss = new StreamString(pipeClient);
            Thread.Sleep(250);
            ReadPipeData();
        }
    }
    int read = 0;
    private async void ReadPipeData()
    {

        string result = await ss.ReadStringAsync();
        string resultDataType = result.Split(' ')[0];

        switch (resultDataType)
        {
            default:
                dataReceived1.text = "A " + read++.ToString();
                dataReceived2.text = resultDataType + " e " + result;
                break;
            case "Initialize":
                resultPopulation = result.Split(' ')[1];
                resultSites = result.Split(' ')[2];

                populationDatas = JsonUtility.FromJson<DataPopulation>(resultPopulation);
                sitesDatas = JsonUtility.FromJson<DataSites>(resultSites);

                mngScript.CreateSites(sitesDatas);
                mngScript.CreatePopulation(populationDatas.NbPersons, populationDatas.IndexOfInfected);
                dataReceived.text = "Initialized " + read.ToString();
                break;
            case "Iterate":
                dataReceived1.text = "A " + read++.ToString();
                dataReceived2.text = resultDataType + " " + result;
                resultIteration = result.Split(' ')[1];
                iterationDatas = JsonUtility.FromJson<DataIteration>(resultIteration);
                
                dataReceived3.text = iterationDatas.PersonsNewState.Count + " Plz ?";
                
                mngScript.SetIterationDatas(iterationDatas);
                break;
        }

        ReadPipeData();
    }
}

[Serializable]
public class DataPopulation
{
    public int NbPersons;
    public List<int> IndexOfInfected;
}

[Serializable]
public class DataSites
{
    public int NbHouse;
    public int NbCompany;
    public int NbHospital;
    public int NbRestaurant;
    public int NbSchool;
    public int NbStore;
    public int NbSupermarket;
}

[Serializable]
public class DataIteration
{
    public List<int> PersonsNewSite;
    public List<int> PersonsNewState;
}

public class StreamString
{
    private BinaryReader stream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.stream = new BinaryReader(ioStream);
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len;
        len = stream.ReadByte() * 256;
        len += stream.ReadByte();
        byte[] inBuffer = new byte[len];
        stream.Read(inBuffer, 0, len);

        string outString = streamEncoding.GetString(inBuffer);

        //ioStream.Flush();

        return outString;
    }

    public async Task<string> ReadStringAsync()
    {
        return await Task.Run(() =>
        {
            int len;
            len = stream.ReadByte() << 24;
            len = stream.ReadByte() << 16;
            len += stream.ReadByte() << 8;
            len += stream.ReadByte();
            byte[] inBuffer = new byte[len];
            stream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        });
    }
}
