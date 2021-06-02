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
    Thread readThread;
    public Text dataReceived;
    public Text dataReceived1;
    public Text dataReceived2;
    public Text dataReceived3;

    private ManagerScript mngScript;
    //public List<int> testTransfer;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Pipe Opening Process Started");
        pipeClient = new NamedPipeClientStream(".", "dataPipe", PipeDirection.In, PipeOptions.Asynchronous);

        Debug.Log("Connecting to server...\n");

        ConnectToServer();

        mngScript = GetComponent<ManagerScript>();
        //string result = ss.ReadString();
        //ChangingText.GetComponent<Text>().text = result;
        //Debug.Log(result);
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

    private async void ReadPipeData()
    {
        string result = await ss.ReadStringAsync();
        Debug.Log(result);
        //ChangingText.GetComponent<Text>().text = result;
        string resultDataType = result.Split(' ')[0];
        string resultPopulation = result.Split(' ')[1];
        string resultSites = result.Split(' ')[2];


        switch (resultDataType)
        {
            default:
            case "Initialize":
                DataPopulation populationDatas = JsonUtility.FromJson<DataPopulation>(resultPopulation);
                DataSites sitesDatas = JsonUtility.FromJson<DataSites>(resultSites); 
                
                dataReceived.text = result.Split(' ')[0];
                dataReceived3.text = populationDatas.NbPersons + " " + populationDatas.IndexOfInfected.Count;
                dataReceived1.text = result.Split(' ')[1];
                dataReceived2.text = sitesDatas.SitesType.Count + " " + sitesDatas.SitesId.Count;
                
                mngScript.CreateSites(sitesDatas.SitesType, sitesDatas.SitesId);
                mngScript.CreatePopulation(populationDatas.NbPersons, populationDatas.IndexOfInfected);
                break;
            case "Iterate":
                //DataPopulation populationDatas = JsonUtility.FromJson<DataPopulation>(resultPopulation);
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
    public List<int> SitesType;
    public List<int> SitesId;
}

[Serializable]
public class DataIteration
{
    public List<int> personsNewSite;
    public List<int> personsNewState;
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
            len = stream.ReadByte() << 8;
            len += stream.ReadByte();
            byte[] inBuffer = new byte[len];
            stream.Read(inBuffer, 0, len);
            return streamEncoding.GetString(inBuffer);
        });
    }
}
