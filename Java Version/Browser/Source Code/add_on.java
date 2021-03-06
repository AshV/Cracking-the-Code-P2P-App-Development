import java.net.*;
import java.io.*;
import java.util.*;
/* This class file is a mutipurpose class file it contains many functions which are used 
   over a series of classes in the project...
 */

class add_on  
{
		Socket file_socket;
		String viewfiles[][];
		static Vector filenames ;	// A vector to maintain the user list (static) 
		

		add_on()		// Constructor...
		{

		}
		/*  This function is used to adjust the length of the byte array and to make it 
			equal to 1024 byte this is done in order to make the size of the request 
			equal to the request accepted by the C# listener... 

			In the function appropriatelength two parameters are passed 
			1. Byte array...
			2. Length of byte array...

			The rest of the bytes (1024 - length) are first converted into character and 
			then made into  char '13' and then converted into bytes again..
			
		*/

		public byte [] appropriatelength(byte[] file_bytes, int file_size)
		{
				int count = 0;
				byte b[] = new byte[1024];
				int remaining = 1024-file_size;

				for (int i = 0;i<file_bytes.length ;i++ )
				{
					b[i] = file_bytes[i];
				}
				
				char a[] = new char[remaining];
		
				for (int i = 0;i<remaining ;i++ )
				{
				a[i] = 13;
				}

				String tempw = new String(a);
				byte d[] = tempw.getBytes();

				for (int i=file_size;i<1024 ;i++ )
				{
				b[i] = d[(i-file_size)];
				}
				
				return (b);
		}			// End Appropriate length.....
		
		/* This function is used to issue the search request... 
			It takes 4 parameters...
				1. Directory information...
				2. Search Criteria.. 
				3. Ip_address of the listener...
				4. Flag_information of the directory on which search is made...

		*/
		

			public void search_request(String directory, String condition, String ip_address, String flag_info)
			{

			try
			{
				InetAddress inet = InetAddress.getByName(ip_address);
				file_socket = new Socket(inet,7070); // Create a client_socket on the 
													// Listener's machine at port 7070. 
		
				// Get The output as well as the input Streams on that socket... 
				BufferedOutputStream out = new BufferedOutputStream(file_socket.getOutputStream());
			
				BufferedInputStream br_socket = new BufferedInputStream(file_socket.getInputStream());
				
				// Now a request is sent to the Listener to show all the shareable files 
				// of the particular directory that satisfy the search criteria..selected
				// by the user..

				// To send a request a class of the name xmlwriter is used which has two 
				// functions of interest they are... requestFString(String, String) and
				// returnRequest(), this xmlwriter is a versatile class as it is used to 
				// generate xml requests for various purposes... SEARCH,UPLOAD, DOWNLOAD 
				// and SHOWFILES... therefore accordingly the requestFString takes two 
				// parameters in case of SHOWFILES the second parameters is not used.

				XmlWriter writer = new XmlWriter();	// Initializing a object of xmlwriter..
				writer.requestFString("SEARCH",directory+condition);// calling the 
																	// function...
				String file_data = writer.returnRequest();	// getting the request in a 
															// temp variable file_data.
					
				byte file_bytes[] = file_data.getBytes();	// getting byte array of string
				
				// An output stream is also initialized this is used to store all the 
				// response from the listener..	
				
							
				BufferedOutputStream out_file = new BufferedOutputStream(new FileOutputStream("response.xml"));
 
				int file_size = file_bytes.length;

				
				byte b[] = new byte[1024];		// Initializing a new byte array of 1024.
				
				// another method of the same class add_on is used now (apporpriatelength) 
				// this is  used so as to make the request sent by the client to the 
				// listener 1024 in length..(for c#) listener's...

				b = appropriatelength(file_bytes, file_size);
			
				out.write(b,0,1024);	// The byte array is written on the output stream

				int y = 0;
				byte f[] = new byte[32];

				while ((y = br_socket.read(f,0,32))>0)	// the socket input stream is read
				{
				out_file.write(f,0,y);	// written on to the file output stream...		
				}

				out.close();		//
				br_socket.close();	//   The filestream and socket streams are
				out_file.close();	//
				
				// A class SParser is also used here this class has a function/method of 
				// the name perform which calls the xml parser to parse the xml file
				// generated by the response from the client soket...
				
				// the function perform returns a Vector which has the files/directories, 
				// along with their flag information and size in case of files....

				SParser sp = new SParser();
				Vector parameters = sp.perform("response.xml");
				file_gui showfiles = new file_gui(parameters,ip_address,directory,flag_info);
				showfiles.show();
			}
			catch (Exception e)
			{
					System.out.println( "Exception in search_request "+e );
					
			}

			try
			{
			  file_socket.close();	// Close the Socket...
			}
			catch (Exception e)
			{
  			 System.out.println( "Some Error while Closing : "+e );
			}

		}		// End Search request....


		/*  This is a simple sorting function which implements bubble sort method to 
		    sort the Listener's name lexicographically...
			It takes as it's argument a 2-D array of the listener's name and a flag
			to call
		*/

		public String [][] sorting(String info[][], boolean cond)
		{
			int  k = 0;
			while (k < (info.length-1))
			{
				int i = 0;
				while (i <  (info.length - k - 1)) 
				{
				  if((info[i][0] != null)	&&(info[i+1][0] != null))
				 {
				 if (info[i][0].compareToIgnoreCase(info[i+1][0]) > 0)
				  {
					String temp = info[i][0];
					info[i][0] = info[i+1][0];	// Swapping operation..
					info[i+1][0] = temp;

					temp = info[i][1];
					info[i][1] = info[i+1][1];	// Swapping operation..
					info[i+1][1] = temp;

					if (cond)
					{
					 temp = info[i][2];
					 info[i][2] = info[i+1][2];	// Swapping operation..
					 info[i+1][2] = temp;
					}

				  }	
				 }
					i++;
				}
					
				k++;
			}

			return info;		// Returning the 2-D sorted array...
		}

