namespace Client
{
	using System;
	using System.Net.Sockets;

	/// <summary>
	/// Summary description for clsChatSocket.
	/// </summary>
	public class clsChatSocket : System.Net.Sockets.TcpClient
	{
		public clsChatSocket()
		{
		}
		
		public Socket GetThisSocket()
		{
			return this.Client;
		}
		
		public bool IfActive()
		{
			return this.Active; 
		}
	}
}