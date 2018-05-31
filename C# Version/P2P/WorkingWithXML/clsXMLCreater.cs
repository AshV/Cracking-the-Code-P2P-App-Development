namespace WorkingWithXML
{
	// basic package includes
	using System;
    using System.Windows.Forms;
	using System.IO;
 
    ///    Creates requests and appropriate responses
    public class XMLCreater
    {
		// member variables for storing Apllication path of 
		// application that's using this class
		// and path to pick up share.ini from
		string ResponsPath;
		string SharedResourceInfoPath;

		// constructer to initiliaze both thses paths
        public XMLCreater(string responsePath,string sharedResourceInfoPath)
        {
			ResponsPath = responsePath;
			SharedResourceInfoPath = sharedResourceInfoPath;
        }

		// creates an XML document
		private MSXML2.IXMLDOMDocument CreateDocument()
		{
			// crate a document object and retrun that
			MSXML2.IXMLDOMDocument document = new MSXML2.DOMDocument();
			return document;
		}

		// saves and closes XML document
		private void SaveAndCloseDocument(MSXML2.IXMLDOMElement responseElem,MSXML2.IXMLDOMDocument document)
		{
			document.async = false;
			// create Processing Instruction for XML documdnt
			MSXML2.IXMLDOMProcessingInstruction	procsInstruct = document.createProcessingInstruction("xml","version=\"1.0\" encoding=\"utf-8\"");
			// create primary element
			MSXML2.IXMLDOMElement elem = document.createElement("p2p_lng");

			// add response object passed to the primary element as child node
			elem.appendChild(responseElem);
			// add Processing Instruction object and primary object to document 
			// object passed as child nodes and save the document
			document.appendChild(procsInstruct);
			document.appendChild(elem);
			document.save(ResponsPath);
		}
		

		// determins the request type passed in the given XML document
		public string DetermineRequestType(string path,out int UploadDownloadPrint,out string[] chatInfo)
		{
			int b = 0;
			string scopeVal = "";
			bool flag = false;
			string[] st = new string[3];

			try
			{
				// load the XML document
				MSXML2.IXMLDOMDocument document = new MSXML2.DOMDocument();
				if(!document.load(path))
					throw new Exception("XML request found corrupted.");

				// retrieve the request element
				MSXML2.IXMLDOMElement	element = document.documentElement;
				MSXML2.IXMLDOMNode		node = element.firstChild;
				MSXML2.IXMLDOMNamedNodeMap nodemap = node.attributes;
				// retrieve it's attributes
				MSXML2.IXMLDOMNode	childNode = nodemap.nextNode();

				if(0 == node.nodeName.CompareTo("request"))
				{
					// see what value does the element tequest holds
					// and react apprpriately
					switch(childNode.nodeValue.ToString())
					{
						case "CHAT":
						{
							b = 4;
							MSXML2.IXMLDOMNode scope = node.firstChild;
							MSXML2.IXMLDOMNamedNodeMap nodemap2 = scope.attributes;
							MSXML2.IXMLDOMNode childNode2 = nodemap2.nextNode();
							MSXML2.IXMLDOMNode childNode3 = nodemap2.nextNode();
							MSXML2.IXMLDOMNode childNode4 = nodemap2.nextNode();
							// set file name to upload to "path" parameter
							st.Initialize();
							st.SetValue(childNode2.nodeValue.ToString(),0);
							st.SetValue(childNode3.nodeValue.ToString(),1);
							st.SetValue(childNode4.nodeValue.ToString(),2);
							break;
						}
						case "SEARCH":
						{
							WriteSearchResponse(node);
							break;
						}
						case "SHOWFILES":
						{
							WriteShowfileResponse("SHOWFILES");
							break;
						}
						case "DOWNLOAD":
						{
							// set flag that its download request
							b = 2;
							flag = true;
							break;
						}
						case "UPLOAD":
						{
							// set flag that its upload request
							b = 1;
							flag = true;
							break;
						}
						case "PRINT":
						{
							// set flag that its print request
							b = 3;
							flag = true;
							break;
						}

						case "STREAMING":
						{
							// set flag that its Streaming request
							b = 5;
							flag = true;
							break;
						}

						default:
							throw new Exception("Request type could not be resolved.");
					}
					
					if(flag)
					{
						MSXML2.IXMLDOMNode scope = node.firstChild;
						MSXML2.IXMLDOMNamedNodeMap nodemap2 = scope.attributes;
						MSXML2.IXMLDOMNode childNode2 = nodemap2.nextNode();
						// set file name to upload to "path" parameter
						scopeVal = childNode2.nodeValue.ToString();
					}
				}
			}
			catch(Exception e)
			{
				WriteErrorResponse(e.Message);
			}

			chatInfo = st;
			UploadDownloadPrint = b;
			return scopeVal;
		}
		
		// writes error XML responses
		public void WriteErrorResponse(string error)
		{
			// create a document object
			MSXML2.IXMLDOMDocument document = CreateDocument();
			// create response and error info elements
			MSXML2.IXMLDOMElement responseElem = document.createElement("response");
			MSXML2.IXMLDOMElement errorInfoElem	 = document.createElement("errorinfo");

			// set attribute of response element
			responseElem.setAttribute( "type", "ERROR");
			// set attribute of errorinfo element
			errorInfoElem.setAttribute( "errorcode", "1");
			errorInfoElem.setAttribute( "severity", "Error" );
			errorInfoElem.setAttribute( "description", error);
			// add errorinfo element to response object as a child
			responseElem.appendChild(errorInfoElem);
			// save the document
			SaveAndCloseDocument(responseElem,document);
		}

		// writes request XML according to the parameter passed
		public void WriteRequest(string type,string searchValue,string mask)
		{
			// create a document object
			MSXML2.IXMLDOMDocument document = CreateDocument();

			// create request element
			MSXML2.IXMLDOMElement requestElem = document.createElement("request");

			// set attributes of request element
			requestElem.setAttribute( "type", type);

			// if one of these kinds of request is to be made
			// specify the filename and pertaining info too.
			if(type.CompareTo("SHOWFILES") != 0)
			{
				string ReqType = "";
				if(type.CompareTo("CHAT") == 0)
					ReqType = "message";
				else
					ReqType = "scope";

				MSXML2.IXMLDOMElement file_infoElem	= document.createElement(ReqType.ToString());

				if(type.CompareTo("CHAT") == 0)
				{
					file_infoElem.setAttribute("sendername",searchValue.Substring(0,searchValue.IndexOf("(")));
					file_infoElem.setAttribute("senderIP",searchValue.Substring(searchValue.IndexOf("(")+1).Substring(0,searchValue.Substring(searchValue.IndexOf("(")+1).Length-1));
					file_infoElem.setAttribute("chatmsg",mask);
				}
				else
				{
					file_infoElem.setAttribute("type",searchValue);
					file_infoElem.setAttribute("mask",mask);
				}
				requestElem.appendChild(file_infoElem);
			}

			// close and save the documdent
			SaveAndCloseDocument(requestElem,document);
		}


		private void WriteShowfileResponse(string reqType)
		{
			// create a document object
			MSXML2.IXMLDOMDocument document = CreateDocument();
			// create response and error info elements
			MSXML2.IXMLDOMElement responseElem = document.createElement("response");
			// set attribute of response element
			responseElem.setAttribute( "type", reqType);

			// open share.ini for reading
			StreamReader readfile = new StreamReader(SharedResourceInfoPath);
			string readData;

			// read entire file
			while((readData = readfile.ReadLine()) != null)
			{
				try
				{
					// for each entry in share .ini create a fileinfo element
					// and fill it with required information
					MSXML2.IXMLDOMElement file_infoElem = document.createElement("fileinfo");
					int index = readData.IndexOf("=",0);
					file_infoElem.setAttribute( "filename",readData.Substring(0,index));
					file_infoElem.setAttribute( "mask", readData.Substring(index+1,1));

					int secindex = -1;
					if(-1 != (secindex = readData.IndexOf("=",index+1)))
						file_infoElem.setAttribute( "filesize", readData.Substring(secindex+1));

					// add this element to response element as child
					responseElem.appendChild(file_infoElem);
				}
				catch(Exception e)	{ MessageBox.Show("Problem faced while responding : " + e.Message);	}
			}
			// close and save the documdent
			SaveAndCloseDocument(responseElem,document);
		}

		// responds for search requests
		private void WriteSearchResponse(MSXML2.IXMLDOMNode node)
		{
			try
			{
				MSXML2.IXMLDOMNode scope = node.firstChild;
				MSXML2.IXMLDOMNamedNodeMap nodemap = scope.attributes;
				MSXML2.IXMLDOMNode	childNode = nodemap.nextNode();
				MSXML2.IXMLDOMNode	childNode2 = nodemap.nextNode();
				string scopeVal = childNode.nodeValue.ToString();
				string maskVal = childNode2.nodeValue.ToString();

				// make sure that search request has criteria specified in it
				if(0 != scope.nodeName.CompareTo("scope"))
					return;

				// validated that directory's existing
				if(!Directory.Exists(scopeVal.Substring(0, scopeVal.LastIndexOf("\\")+1)))
					throw new Exception("Directory does not exists any more");

				MSXML2.IXMLDOMDocument document = CreateDocument();
				MSXML2.IXMLDOMElement responseElem = document.createElement("response");
				responseElem.setAttribute( "type", "SHOWFILES");

				int i = 0;
				// get files in the specified directory satisfying the
				// given criteria
				string[] files = Directory.GetFiles(scopeVal.Substring(0, scopeVal.LastIndexOf("\\")+1),scopeVal.Substring(scopeVal.LastIndexOf("\\")+1));
				files.Initialize();

				while(i < files.Length)
				{
					// make fileinfo elements and fill then up with 
					// required 
					MSXML2.IXMLDOMElement file_infoElem = document.createElement("fileinfo");
					file_infoElem.setAttribute( "filename",files[i]);
					file_infoElem.setAttribute( "mask",maskVal);
					file_infoElem.setAttribute( "filesize",Convert.ToString(new FileInfo(files[i]).Length));
					++i;
					// add them to response element as children;
					responseElem.appendChild(file_infoElem);
				}

				// get files in the specified directory satisfying the
				// given criteria
				string[] dirs = Directory.GetDirectories(scopeVal.Substring(0, scopeVal.LastIndexOf("\\")+1),scopeVal.Substring(scopeVal.LastIndexOf("\\")+1));
				dirs.Initialize();

				i = 0;
				while(i < dirs.Length)
				{
					// make fileinfo elements and fill then up with 
					// required 
					MSXML2.IXMLDOMElement file_infoElem = document.createElement("fileinfo");
					file_infoElem.setAttribute( "filename",dirs[i] + "\\");
					file_infoElem.setAttribute( "mask",maskVal);
					++i;

					// add them to response element as children;
					responseElem.appendChild(file_infoElem);
				}
				// close and save the document
				SaveAndCloseDocument(responseElem,document);
			}
			catch(Exception e) { WriteErrorResponse(e.Message); }
		}
    }
}