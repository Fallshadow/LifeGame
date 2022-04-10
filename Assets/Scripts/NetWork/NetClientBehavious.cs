using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class NetClientBehavious : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public LifeGameChessBoard PaintChessBoard = null;
    public bool m_Done;

    public int sendTime = -1;
    public List<byte[][]> sendListDatas = new List<byte[][]>();
    public List<bool> sendListBool = new List<bool>();
    
    void Start ()
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 9000;
        m_Connection = m_Driver.Connect(endpoint);
        if (PaintChessBoard != null)
        {
            PaintChessBoard.UpdateChessBoardCB += () =>
            {
                SendChessBoradData(PaintChessBoard.CurDatas, PaintChessBoard.needReset);
                PaintChessBoard.needReset = false;
            };
        }
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            if (!m_Done)
                Debug.Log("Something went wrong during connect");
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;

        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");

                if(!GameControllerChessBoard.instance.StartUpdate) continue;

                if (sendListDatas[sendTime] != null)
                {
                    m_Driver.BeginSend(m_Connection, out var writerStart);
                    writerStart.WriteFixedString32($"Start_{sendListDatas[sendTime].Length}_{sendListDatas[sendTime][0].Length}");
                    m_Driver.EndSend(writerStart);
                    
                    for (int index = 0; index < sendListDatas[sendTime].Length; index++)
                    {
                        for (int index2 = 0; index2 < sendListDatas[sendTime][index].Length; index2++)
                        {
                            m_Driver.BeginSend(m_Connection, out var writer);
                            writer.WriteUInt(sendListDatas[sendTime][index][index2]);
                            m_Driver.EndSend(writer);
                        }
                    }
                    
                    m_Driver.BeginSend(m_Connection, out var writerEnd);
                    writerEnd.WriteFixedString32("End");
                    m_Driver.EndSend(writerEnd);
                }

                // uint[][] value = new uint[2][];
                // for (int index = 0; index < value.Length; index++)
                // {
                //     value[index] = new uint[2];
                //     for (int index2 = 0; index2 < value.Length; index2++)
                //     {
                //         value[index][index2] = 2;
                //         m_Driver.BeginSend(m_Connection, out var writer);
                //         writer.WriteUInt(value[index][index2]);
                //         m_Driver.EndSend(writer);
                //     }
                // }

                // byte[][] data = new byte[2][];
                // for (int index = 0; index < data.Length; index++)
                // {
                //     data[index] = new byte[2];
                //     Buffer.BlockCopy(value[index], 0, data[index], 0, data[index].Length);
                // }
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                Debug.Log("Got the value = " + value + " back from the server");
                m_Done = true;
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                m_Connection = default(NetworkConnection);
                sendListDatas.Clear();
                sendTime = 0;
            }
        }
        
        Debug.Log("Empty");
    }

    public void SendChessBoradData(byte[][] datas, bool reset)
    {
        sendTime++;
        sendListDatas.Add(datas);
    }
}