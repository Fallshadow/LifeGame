using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetServerBehavious : MonoBehaviour
{
    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;
    private bool isWriting = false;
    private byte[][] datas;
    private int allDataCount = 0;
    private int weight = 0;
    private int height = 0;
    private int curDataCount = 0;
    
    private void Start()
    {
        m_Driver = NetworkDriver.Create();
        var endPoint = NetworkEndPoint.AnyIpv4;
        endPoint.Port = 9000;
        if (m_Driver.Bind(endPoint) != 0)
        {
            Debug.Log("Failed to bind to port 9000");
        }
        else
        {
            m_Driver.Listen();
        }

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                continue;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    string start = stream.ReadFixedString32().ToString();
                    if (start.Contains("Start"))
                    {
                        if (isWriting)
                        {
                            Debug.Log("丢弃！");
                            continue;
                        }

                        string[] wlDatas = start.Split('_');
                        if (wlDatas.Length == 3)
                        {
                            weight = Convert.ToInt32(wlDatas[1]);
                            height = Convert.ToInt32(wlDatas[2]);
                            datas = new byte[weight][];
                            for (int index = 0; index < datas.Length; index++)
                            {
                                datas[index] = new byte[height];
                            }

                            allDataCount = height * weight;
                            curDataCount = 0;
                        }
                        else
                        { 
                            Debug.Log("丢弃！");
                            continue;
                        }

                        isWriting = true;
                    }
                    else if (start == "End")
                    {
                        isWriting = false;
                        
                    }

                    curDataCount++;
                    int newHeight = (curDataCount - 1) / weight;
                    int newWidth = (curDataCount - newHeight * weight);
                    datas[newHeight][newWidth] = stream.ReadByte();
                    Debug.Log($"当前行数：{newHeight} 当前列数：{newWidth} 当前值：{datas[newHeight][newWidth]}");
                }
                // 处理断开连接
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
