import java.io.*;
import java.net.*;
import java.util.*;

/*  This Class is the listener class of the application. This class handles the request from clients 
 *	the class extends Thread class because any new request runs in a new thread .
 */
class  OneServer extends Thread {
		
		// Make object for client socket
		private Socket socket;
		
		// Make object for tokenizer
        private StringTokenizer st_xml;
		
		// Make object for parser
		static  XMLParserServer sp;
		private BufferedOutputStream data_write;
		
		// Return files and directroy names.Return the file size and rights of the files 
		String[][] returnvalueoffiles;
		
		// Vectro Declared
		static Vector v_file_name;
		//This class is used to write xmldata
		private XmlWriter xmlwriter;
		
		private check_directory check;
		
		//Declearing BufferedInputStream
		private BufferedInputStream in;
		
		// Declearing BufferedOutputStream
		private BufferedOutputStream out;
		
		// This String will contain the FileName
		String left_half;
		
		// Store right part of the file it means after "." 
		String right_half;
		
		// Store detail path name of file & directies in file object
		File path_file;
		
		// Store files and directries names in string array
		String[] files;
		
		/* when search any word if any charecter present of left of file  extension then  in that 
		* condition this flag become true */
		boolean left_half_flag = false;

		/* when search any word if any charecter present of right of file  extension then  in that 
		* condition this flag become true */
		boolean right_half_flag = false;
		
		// file and directroy store in this variable 
		String filename;
		
		// file size store in this variable 
		String filesize;

		// file rigts store in this variable 
		String filemask;
		
		String stemp="";
		
	/* from this fuction listener take request from client and according to there
	*  request listener give the response */
	public OneServer(Socket soc) throws IOException	
			{
				
				// store the socket value which is connected by user		
				socket = soc;
				
				// take the request from the user in the form of BufferInputStream 
				in = new BufferedInputStream (socket.getInputStream());
				
				// give the response to the user from listener in form of BufferOutputStream 
				out = new BufferedOutputStream(socket.getOutputStream());

				// start the new thread for new user
				start();
			}


