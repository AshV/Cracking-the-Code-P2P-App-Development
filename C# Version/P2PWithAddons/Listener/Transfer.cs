namespace Listener
{
	using System;
	using System.Net.Sockets;
	using System.Net;
	using System.Threading;
	using System.Text;
	using System.IO;
	using System.WinForms;

	public class MyTcpListener :  TCPListener
	{
		public MyTcpListener(int port) : base(port)
		{

		}
		public void StopMe() 
		{
			if ( this.Server != null )
			{
				this.Server.Close();
			}
		}
	}

	/// <summary>
	///    Summary description for Transfer.
	/// </summary>
	public class Transfer
	{
		MyTcpListener tcpl;

		public Transfer()
		{
			int port = 8081;
			this.tcpl = new MyTcpListener(port);			
		}
	
		public void TransferShutdown()
		{
			tcpl.StopMe();
		}

		public void ListenForPeers() 
		{        			
			try
			{  
				Encoding ASCII = Encoding.ASCII;     
								
				tcpl.Start();								

				while (true)
				{		
					// Accept will block until someone connects					
					Socket s = tcpl.Accept();		
					//MessageBox.Show("A Client Connected");
					NetworkStream DataStream = new NetworkStream(s);

					String filename;
					Byte[] Buffer = new Byte[256];
					DataStream.Read(Buffer, 0, 256);
					filename = Encoding.ASCII.GetString(Buffer);
					StringBuilder sbFileName = new StringBuilder(filename);
					FileStream fs = new FileStream(sbFileName.ToString(), FileMode.Open, FileAccess.Read);     
					BinaryReader reader = new BinaryReader(fs);
					byte[] bytes = new byte[1024];
					int read;
					while((read = reader.Read(bytes, 0, bytes.Length)) != 0) 
					{
						DataStream.Write(bytes, 0, read);
					}
					reader.Close();	
					DataStream.Flush();
					DataStream.Close();
				}
			}
			catch(SocketException ex)
			{				
				MessageBox.Show(ex.ToString());
			}
		}

		public void DownloadToClient(String server, string remotefilename, string localfilename) 
		{
			try
			{
				TCPClient tcpc = new TCPClient();                
				Byte[] read = new Byte[1024];           		       

				int port = 8081;

				IPAddress adr = new IPAddress(server);
				IPEndPoint ep = new IPEndPoint(adr, port);

				if (tcpc.Connect(ep) == -1) 				
				{		
					throw new Exception("Unable to connect to " + server + " on port " + port);
				}
				// Get the stream
				Stream s = tcpc.GetStream();

				Byte[] b = Encoding.ASCII.GetBytes(remotefilename.ToCharArray());
				s.Write( b, 0,  b.Length );
				int bytes;

				FileStream fs = new FileStream(localfilename, FileMode.OpenOrCreate);

				BinaryWriter w = new BinaryWriter(fs);

				// Read the stream and convert it to ASII
				while( (bytes = s.Read(read, 0, read.Length)) != 0) 
				{ 			
					w.Write(read, 0, bytes);
					read = new Byte[1024];				
				}               

				tcpc.Close();
				w.Close();
				fs.Close();
			}
			catch(Exception ex)
			{
				throw new Exception(ex.ToString());					
			}
		}	
	}
}