	/* This function/method is used everytime an object of class Client is invoked
	   it is used to get Listener's List, Search List from the server...After retriving 
	   the information it arranges the information in proper fromat in a 2_D array and 
	   returns the array to the class which invoked the function...
	 
		The parameters this method takes are..
		1. param -- Criteria for calling the function (Search/Root).
		2. us -- if param equals Search then this will hold the Search Criteria..
		3. fs -- if param equals Search then this will hold the Search Criteria..
		4. present_users[][] -- Present user's connected to the server...
	 
	 */  
	public String[][] start_connection(String param,String us, String fs, String present_users[][])
	{
		URLConnection urlconnection;
		InputStream url_inputstream;	// For Connecting and gaining the information..
		String information[][] = {{ " "," "}};
		Vector values = new Vector();
		boolean go_on = true;
		boolean search_flag = false;
		
		
	try
		{
		if (param.equalsIgnoreCase("search")) // if Search 
		{
			// Call The ASP with proper format and parameters and initialize a vector to 
			// store the information generated from this request..  
			urlconnection = (new URL("http://SERVERNAME/search.asp?us="+us+"&fs="+fs)).openConnection();
		    urlconnection.connect();
			search_flag= true;
			filenames = new Vector();
			filenames.add(0,(Object)"filesearch");
		
		}
		else	// if root
		{
		urlconnection = (new URL("http://SERVERNAME/userlist.asp")).openConnection();
		urlconnection.connect();
		search_flag= false;
		}
		StringBuffer sb = new StringBuffer();
		try
		{
		url_inputstream = urlconnection.getInputStream();	// get the inputstream

		// read the response from the request and store it in the response.xml file...
		BufferedReader br = new BufferedReader(new InputStreamReader(url_inputstream));
		
		BufferedWriter file_output = new BufferedWriter(new OutputStreamWriter(new FileOutputStream("response.xml")),32);
			
		int i = 0;
		char[] b = new char[32];
		String string = "";
		while ((i = br.read(b,0,32)) > 0 )
		{
	   	String temp = new String(b,0,i);
		string = string +temp;
		}
		string = string.trim();
		char d[] = string.toCharArray(); 
		file_output.write(d,0,d.length);

		br.close();		// Close the inputStream..
		file_output.close();	// Close the inputStream..
	
		}

		catch(Exception ef)
		{
		}
		// A class SParser is also used here this class has a function/method of 
		// the name perform which calls the xml parser to parse the xml file
		// generated by the response from the client soket...
				
		// the function perform returns a Vector which has the files/directories, 
    	// along with their flag information and size in case of files....


		SParser sp = new SParser();
		values = sp.perform("response.xml");	
			
		
		if (param.equalsIgnoreCase("search")) // if the param is search then  
		{									  // storing is done in the 
											  // following format..
			
			information = new String[present_users.length][2];
			viewfiles = new String[values.size()][3];
			int i = 1;
			int g = 0;
			int count = 0;
						
			while(i<(values.size()))
			{
				viewfiles[g][0] = (String)values.get(i); 			
				i++;
				viewfiles[g][1] = (String)values.get(i);
				i++;
				viewfiles[g][2] = (String)values.get(i);
				i++;
				String temp= "";
				temp = viewfiles[g][0]+"?"+viewfiles[g][2]+"?";
				
				if (viewfiles[g][2].substring(viewfiles[g][2].lastIndexOf("\\")+1).equals(""))
				{
					temp = temp+"0"+"?";
				}
				else
				{
					temp = temp + "0"+"?"+"  "+"?";		
				}
				filenames.add((Object)temp);
				temp = "";
				g++;
			}
		
			for (int index = 0;index<present_users.length ;index++ )
			{
				if (present_users[index][0] != null)
				{
				  int inf  = present_users[index][0].indexOf("(");
				  if (inf != -1)
				   {
					present_users[index][0] = present_users[index][0].substring(0,inf);
				   }
				  
				}
			}
			
			
			for (int index = 0;index<present_users.length ;index++ )
			{
				if (present_users[index][0] != null)
				{
				 count = 0;
				for (int temp = 0;temp<g ;temp++ )
				{
					if (viewfiles[temp][1].length()>=present_users[index][0].length())
					{
						if (present_users[index][0].equalsIgnoreCase(viewfiles[temp][1].substring(0,present_users[index][0].length())))
						{
							count++;
						}
					}
					
				}
				present_users[index][0] = present_users[index][0] + "( "+count+" )";
				}
								
			}

			information = present_users;
			
			
		}
		else
		{
		  if (values.size()>1)
		  {
			information = new String[values.size()][2];
			int i = 1;
			int g = 0;
			while(i<(values.size()))
			{
				information[g][0] = (String)values.get(i); 			
				i++;
				information[g][1] = (String)values.get(i);
				i++;
				g++;
			}

		  }
		  else
		   go_on = false;
	
		}
		
		 
		   if (!go_on)
		   {
		     information = new String[1][2];
	  	     information[0][0] = "Not Connected...";
			 information[0][1] = " - ";
		   }
		   else
		   {
			information = sorting(information,false);  // Sorting is performed,,,
		   }
		}		// End Try..
		catch(Exception e)
		{
		   go_on = false;
		}

					// End Else....

					

		return (information);		// return the sorted list...
	
	}

	public Vector returnfilenames()
	{
		
	return filenames;
	}
}
