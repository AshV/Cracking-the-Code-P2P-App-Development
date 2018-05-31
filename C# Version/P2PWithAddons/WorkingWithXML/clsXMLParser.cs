namespace WorkingWithXML
{
    using System;			// Provides the basic functionality of .NET
	using System.Net;		// Provides the net related functionality 
	using System.IO;		// Provides the I/O functionality
	using System.Text;		// Provides text based manipulations

	/// <summary>
	///  Generic structure used for parsing the XML data
	///  This structure composes of various sub structures
	///  Each sub structure represents and XML request in whole
	///  Every XML is parsed into its corresponding structure to 
	///  fill its values TypeOfXMLRecieved() function of 
	///  ServerCommunication class will determine that which structure
	///  has to be filled
	/// </summary>
	public struct XMLSTRUCT
	{
		/// <summary>
		/// This structure is used store the parsed 
		/// values of the AUTH XML which is returned
		/// after login process
		/// </summary>
		public struct __AUTHENTICATION
		{
			/// <summary>
			/// Stores the Code value in it 0(successful)
			/// or 1(some error occured)
			/// </summary>
			public int iCode;
			
			/// <summary>
			/// This will stores the status of the login process
			/// and any error message if occured while login
			/// </summary>
			public string sStatus;
			
			/// <summary>
			/// This is used for cross checking the IP address
			/// which is send to the server that login is successful
			/// or not
			/// </summary>
			public string sIPAddress;
		}

		/// <summary>
		/// This structure is used to store the List of all 
		/// the Listeners from the server that are currently 
		/// The values are returned in the USERLIST response XML
		/// running
		/// </summary>
		public struct __USERLIST
		{
			/// <summary>
			/// Name by which the Listener has logged in
			/// </summary>
			public string sUsername;
			
			/// <summary>
			/// IP Address of that Listener
			/// </summary>
			public string sIPAddress;
		}

		/// <summary>
		/// This is use to store the values which are parsed
		/// from the SHOWFILES response XML from the Listener
		/// It containes the Files and Folders which are to 
		/// be shown to the user
		/// </summary>
		public struct __SHOWFILES
		{
			/// <summary>
			/// Stores the Filename or Folder name
			/// </summary>
			public string	sFilename;
			
			/// <summary>
			/// Stores the FileSize, 0 in case of folders
			/// </summary>
			public int		iFileSize;
			
			/// <summary>
			/// Mask stores the mask value of a file or folder
			/// 0(readonly file/folder) 1(read/write access)
			/// </summary>
			public int		iMask;
		}

		/// <summary>
		/// In case of any Error an ERROR response XML is
		/// thrown from the Listener. The values are parsed into
		/// this structure
		/// </summary>
		public struct __ERROR
		{
			/// <summary>
			/// Stores the error code
			/// </summary>
			public int		iErrCode;
			
			/// <summary>
			/// Stores the severity of the error
			/// Message or Warning or Error
			/// </summary>
			public string	sSeverity;
			
			/// <summary>
			/// The actual error description is stored in this 
			/// variable
			/// </summary>
			public string	sDescription;
		}

		/// <summary>
		/// no XML parser has been made for this structure, 
		/// since it is not used in this version
		/// </summary>
		public struct __UPDNLOAD
		{
			public string	sFilename;
			public int		iMask;
		}

		/// <summary>
		/// no XML parser has been made for this structure, 
		/// since it is not used in this version
		/// </summary>
		public struct __MESSAGE
		{
			public string	sSenderName;
			public string	sMessage;
			public string	sIPAddress;
		}

		/// <summary>
		/// this structure stores the values from the 
		/// SERVERSEARCH XML that is returned by the Server
		/// as the result of search
		/// </summary>
		public struct __SERVERSEARCH
		{
			/// <summary>
			/// IP address of the machine where the file or folder
			/// is found
			/// </summary>
			public string 	sIPAddress;
			
			/// <summary>
			/// Username i.e login name of the machine
			/// </summary>
			public string	sUsername;
			
			/// <summary>
			/// Name of the file found for search criteria is
			/// in this variable
			/// </summary>
			public string	sFilename;
		}

		/// <summary>
		/// Global varibales which are used
		/// in differents parts of the code
		/// for their specific structures
		/// </summary>
		public __AUTHENTICATION		AUTH;
		public __USERLIST[]			USERLIST;
		public __SHOWFILES[]		SHOWFILES;
		public __SHOWFILES[]		SEARCH;
		public __SERVERSEARCH[]     SERVERSEARCH;
		public __ERROR				ERROR;
		public __MESSAGE			MESSAGE;
	}

	/// <summary>
    ///    Summary description for clsXMLParser.
    ///    This class is used to parse any XML that is recieved
    ///    by the Listener of Browser(Client)
    ///    and stores the values to their corresponding
    ///    structures so that the application could use them
    /// </summary>
 	public class XMLParser
    {
		/// <summary>
		/// Stores the Filename to write when login response
		/// when has arrived to the Browser
		/// </summary>
		public string							LOGINXML;

		/// <summary>
		/// Stores the Filename to write when USERLIST response
		/// when has arrived to the Browser
		/// </summary>
		public string							USERLISTXML;

		/// <summary>
		/// Stores the Filename to write when SERVERSEARCH response
		/// when has arrived to the Browser
		/// </summary>
		public string							SERVERSEARCHRESULTXML;

		/// <summary>
		/// stores the number of tags that are found
		/// in the response XML
		/// </summary>
		protected int							iTags;
		
		/// <summary>
		/// Used to store the counter that is how many time
		/// a loop is running 
		/// </summary>
		protected int							iCounter;
		
		/// <summary>
		/// This document variable points to the XML documet
		/// </summary>
		protected MSXML2.IXMLDOMDocument		document;
		
		/// <summary>
		/// Points to the element of the XML document
		/// </summary>
		protected MSXML2.IXMLDOMElement			element;
		
		/// <summary>
		/// Points to the node of the XML 
		/// </summary>
		protected MSXML2.IXMLDOMNode			node, ChildNode;
		
		/// <summary>
		/// points to the node list of the XML document
		/// Stores the node list of the XML
		/// </summary>
		protected MSXML2.IXMLDOMNodeList		nodeList;
		
		/// <summary>
		/// Stores the node map of the XML document
		/// </summary>
		protected MSXML2.IXMLDOMNamedNodeMap	nodeMap; 

		/// <summary>
		/// Default constructor
		/// </summary>
		public XMLParser()
        {
        }
		
		/// <summary>
		/// Initialize some important variables
		/// </summary>
		protected void InitVariables()
		{
			iTags=0;
			iCounter = 0;
			document = new MSXML2.DOMDocument();
		}

		/// <summary>
		/// This function is responsible for parsing the XML
		/// Actually this function will call the exact parse function
		/// depending upon the type of XML Recieved
		/// </summary>
		/// <param name="XMLFilename"> </param>
		/// <param name="outStruct"> </param>
		/// <param name="TagName"> </param>
		public int ParseXML(string XMLFilename, out XMLSTRUCT outStruct, string TagName)
		{
			// Declare and initializes the iElements to 0
			int iElements = 0;
			
			// Initializes the outStruct variable of this function
			// this structure is used to store the values of parsed XML
			outStruct = new XMLSTRUCT();
			
			// The following 12 lines of code checks the Type of XML recieved
			// and calls are made to there corresponding parser function
			// which actually are reponsible for parsing the XML
			// all the parse functions are user defined functions
			// the Number of Parsed records are stores in the iElements
			// variable which is returned by the function
			if( 0 == TagName.CompareTo("AUTH") ) 
				iElements = ParseAUTHXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("USERLIST") ) 
				iElements = ParseUSERLISTXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("SHOWFILES") ) 
				iElements = ParseSHOWFILESXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("SEARCH") ) 
				iElements = ParseSHOWFILESXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("ERROR") ) 
				iElements = ParseERRORXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("SERVERSEARCH") ) 
				iElements = ParseSERVERSEARCHXML(XMLFilename, out outStruct);
			else if( 0 == TagName.CompareTo("CHAT") ) 
				iElements = ParseCHATXML(XMLFilename, out outStruct);

			// Returns the iElements variable to the calling function
			return iElements;
		}

		protected int ParseCHATXML(string Filename, out XMLSTRUCT outStruct)
		{
			
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;
				
				// Initialize the AUTH sructure of the outStruct
				// variable
				outStruct.MESSAGE = new XMLSTRUCT.__MESSAGE();
				
				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("sendername") )
							outStruct.MESSAGE.sSenderName = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("chatmsg") )
							outStruct.MESSAGE.sMessage = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("ip") )
							outStruct.MESSAGE.sIPAddress = ChildNode.nodeValue.ToString();
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}

			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}
		
		/// <summary>
		/// Actual Parsing of AUTHENTICATION XML
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="outStruct"> </param>
		protected int ParseAUTHXML(string Filename, out XMLSTRUCT outStruct)
		{
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;
				
				// Initialize the AUTH sructure of the outStruct
				// variable
				outStruct.AUTH = new XMLSTRUCT.__AUTHENTICATION();
				
				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("code") )
							outStruct.AUTH.iCode =  Convert.ToInt32(ChildNode.nodeValue);
						else if( 0 == ChildNode.nodeName.CompareTo("status") )
							outStruct.AUTH.sStatus = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("ip") )
							outStruct.AUTH.sIPAddress = ChildNode.nodeValue.ToString();
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}
			
			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}

		/// <summary>
		/// Actual Parsing of USERLIST XML
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="outStruct"> </param>
		protected int ParseUSERLISTXML(string Filename, out XMLSTRUCT outStruct)
		{
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;

				// Initialize the USERLIST sructure of the outStruct
				// variable
				outStruct.USERLIST = new XMLSTRUCT.__USERLIST[iTags];

				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("username") )
							outStruct.USERLIST[iCounter].sUsername = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("ip") )
							outStruct.USERLIST[iCounter].sIPAddress = ChildNode.nodeValue.ToString();
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}
			
			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}
	
		/// <summary>
		/// Actual Parsing of SERVERSEARCH XML
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="outStruct"> </param>
		protected int ParseSERVERSEARCHXML(string Filename, out XMLSTRUCT outStruct)
		{
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;

				// Initialize the SERVERSEARCH sructure of the outStruct
				// variable
				outStruct.SERVERSEARCH = new XMLSTRUCT.__SERVERSEARCH[iTags];

				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("ip") )
							outStruct.SERVERSEARCH[iCounter].sIPAddress = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("username") )
							outStruct.SERVERSEARCH[iCounter].sUsername = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("filename") )
							outStruct.SERVERSEARCH[iCounter].sFilename = ChildNode.nodeValue.ToString();
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}
			
			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}

		/// <summary>
		/// Actual Parsing of SHOWFILES XML
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="outStruct"> </param>
		protected int ParseSHOWFILESXML(string Filename, out XMLSTRUCT outStruct)
		{
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;

				// Initialize the SHOWFILES sructure of the outStruct
				// variable
				outStruct.SHOWFILES = new XMLSTRUCT.__SHOWFILES[iTags];

				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("filename") )
							outStruct.SHOWFILES[iCounter].sFilename = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("mask") )
							outStruct.SHOWFILES[iCounter].iMask =  Convert.ToInt32(ChildNode.nodeValue);
						else if( 0 == ChildNode.nodeName.CompareTo("filesize") )
							outStruct.SHOWFILES[iCounter].iFileSize =  Convert.ToInt32(ChildNode.nodeValue);
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}
			
			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}

		/// <summary>
		/// Actual Parsing of ERROR XML
		/// </summary>
		/// <param name="Filename"> </param>
		/// <param name="outStruct"> </param>
		protected int ParseERRORXML(string Filename, out XMLSTRUCT outStruct)
		{
			// initializes all the required variables
			InitVariables();
			
			// Initialize outStruct variable of this function
			outStruct = new XMLSTRUCT();
			
			// Process the XML document syncronously
			document.async = false;
			
			// load the xml document in memory for parsing
			if(document.load(Filename))
			{
				// get the first element of the XML
				element = document.documentElement; 
				
				// get the first child of the element
				node = element.firstChild;
				
				// extracts the node list present under the node
				nodeList = node.childNodes;
				
				// iTags will assigns to the number of nodes present
				// in node list
				iTags = nodeList.length;

				// Initialize the ERROR sructure of the outStruct
				// variable
				outStruct.ERROR = new XMLSTRUCT.__ERROR();

				// move the node to the next node of the nodelist
				node = nodeList.nextNode();
				
				// Extract each value from its specific node
				for(iCounter = 0; iCounter < iTags; iCounter++ )
				{
					// gets the attribute map that is how many attributes
					// are present in the node
					nodeMap = node.attributes;
					
					// extract the next node from the node map
					ChildNode = nodeMap.nextNode();
					
					// The following 9 lines of code will extract the 
					// various attribute values from the XML node
					// and fills it to the outStruct's corresponding
					// structure
					do
					{
						if( 0 == ChildNode.nodeName.CompareTo("errorcode") )
							outStruct.ERROR.iErrCode = Convert.ToInt32(ChildNode.nodeValue);
						else if( 0 == ChildNode.nodeName.CompareTo("severity") )
							outStruct.ERROR.sSeverity = ChildNode.nodeValue.ToString();
						else if( 0 == ChildNode.nodeName.CompareTo("description") )
							outStruct.ERROR.sDescription = ChildNode.nodeValue.ToString();
					} while( null != (ChildNode = nodeMap.nextNode()) );
					
					// now move to next node
					node = nodeList.nextNode();
				}
			}
			
			// Return the number of nodes parsed for the values
			return iCounter==iTags?iCounter:0;
		}
    }
}