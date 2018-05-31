namespace WorkingWithXML
{
    using System;			// Provides the basic functionality of .NET
	using System.Net;		// Provides the net related functionality 
	using System.IO;		// Provides the I/O functionality
	using System.Text;		// Provides text based manipulations
	using System.Windows.Forms;	// Provides the use of graphic interface
	using System.Web;

    /// <summary>
    ///    Summary description for ServerCommunication.
    ///    This class is responsible for all the communication with
    ///    the server as well as Listener
    ///    It has got some handy functions which can be very helpful
    ///    like: GetIPAddress, FileDelete etc
    /// </summary>
    public class ServerCommunication
    {
        /// <summary>
        /// Default constructor of the class
        /// This cinstructor of the class is generated automatically
        /// by the IDE
        /// </summary>
		public ServerCommunication()
        {
		}

		/// <summary>
		/// Get the response data from server represented by WebAddress
		/// When the request is made to the server it opens a stream
		/// to the response and the function read bytes from that 
		/// response stream and convert them to string and returns the value
		/// </summary>
		/// <param name="WebAddress"> </param>
		public string GetDataFromServer(string WebAddress)
		{
			// Declares a local variable webRequest of type HttpWebRequest
			// which is a part of System.Net package. It is used to form
			// an HttpRequest and sends it to the server
			HttpWebRequest	webRequest;

			// Declares a local variable webResponse of type HttpWebResponse
			// which is a part of System.Net package. It is used to get the
			// response from the server against the HttpWebRequest 
			HttpWebResponse	webResponse;

			// Declares a variable responseStream of Stream type which is 
			// use to get the response from the server's end
			Stream			responseStream;
			
			// streamBuffer variable is declared here of type Byte array
			// this is used to read the actual data from the responseStream
			Byte[]			streamBuffer;
			
			// Declares a variable ReturnData of type string which
			// at last stores the data which is to be returned
			string			ReturnData;

			// initializes the ReturnData to null
			ReturnData = null;
			
			// Creates and initializes a webRequest by calling Create
			// function of the WebRequestFactory and type cast it to
			// HttpWebRequest type
			webRequest = (HttpWebRequest)WebRequest.Create(WebAddress);

			// After requesting the server for HttpWebRequest
			// it will opens a response for the clients end to read
			// this response is catched by the foloowinf line of code
			// and assigns it to webResponse
			webResponse =(HttpWebResponse)webRequest.GetResponse();
				
			// GetResponseStream method of webResponse actually gets
			// the response stream and assigns it to the responseStream
			responseStream = webResponse.GetResponseStream();
				
			// initialize streamBuffer so that it can read 16 bytes of data
			// at a time
			streamBuffer = new Byte[16];
				
			// Declares a int variable iBytesRead which keeps the
			// records of how many bytes have been read from the
			// stream
			int iBytesRead;
				
			// Reads 16 bytes from the stream until the stream gets
			// enpty and the value assigned to iBytesRead is zero
			while( 0 != (iBytesRead = responseStream.Read(streamBuffer, 0, 16)) )
				
				// This will convert the bytes data that is read from the
				// stream and stored in streamBuffer to string and concatenates
				// it to ReturnData
				ReturnData += Encoding.ASCII.GetString(streamBuffer, 0, iBytesRead);
				
			// Removes the leading and trailing spaces from the Data
			// that is stored is returnData variable
				
			if( ReturnData != null )
			{
				ReturnData = ReturnData.Trim();
//				ReturnData = ReturnData.Substring(0,ReturnData.LastIndexOf("</p2p_lng>") + "</p2p_lng>".Length );
			}

			// Flushes the responseStream
			responseStream.Flush();
			
			// Closes the responseStream
			responseStream.Close();
			
			// Returns the value of ReturnData variable
			return ReturnData;
		}
		
		/// <summary>
		/// Get the response data from server represented by WebAddress
		/// using Proxy server. When the request is made to the server it 
		/// opens a stream to the response and the function read bytes 
		/// from that response stream and convert them to string and 
		/// returns the value
		/// </summary>
		/// <param name="WebAddress"> </param>
		public string GetDataFromServerUsingProxy(string WebAddress, string ProxyAddress, int Port)
		{
			// Declares a local variable webRequest of type HttpWebRequest
			// which is a part of System.Net package. It is used to form
			// an HttpRequest and sends it to the server
			HttpWebRequest	webRequest;

			// Declares a local variable webResponse of type HttpWebResponse
			// which is a part of System.Net package. It is used to get the
			// response from the server against the HttpWebRequest 
			HttpWebResponse	webResponse;

			// Declares a variable responseStream of Stream type which is 
			// use to get the response from the server's end
			Stream			responseStream;
			
			// streamBuffer variable is declared here of type Byte array
			// this is used to read the actual data from the responseStream
			Byte[]			streamBuffer;
			
			// Declares a variable ReturnData of type string which
			// at last stores the data which is to be returned
			string			ReturnData;

			// initializes the ReturnData to null
			ReturnData = null;

			System.Net.IWebProxy ProxyData = new System.Net.WebProxy(ProxyAddress,Port);
				
			// Creates and initializes a webRequest by calling Create
			// function of the WebRequestFactory and type cast it to
			// HttpWebRequest type
			webRequest = (HttpWebRequest)WebRequest.Create(WebAddress);
			webRequest.Proxy = ProxyData;

			// After requesting the server for HttpWebRequest
			// it will opens a response for the clients end to read
			// this response is catched by the foloowinf line of code
			// and assigns it to webResponse
			webResponse =(HttpWebResponse)webRequest.GetResponse();
				
			// GetResponseStream method of webResponse actually gets
			// the response stream and assigns it to the responseStream
			responseStream = webResponse.GetResponseStream();
				
			// initialize streamBuffer so that it can read 16 bytes of data
			// at a time
			streamBuffer = new Byte[16];
				
			// Declares a int variable iBytesRead which keeps the
			// records of how many bytes have been read from the
			// stream
			int iBytesRead;
				
			// Reads 16 bytes from the stream until the stream gets
			// enpty and the value assigned to iBytesRead is zero
			while( 0 != (iBytesRead = responseStream.Read(streamBuffer, 0, 16)) )
					
				// This will convert the bytes data that is read from the
				// stream and stored in streamBuffer to string and concatenates
				// it to ReturnData
				ReturnData += Encoding.ASCII.GetString(streamBuffer, 0, iBytesRead);
				
			// Removes the leading and trailing spaces from the Data
			// that is stored is returnData variable
			if( ReturnData != null )
			{
				ReturnData = ReturnData.Trim();
//				ReturnData = ReturnData.Substring(0,ReturnData.LastIndexOf("</p2p_lng>") + "</p2p_lng>".Length );
			}

			// Flushes the responseStream
			responseStream.Flush();
			
			// Closes the responseStream
			responseStream.Close();
			
			// Returns the value of ReturnData variable
			return ReturnData;
		}

		/// <summary>
		/// Writes the DataToWrite to Filename
		/// This function writes the string data which is stored in
		/// DataToWrite variable to the file pointed by Filename
		/// Usually we write XML file
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="DataToWrite"> </param>
		public void WriteDataToFile(string Filename, string DataToWrite)
		{
			// Declares and initializes the FileToCreate variable of 
			// type File and passess Filename to its constructor to 
			// associate it with the File
			//File FileToCreate = new File(Filename);

			// Addedd for Beta 2
			FileStream WriteStream = new FileStream(Filename,FileMode.Create);

			// Deaclares and creates a WriteStream object, used
			// to write the data to the stream which is present
			// in DataToWrite variable
			//Stream WriteStream = FileToCreate.OpenWrite();

			// Writes the data to the file by converting data
			// to byte format
			WriteStream.Write(Encoding.ASCII.GetBytes(DataToWrite), 0, DataToWrite.Length);
			
			// Closes the written file
			WriteStream.Close();
		}

		/// <summary>
		/// Determine the type of request/response recived in XML
		/// This is done by extracting the attribute value from the
		/// first node value of first child element of the document
		/// </summary>
		/// <param name="XMLFilename"> </param>
		public string TypeOfXMLRecieved(string XMLFilename)
		{
			// Declares and initializes a local variable document
			// of type IXMLDOMDocument present in MSXML2 class. This
			// variable is used to point to the XML filename or document
			MSXML2.IXMLDOMDocument	document = new MSXML2.DOMDocument();
			
			// Declares a local variable element of type IXMLDOMElement
			// This is used to point to the elements present in the XML
			// document
			MSXML2.IXMLDOMElement	element;

			// Declares a local variable node of type IXMLDOMElement
			// This is used to point to the nodes present in the XML
			// Document
			MSXML2.IXMLDOMNode		node;
			
			// A local variable NodeValue is declared of type string 
			// it is used to store the retrieved value from the XML and 
			// returns it from the function
			string NodeValue;

			// Read the XML document syncronously
			document.async = false;
			
			// Initializes NodeValue to null
			NodeValue = null;
			
			// Loads the XML document for reading
			if( document.load(XMLFilename) )
			{
				// Extract the first element of the XML
				element = document.documentElement; 
				
				// Extract the first child node from the element
				// and stores it to the node
				node = element.firstChild;
				
				// now extract the first node value from the attributes
				// present in the XML and saves it to NodeValue
				NodeValue = node.attributes.nextNode().nodeValue.ToString(); 
			}
			
			// Simply returns the NodeValue variable
			return NodeValue;
		}

		/// <summary>
		/// This function is used to delete a file represented by filename
		/// </summary>
		/// <param name="Filename"> </param>
		public void FileDelete(string Filename)
		{
			// Declares and initializes an object f of type File which is
			// present in System.IO package and assigns Filename to it
			//File f = new File(Filename);
			
			//Added for Beta 2
			File.Delete(Filename);
			// Calls the Delete function of File Class to delete the 
			// file represented by Filename
			//f.Delete();
		}

		/// <summary>
		/// Retrieve the IP Address of the machine represented by
		/// Hostname. This function makes the use of the DNS class
		/// for extracting the IP address and returns the first entry
		/// from the IP list obtained
		/// </summary>
		/// <param name="Hostname"> </param>
		public string GetIPAddress(string Hostname)
		{
			// Creates a new local variable named LocalHost of type
			// IPHostEntry which is present in the System.Net package
			// It then calls the GetHostByName function of the DNS class
			// and passes the Hostname to it
			IPHostEntry		LocalHost = Dns.GetHostByName(Hostname);	// To retrieve my computer's IP

			// Now the LocalHost has got the list of IPs corresponding
			// to the hostname and it will return the first entry from the
			// list
			return LocalHost.AddressList[0].ToString();
		}
    }
}