	// when any new user connect then this fuction call by start() 
	public void run() 
			{
		try
			{
			int i = 0;
			String value;
			byte [] a = new byte[1024];
			
			// open main.xml file as BufferedOutputStream for writting data into it 
			data_write = new BufferedOutputStream(
				(new FileOutputStream("main.xml")));
			
			// read data from user DataInputstream
			in.read(a,0,1024);
			
			// Store reading value in temp String
			String temp = new String(a);
			System.out.println(temp);
			// Break the temp till last ">" + 1 value
			temp = temp.substring(0,(temp.lastIndexOf(">")+1));
			
			// convert temp string value in Byte
			byte d[] = temp.getBytes(); 
			
			// write converted value in "main.xml" file 
		   	data_write.write(d, 0, d.length);
			
			// Close the date_write stream
			data_write.close();
			
			// Make the object of XmlWriter class
			xmlwriter = new XmlWriter();
			
			// Make the object of XMLParserServer class
			sp = new XMLParserServer();
			
			// call the function of class XMLParserServer and pass the argument as  string 
			sp.perform("main.xml");
		
			// This function return the file names as vector 
			v_file_name = sp.yakreturn();

		// This for loop run the size of the vector
		for(int t = 0; t < v_file_name.size(); t++)
			{
				

		// vector value store is showfiles type then enter in this condition
		if((v_file_name.get(t).toString()).equalsIgnoreCase("SHOWFILES"))
			{
				String s = "";
		
				// open stream 	of "Share.ini" file for reading
				BufferedReader data_read = new 
				BufferedReader(new FileReader("Share.ini"));

		// This while loop run till stream "data_read" not become null
		while ((s = data_read.readLine()) != null)
			{
                String filename = "";
                String filesize = "";
                String mask = "";
				
				// divide string "s" on the basis of "=" and store in st_xml
				st_xml = new StringTokenizer(s, "=");

				// This while loop run till that tokenizer present in st_xml
				while(st_xml.hasMoreTokens())
                {
		
						// Here store first token in variable filename
						filename = st_xml.nextToken();

						/* This filename string end with "\" then enter in this part otherwise enter 
						 * in else part. if filename string end with "\" it means that is directroy there
						 * oterwise that is file */
				        if( !filename.endsWith("\\") )
							{
								
								// Here store second token in variable mask. it is rights of file 
				                mask = st_xml.nextToken();
								
								// Here store thired token in variable filesize. it is writes of file 
						        filesize = st_xml.nextToken();                        
								
								/* Call the returnHeader function from XmlWriter class this fuction return
						    	 * the header of xml file as string and store this value in stemp variable. */
								stemp = xmlwriter.returnHeader(v_file_name.get(t).toString());
								
								/* Call the responseFString function from XmlWriter class this fuction 
								 * writes the xml file for files. */
						        xmlwriter.responseFString(v_file_name.get(t).toString(), filename, filesize, mask);                             
							}
						else
							{
								
								// Here store second token in variable mask. it is rights of file 
						        mask = st_xml.nextToken();
								
								/* Call the returnHeader function from XmlWriter class this fuction return
						    	 * the header of xml file as string and store this value in stemp variable. */
								stemp = xmlwriter.returnHeader(v_file_name.get(t).toString());
								
								/* Call the responseFString function from XmlWriter class this fuction 
								 * writes the xml file for Directroy. */
						        xmlwriter.responseFString(v_file_name.get(t).toString(), filename, "", mask);                             
							}
		           }


			}
			 
			 /* Call the returnResponse function this function return whole xml except header of xml as 
			  * string.Store this value in wholexmlwithoutheader veriable */
			 String wholexmlwithoutheader = xmlwriter.returnResponse();
			
			 /* Add two string veriable and store in any third string variable. This variable store whole
			  * xml file */
			 wholexmlwithoutheader = stemp+wholexmlwithoutheader;
			 System.out.println(wholexmlwithoutheader);
			 // Find the length of xml file and send 0 to length of file xml file bytes to user
 			 out.write(wholexmlwithoutheader.getBytes(),0,wholexmlwithoutheader.length());
			 
			 // Close the data_read stream which read from file.
 			 data_read.close();

			 // Close the out stream which connected to user.
			 out.close();
			}

			// In this condition we do all download work related to user request
			else if((v_file_name.get(t).toString()).equalsIgnoreCase("DOWNLOAD"))
				{
					
					// Store the file name in veriable f_name. This file down loaded by user
					String f_name = v_file_name.get(1).toString();
					
					// initilize the veriable of len
					int len = 0;

					// Open the file stream of stored file name which is present in f_name. 
					FileInputStream fstream = new FileInputStream(f_name);
			
					// Make variable c_write as Byte array which send to user
					byte[] c_write = new byte[32];

			 // While loop run upto 32 Byte of all stored arrey value		
			 while ((len = fstream.read(c_write,0,32))>0)
				{
				
					// Send the out stream to user every 32 Byte
					out.write(c_write, 0, len);
			    }
			
					// Close the out Stream
					out.close();

			}

			// In this condition we do all search work related to user request
			else if((v_file_name.get(t).toString()).equalsIgnoreCase("SEARCH"))
				{
					
					/* Make the object of check_directory which search file and diretory 
					 * which is requested by the user */
					check = new check_directory();
					
					// Store file & directory name with path in whole_String variable
					String whole_String = v_file_name.get(1).toString();
					
					// Store file & directory path in full_path
					String  full_path = whole_String.substring(0, (whole_String.lastIndexOf("\\") + 1));
					
					// Store file & directory name without path in word variable
					String  word = whole_String.substring((whole_String.lastIndexOf("\\") + 1 ));			
					
					// Make file object of file which path present in full_path
			        path_file = new File(full_path);
			    
					
					// Find the position of "." in file
					int dot_index = word.indexOf('.');

		/* When "." not present in file then return -1 then enter in this condition otherwise enter
		 * in else part */
		if (dot_index == -1)
		{

			// whole word value store in left_half variable
			left_half = word;

			// write_half variable value become blank(" ")
			right_half = " ";

			// Find the position of "*" in left_half variable and store that variable in asterix_index
			int asterix_index = left_half.indexOf("*");

			// if left_half variable not content any "*" then its return -1 then not enter in this condition
			if (asterix_index != -1)
			{
				/* Store value in left_half first position to "*" position when check the left_half_flag 
				 * flag to true */
				left_half = left_half.substring(0,asterix_index);
				left_half_flag = true;
			}
		}
		else
		{
			// Store file name begining to "." position left part
		    left_half = word.substring(0,word.indexOf('.'));

			// Store file name last to "." position of right part of that file 
	  	    right_half = word.substring(word.indexOf('.') + 1);

			// left_half is equal to "*" or left_half is equal to "" then enter in this condition
		    if ((left_half.equals("*"))||(left_half.equals("")))
			 {
				 // left_half string value insilize by null(" ")
				 left_half = " ";
			 }
			 else
			 {
				// Find the position of "*" in left_half variable and store that variable in asterix_index
				int asterix_index = left_half.indexOf("*");

			    // if left_half variable not content any "*" then its return -1 then not enter in this condition
				if (asterix_index != -1)
				{

					/* Store value in left_half first position to "*" position when check the left_half_flag 
					 * flag to true */
					left_half = left_half.substring(0,asterix_index);
					left_half_flag = true;
				}
			  }
		   // right_half is equal to "*" or right_half is equal to "" then enter in this condition
		   if ((right_half.equals("*"))||(right_half.equals("")))
			{
			    // right_half string value insilize by null(" ")
				right_half = " ";
			}
			else
			{
				// Find the position of "*" in right_half variable and store that variable in asterix_index
				int asterix_index = right_half.indexOf("*");

			    // if right_half variable not content any "*" then its return -1 then not enter in this condition
				if (asterix_index != -1)
				{
					/* Store value in right_half first position to "*" position when check the right_half_flag 
					 * flag to true */
					right_half = right_half.substring(0,asterix_index);
					right_half_flag = true;
				}

			}
		 }

		// Store files name which are present in this path_file in files array  
	    files = path_file.list();


		// make object of String array which contents files & directories name filesize and mask  
		returnvalueoffiles = new String[files.length + 1][3];

		// Store all values in returnvalueoffiles array which return by wild_card function
		returnvalueoffiles =	check.wild_card(left_half,right_half,left_half_flag,right_half_flag, path_file);


			for(int y = 0; y < files.length + 1; y++)
				{
						/* data in this array returnvalueoffiles[y][0] not present in this then break the 
						 * the loop other wise go to else part  */	
						if(returnvalueoffiles[y][0] == null)
						{
							break;
						}
						else
						{
						
						// Store full path	with file & directories name in filename
						filename = path_file + "\\" + returnvalueoffiles[y][0];


						// Store size of file 
						filesize = returnvalueoffiles[y][1];


						// Store the writes of the files
						filemask = returnvalueoffiles[y][2];


						/* Call the returnHeader function from XmlWriter class this fuction return
						 * the header of xml file as string and store this value in stemp variable. */
						stemp = xmlwriter.returnHeader("SHOWFILES");
						
						/* Call the responseFString function from XmlWriter class this fuction 
						 * writes the xml file for Directroy. */
						xmlwriter.responseFString("SHOWFILES", filename, filesize, filemask);                             
						}
					 }
						
						String wholexmlwithoutheader = "";

						/* data in this array returnvalueoffiles[0][0] not present in this then enter
						 * in this condition it means there is no file and diretory is there otherwise 
						 * go to else part of this condition*/
						 if(returnvalueoffiles[0][0] == null)
							 {
								// Make one xml file without any files & directries list 
								stemp = xmlwriter.returnHeader("SHOWFILES");
								wholexmlwithoutheader = "</response></p2p_lng>";
							 }
						 else
							 {
					    		/* Call the returnResponse function this function return whole xml except header 
								 * of xml as string.Store this value in wholexmlwithoutheader veriable */
								wholexmlwithoutheader = xmlwriter.returnResponse();
							 }
					
								 /* Add two string veriable and store in any third string variable. This variable store whole
								  * xml file */
								 wholexmlwithoutheader = stemp+wholexmlwithoutheader;
					
								 // Find the length of xml file and send 0 to length of file xml file bytes to user
								 out.write(wholexmlwithoutheader.getBytes(),0,wholexmlwithoutheader.length());
								 
					 			 // Close the data_read stream which read from file.
								 out.close();
			
				}


		// vector value store is upload type then enter in this condition
		else if((v_file_name.get(t).toString()).equalsIgnoreCase("UPLOAD"))
			{
					// Store file & directory name with path in whole_String variable
					String upload_name = v_file_name.get(1).toString();

					
					// initilize the veriable of len
					int len = 0;
					
					// Make variable c_write as Byte array which send to user
					byte[] c_write = new byte[32];
			
					// open stream 	of upload_name file for writing
					data_write = new BufferedOutputStream(
					(new FileOutputStream(upload_name)));
					
				 // While loop run upto 32 Byte of all stored arrey value		
				 while ((len = in.read(c_write,0,32))>0)
				{
					// Send the out stream to user every 32 Byte
					data_write.write(c_write, 0, len);
			    }
				
				// Close the date_write stream
				data_write.close();

			}

		}
	} 
	catch (IOException e)
	{
		System.out.println("Exception ocurred" + e);
	} 
	
	}

}


public class MultiServer 
	{
		
		// Here inisilige the PORT
		static final int PORT = 7070;

	MultiServer()
		{
		}

	void multiaccess() throws IOException
		{
			
			/* Create a object of server socket on this port any client can connect and they can send
			* there request */
			ServerSocket s = new ServerSocket(PORT);
			System.out.println("Server Started");
		try
		{
			while (true)
			{
				
				System.out.println("Connection open first");
				// Create  a new socket for every client
				Socket soc = s.accept();
				System.out.println("Connection open" +soc);
				
				try
				{
				
					// Call the OneServer class and pass the connected client socket 
					new OneServer(soc);
				}
				catch (Exception e)
				{
		
				// Close the created socket
				soc.close();
				}
			}
		}
		catch (Exception e)
		{}

	}		
}

