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
using System.IO;

public class ScriptClient : MonoBehaviour
{
    public GameObject ChangingText;
    public NamedPipeClientStream pipeClient;
    public StreamString ss;
    Thread readThread;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Pipe Opening Process Started");
        pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous);

        Debug.Log("Connecting to server...\n");
        pipeClient.Connect();

        ss = new StreamString(pipeClient);

        string result = ss.ReadString();
        ChangingText.GetComponent<Text>().text = result;
        Debug.Log(result);

        Thread.Sleep(250);

        InvokeRepeating("ReadPipeData", 0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Updating");
        //Debug.Log(ss.ReadString());
        //getPipedData();
    }

    private async void ReadPipeData()
    {
        string result = await ss.ReadStringAsync();
        ChangingText.GetComponent<Text>().text = result;
    }

    private void getPipedData()
    {
        Debug.Log("Thread Called - Start");

        ss = new StreamString(pipeClient);
        Debug.Log(ss.ReadString());
        Debug.Log("Thread Called - End");

    }
}



public class StreamString
{
    private Stream ioStream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len;
        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        byte[] inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);

        string outString = streamEncoding.GetString(inBuffer);

        //ioStream.Flush();

        return outString;
    }

    public async Task<string> ReadStringAsync()
    {
        return await Task.Run(() =>
        {
            int len;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);
            return streamEncoding.GetString(inBuffer);
        });
    }

    public async Task<int> ReadIntAsync()
    {
        return await Task.Run(() =>
        {
            return ioStream.ReadByte();
        });
    }

    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }
}
