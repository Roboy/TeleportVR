using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class InfiniTAMSender : MonoBehaviour
{
	public string ipAddress = "localhost";
	public int port = 5447;
	public static InfiniTAMSender instance;
	
	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	#endregion

	void Start()
	{
		if (instance != null)
		{
			Destroy(instance);
			Debug.LogError("Two instances of InfiniTAM Sender");
		}
		instance = this;
			
		ConnectToTcpServer();
	}
	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	public void ConnectToTcpServer()
	{
		try
		{
			socketConnection = new TcpClient(ipAddress, port);
			Debug.Log("Successfully connected to InfiniTUM client.");
			UIManager.WriteToLogger("Socket connection successful.");
			UIManager.SetConnectionState(1, true);
		}
		catch (Exception e)
		{
			Debug.Log("On InfiniTUM client connect exception " + e);
			UIManager.WriteToLogger("Client socket connection failed.");
		}
	}
	
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public new void SendData(int[] header, byte[] data)
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] headerAsBytes = new byte[header.Length * sizeof(int)];
				Buffer.BlockCopy(header, 0, headerAsBytes, 0, headerAsBytes.Length);
				// Write byte array to socketConnection stream.                 
				stream.Write(headerAsBytes, 0, headerAsBytes.Length);
				//Debug.Log("Header sent");
			}
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				//byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
				// Write byte array to socketConnection stream.                 
				stream.Write(data, 0, data.Length);
				//Debug.Log("Data send");
			}
		}
		catch (SocketException socketException)
		{
			Debug.LogError("Socket exception: " + socketException);
		}
	}
	
	public new void SendHeader(int[] header)
	{
		if (socketConnection == null)
		{
			Debug.Log("Noo connection");
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] headerAsBytes = new byte[header.Length * sizeof(int)];
				Buffer.BlockCopy(header, 0, headerAsBytes, 0, headerAsBytes.Length);
				// Write byte array to socketConnection stream.                 
				stream.Write(headerAsBytes, 0, headerAsBytes.Length);
				Debug.Log("Header sent");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}