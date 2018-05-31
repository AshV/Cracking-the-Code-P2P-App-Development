import javax.swing.*;
import javax.swing.event.*;
import java.awt.event.*;
import java.awt.*;
import java.net.*;
import java.io.*;
import javax.swing.table.*;
import java.util.*;

/* When the Client selects a particular Listener's name from the list of the user's 
   connected and presses the button Open then this class is called. This class helps 
   the client directly connects to the Listener's machine and browse through, download, 
   upload, search for the files/folders shared by that particular Listener....
   
   This class provides the client facilities for download, upload, search for files,move 
   through shared folders facility...
*/


public class file_gui extends JFrame implements ActionListener
{
	private JButton file_open;				//
	private JButton file_search;			//	
	private JButton file_close;				//   
	private JButton file_upload;			//	 GUI Components for various purposes..
	private JButton file_download;			//
	private JTable file_listing;			//
	private JScrollPane file_scroller;		//
	private JLabel status;					//
		
	Container contentpane;					//  For the placeing the GUI Componments
	private Socket file_socket;				//  To Declare the client socket...
	Vector values = new Vector();			//  To declare a vector "values" for holding 
										    //	the results of the xml response from the 
										    //  the listener...
	
	TableModel default_table;				//	For JTable GUI Component
	String names[] = {"Files / Folders..", "Size", "Type"};// Name of the Columns in JTable
	Object data[][] ={{" "," "," " },{" ", " "," "},{" ", " "," "},{" ", " "," "},{" ", " "," "},{" ", " "," "}};// Initializing the JTable Columns..
	String ip_address;								// Variable to store the ip_address.
	String information[][];							
	String file_name= new String();
	String status_text;							// Variable to store the root information
	String flag_info;							// Variable to store the flag information
	boolean done =  false;

	/* Constructor is called with 4 parameters they are as follows...
	   1.  This is the result of XML parsing done (and stored in a vector) at the class
		   files which calls this class.
	   2.  The IP address of the Listener to which the client wants to connect...
	   3.  The root information (ie . the directory in which the user is currently 
		   browsing..). initially this information is kept as "ROOT".
	   4.  The flag Information about the directory in which the user5 is currently
		   browsing,this is used for upload purposes.. initially this is kept as 0 as 0 
		   stands for no uploads and client cannot upload on listener's root directory..	
    */
		   

	public file_gui(Vector parameter, String ip ,String stat_text, String flag_info)
	{
	 values = parameter;		// 
	 ip_address = ip;			//		Initializing the variables with parameters...
	 status_text = stat_text;	//	
	 this.flag_info = flag_info; // 	
	
        setTitle ("Peer 2 Peer Client");
		setSize(475,405);				// Set The size of the frame...
		
		contentpane = getContentPane();	//  Initialize the window for placing the 
										//  components..
		contentpane.setLayout(null);	// Setting the Layout to Absolute Layout..
		
		file_open = new JButton("Open");	// Initializing the GUI Component.
		file_open.setMnemonic('O');			// Setting the Mnemonic..
		file_open.setBounds(20,20,80,35);	// Positioning the GUI Component. 
		
		file_upload = new JButton("Upload");	// Initializing the GUI Component.
		file_upload.setMnemonic('U');			// Setting the Mnemonic..
		file_upload.setBounds(100,20,80,35);	// Positioning the GUI Component. 

		file_download = new JButton("Download");	// Initializing the GUI Component.
		file_download.setMnemonic('D');				// Setting the Mnemonic..
		file_download.setBounds(180,20,100,35);		// Positioning the GUI Component. 

		file_search = new JButton("Search");	// Initializing the GUI Component.
		file_search.setMnemonic('S');			// Setting the Mnemonic..
		file_search.setBounds(280,20,80,35);	// Positioning the GUI Component. 

		file_close = new JButton("Close");		// Initializing the GUI Component.
		file_close.setMnemonic('C');			// Setting the Mnemonic..
		file_close.setBounds(360,20,80,35);		// Positioning the GUI Component. 

		status = new JLabel(status_text);		// Initializing the GUI Component.
		status.setBounds(10,355,300,25);		// Positioning the GUI Component. 

		//	Initializing the Table
		
		default_table = new AbstractTableModel()
		{
        // These methods always need to be implemented.
        public int getColumnCount() { return names.length; }
	    public int getRowCount() { return data.length;}
        public Object getValueAt(int row, int col) {return data[row][col];}

	     // The default implementations of these methods in
         // AbstractTableModel would work, but we can refine them.
        public String getColumnName(int column) {return names[column];}
        public Class getColumnClass(int col) {return getValueAt(0,col).getClass();}
        public boolean isCellEditable(int row, int col) {return (col==4);}
        public void setValueAt(Object aValue, int row, int column) {
                data[row][column] = aValue;
				fireTableCellUpdated(row, column);
            }
         };

		/* This condition is applied so as to disable all the buttons except the close 
		   button when view files button is pressed as while viewing the files the client 
		   cannot make downloads/uploads etc...
        */

		 if (stat_text.equalsIgnoreCase("SEARCH RESULTS"))		
		 {
			file_open.setEnabled(false);
			file_download.setEnabled(false);
			file_upload.setEnabled(false);
			file_search.setEnabled(false);
		 }
			 
		contentpane.add(file_open);				//
		contentpane.add(file_close);			//
		contentpane.add(file_download);			//
		contentpane.add(file_upload);			//  Adding the GUI Components...
		contentpane.add(file_search);			//
		contentpane.add(status);				//
			
		/* An important function "formating" is called with the parameter a vector value
		   this helps in extracting the information stored in the vector and placing them 
		   in a string 2-Dimensional array in proper format for later reference also 
		   placing the information on to the JTable...
		*/   
	
		formating(values);
		
		// Positioning and Initializing the GUI Component (Table)... 

		file_listing = new JTable(default_table);
		file_listing.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
		//file_listing.getTableHeader().setReorderingAllowed(false);  
        //file_listing.setBounds(10,55,650,300);
		file_listing.setGridColor(new Color(255,255,255));
		//file_listing.setPreferredScrollableViewportSize(new Dimension(500, 70));

		// Adding Scroll Facility to the JTable by adding JScrollpane Component
			 
		file_scroller = new JScrollPane();//file_listing);
		file_scroller.setBounds(10,55,450,300);
		file_scroller.setViewportView(file_listing);
		file_scroller.setHorizontalScrollBarPolicy(                      JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
		file_listing.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS); 
      
		contentpane.add(file_scroller);		// Add the Scroll Bar...

		// Adding the window  Listener to the gui window
		// ie. Code for the "cross"...

		addWindowListener (new java.awt.event.WindowAdapter () {
        public void windowClosing (java.awt.event.WindowEvent evt) {
		   setVisible(false);
	 	}
        }
        );

		file_open.addActionListener(this);			//
		file_upload.addActionListener(this);		//	
		file_download.addActionListener(this);		//	Add the  ActionListener...
		file_search.addActionListener(this);		//	
		file_close.addActionListener(this);			//
	
	}			// End of Constructor file_gui...


// The part Below pertains to the Action Performed when a Button is pressed...

public void actionPerformed(ActionEvent ae)
  {
	if (ae.getSource() == file_open )	// When open Button is pressed...
	 {
		  /* This button will funbction only in case when the user selects a folder and 
			 opens it for knowing the details of the folder.. If the client tries to open 
			 a file a MessageBox is shown....
          */
		  
		  try
		  {
			 int row = 0;						
			 row = file_listing.getSelectedRow();		// Get the Selection...
											
			 if (information[row][2].equalsIgnoreCase("Folder")) // Check whether Folder
			 {
			   /* If a folder is selected and then pressed the open button then an object
			      of the class add_on is created (request) This is responsible for sending
				  the request to the Listener by using a function of the class of the name
				  "search_request". The parameters passed on to the function are the folder
				  name, followed by the search criteria here in this case it is *.*, foll-
				  -owed by the ip_address of the listener, followed by the flag 
				  information of the folder searched for...
			   */	  
			   add_on open_request = new add_on();
			   open_request.search_request(information[row][0],"*.*",ip_address,information[row][3]);	
			 }
			 else		// If a file is selected...
			 {
				// Alert / Message Box is displayed... 
				JOptionPane.showMessageDialog(this,"Cannot Open a File Over Network. Try Downloading it.  ","Peer 2 Peer...",JOptionPane.INFORMATION_MESSAGE);
			 }
	      }
		  catch(Exception es)
		  {}
	 }			// End File Open ....
	else if (ae.getSource() == file_close)	// If close button is pressed		
	 {
		this.setVisible(false);		// Hide this frame...	
	 }
	else if (ae.getSource() == file_download)	// If Download button is pressed...
	 {
		/* When download button is pressed a class called SwingWorker provided by Sun is
		   called this class helps in running a task in seperate thread thus helps in gui.
        */
		 final SwingWorker worker = new SwingWorker()
		  {
	 		public Object construct() 
			{
				// The function of the class is given a task to perform (downloading).
				// By calling a class donload_file..
				// This downloading is done in a seperate thread...
			   return new download_file();
			}
		  };
		 worker.start();	// Calling the start of the swingworker
								
	 }
	else if (ae.getSource() == file_upload)
	 {
	  	 done = false;
		 /* When Upload button is pressed a class called SwingWorker provided by Sun is
		   called this class helps in running a task in seperate thread thus helps in gui.
        */
		 final SwingWorker upload = new SwingWorker() 
		 {
			public Object construct()
			{
				// The function of the class is given a task to perform (Uploading).
				// By calling a class upload_file..
				// This uploading is done in a seperate thread...
			   return new upload_file();
			}
		 };
		upload.start();		// Calling the start of the swingworker
		if (done)
		 {
			this.setVisible(false);		// Hide the window after the upload...
		 }
	 } // End Upload.....
	else if (ae.getSource() == file_search)	// When Search Button is pressed...
	 {
		/* When search button is pressed by the client then root information is checked
		   if root information is is "ROOT" then a message box is shown indicating that
		   no search can be done...
		   else a class search screen is called... which caters to the search ...
		 */

		if (status_text.equalsIgnoreCase("Root"))	// If status_text is "ROOT"
		{
			JOptionPane.showMessageDialog(this,"Cannot Search on Root. Try searching in sub directories .  ","Peer 2 Peer...",JOptionPane.INFORMATION_MESSAGE);
		}
		else	// If search Text is not root...
		{
			/* Create an Object of the class search_screen ..
				the parameters passed on to the constructor are...
				1. Status_text...
				2. the ip address of the listener...
				3. The flag_info of the folder on which search is being performed...
			*/
			search_screen search_now = new search_screen(status_text,ip_address,flag_info);
			search_now.show();  // Show the frame...
		}
	 }
		

}			// End of Action Listener....

	

	/* An important function "formating" is called with the parameter a vector value
		   this helps in extracting the information stored in the vector and placing them 
		   in a string 2-Dimensional array in proper format for later reference also 
		   placing the information on to the JTable...
		*/   

 void formating(Vector values)
  {
		// To check whether the vector has more than one value or not.

   if (values.size() > 1)
	{
		int array_size = values.size();
					
		information = new String[array_size][4]; // Array in to which the information 
												 //  extracted is added... 
		// Information to be placed on the Table is put into the array (2-D) data...

		if (array_size > 19)
		{
		data = new Object[array_size][3];
		}
		else
		{
		data = new Object[19][3];			// Minimum size of array...
		}
		boolean file_or_Folderectory; 

		int i = 1;
		int g = 0;
	  while(i<array_size)
	  {
	   try
	   {
		   String temp = (String)values.get(i); 	
		  
		   file_name = temp.substring(temp.lastIndexOf("\\")+1);
		  if (file_name.equals(""))
		  {
			information[g][0] = (String)values.get(i); 			
	  	    data[g][0] = values.get(i);
			information[g][2] = "Folder";
			data[g][2] = "Folder";
			i++;
			information[g][3]  = (String)values.get(i); 			
			i++;
			data[g][1] = (Object)"  -  ";

		  }
		  else
		  {
			information[g][0] = (String)values.get(i);
			int index =file_name.lastIndexOf(".");
			String gh = "";
			if (index == -1)
			{
			information[g][2] = "";
			data[g][2] = information[g][2]+" File";
			gh = file_name;
			}
			else
			{
			information[g][2] = file_name.substring(file_name.lastIndexOf(".")+1);
			data[g][2] = information[g][2]+" File";
			gh = file_name.substring(0,index);
			}
			data[g][0] = gh;
			information[g][2] = file_name.substring(file_name.lastIndexOf(".")+1);
			data[g][2] = information[g][2]+" File";
			i++;
			information[g][3]  = (String)values.get(i); 			
			i++;
			information[g][1] = (String)values.get(i);
			data[g][1] = values.get(i);
			i++;			
		
		  }
		  
	   }
	   catch (Exception e)
	   {
	    System.out.println( e );
	   }
 		 g++;
		 file_name = "";
    }		// End While.....

  }		//  end if..
  else
   {
		 information = new String[1][3];
		 information[0][0] = "No Files are shareable";
		 information[0][1] = " ";
		 information[0][2] = " ";

		data[0][0] = (Object)"No Files are shareable";
		data[0][1] =(Object)" - ";
		data[0][2] =(Object)" - ";

   }		
}

/*  This class is used to download the file from the listener on to the Client's 
	machine....
 */ 

   public class download_file extends JFrame
	{
	   download_file()		// Constructor...
	 	{
		try
		 {
			int row = 0;
			row = file_listing.getSelectedRow();	// Get the selection of the user...	
			
			InetAddress inet = InetAddress.getByName(ip_address);
			file_socket = new Socket(inet,7070);	// Establish a socket connection with 
													// the Listener on the port 7070
													// address -- inet..
			// Get The output as well as the input Streams on that socket...
			
			BufferedOutputStream out = new BufferedOutputStream(file_socket.getOutputStream());
		
			BufferedInputStream br_socket = new BufferedInputStream(file_socket.getInputStream());

			// if the selection is a folder... then pop up  a message for denial of 
			// download..				
			if (information[row][2].equalsIgnoreCase("Folder"))
			{
			JOptionPane.showMessageDialog(this,"Cannot Download a Folder. Try Opening it.  ","Peer 2 Peer...",JOptionPane.INFORMATION_MESSAGE);
			}
			else	 // If the request is that of a file..
			{
			XmlWriter writer = new XmlWriter();  // Call a class XMLWRITER to generate 
												 //  request by using a function
			writer.requestFString("DOWNLOAD",information[row][0]); // requestFString...
			String file_data = writer.returnRequest();
			byte file_bytes[] = file_data.getBytes();	// get the Number of bytes from the
														// request...
			String temporary =information[row][0].substring(information[row][0].lastIndexOf("\\")+1); 

			 JFileChooser jfc = new JFileChooser();		// Call an object of JFileChooser 
													 	// File Dialog to place thefile.	
				 
			 File file = new File (temporary);
			 jfc.setSelectedFile(file);
			 jfc.ensureFileIsVisible(file);
			 int button_pressed = jfc.showSaveDialog(this);
			 String str1 = "";
			 File file_final = jfc.getSelectedFile();	
		     if (button_pressed == JFileChooser.APPROVE_OPTION)
			 {
				str1 = file_final.getPath();// Get the path where the file is being saved.. 
			 } 
				
			BufferedOutputStream out_file = new BufferedOutputStream(new FileOutputStream(str1)); // Create an outputstream to that path...
 
			int file_size = file_bytes.length;
												// Adjust the request length to 1024 
												// (for c#) listener's...
			byte b[] = new byte[1024];
			// another method of the class add_on is used now (apporpriatelength) this 
			// is  used so as to make the request sent by the client to the listener 
			// 1024 in length..(for c#) listener's...

			// The methos takes a byte array and it's length as parameters and return 
			// a byte array of length 1024 bytes....

			add_on download = new add_on();		 
			b = download.appropriatelength(file_bytes, file_size);
		
			out.write(b,0,1024);  // The byte array is written on the output stream

			int y = 0;
			byte f[] = new byte[32];
			int file1_size = Integer.parseInt(information[row][1]);
			// Generate a progress monitor to monitor the request...
			ProgressMonitor pm = new ProgressMonitor(this,"Downloading File..","Downloading Please Wait...",0,file1_size);
			int current = 0;
			pm.setMillisToPopup(5);

			while ((y = br_socket.read(f,0,32))>0) // Read the socket and write on to the 
												  //  file...
			{
			out_file.write(f,0,y);
			current = current + y;
			pm.setProgress(current);		// Monitoring the progress of the progress 	
											// monitor.,..
			}

			out.close();				//
			br_socket.close();			// The filestream and socket streams are
			out_file.close();			// Closed
				
		  }
	  }
	  catch(Exception es)
	  {
	  }
	  try
	  {
	  file_socket.close();			// Close the Client_socket...
	  }
	  catch (Exception e)
	  {
		System.out.println( "Some Error while Closing : "+e );
 	  }
    }		// End Constructor..
 }			// End Class Download_file...


/*  This class is used to upload the file from the Client on to the Listener's 
	machine....
 */ 

  public class upload_file extends JFrame
   {
	  upload_file()		// Constructor...
	  {
	 	String str1 ="";
		Vector parameters = new Vector();
		try
	  	 {
			InetAddress inet = InetAddress.getByName(ip_address);
			file_socket = new Socket(inet,7070);// Establish a socket connection with 
													// the Listener on the port 7070
													// address -- inet..
			// Get The output as well as the input Streams on that socket...
			
			BufferedOutputStream out = new BufferedOutputStream(file_socket.getOutputStream());
		
			BufferedInputStream br_socket = new	 BufferedInputStream(file_socket.getInputStream());
			
			// if the upload is in a root... then pop up  a message for denial of 
			// download..
		
			// if the flag is a 0... then pop up  a message for denial of 
			// download..	

			if (status_text.equalsIgnoreCase("ROOT"))
			{
			  JOptionPane.showMessageDialog(this,"Cannot Upload In Root. Try sub Folders .  ","Peer 2 Peer...",JOptionPane.INFORMATION_MESSAGE);
			}
			else if (flag_info.equals("0"))
			{
 			  JOptionPane.showMessageDialog(this,"Cannot Upload In Read Only Folder . Try other Folders .  ","Peer 2 Peer...",JOptionPane.INFORMATION_MESSAGE);
			}
			else		// Else if flag = 1
			{
  			 JFileChooser jfc = new JFileChooser();	// Call an object of JFileChooser 
													// File Dialog to choose the file.
				 
		 	 int button_pressed = jfc.showOpenDialog(this);
			
			 File file_final = jfc.getSelectedFile();	
			 if (button_pressed == JFileChooser.APPROVE_OPTION)
			  {
			   str1 = file_final.getPath();		// Get the path of the file
			  } 
					
		 	String temp = str1.substring(str1.lastIndexOf("\\")+1);
			temp = status_text+temp;
					
			XmlWriter writer = new XmlWriter();		// Call a class XMLWRITER to generate 
												 //  request by using a function
			writer.requestFString("UPLOAD",temp);		// requestFString...
			String file_data = writer.returnRequest();

			byte file_bytes[] = file_data.getBytes();   // get the  bytes from the
														// request...
			
			BufferedInputStream file_read = new BufferedInputStream(new 	FileInputStream(str1));		// Create an inputstream to that path...
					
			int upload_file_size = file_read.available();
			int file_size = file_bytes.length;

			byte b[] = new byte[1024];		
				// Adjust the request length to 1024 
				// (for c#) listener's...
			
			// another method of the class add_on is used now (apporpriatelength) this 
			// is  used so as to make the request sent by the client to the listener 
			// 1024 in length..(for c#) listener's...

			// The methos takes a byte array and it's length as parameters and return 
			// a byte array of length 1024 bytes....

			add_on upload = new add_on();
			b = upload.appropriatelength(file_bytes, file_size);
			
			out.write(b,0,1024);	  // The byte array is written on the output stream

			int y = 0;
			byte f[] = new byte[1024];
			// Generate a progress monitor to monitor the request...
			
			ProgressMonitor pm = new ProgressMonitor(this,"Uploading File..","Uploading Please Wait...",0,upload_file_size);
			int current = 0;
			//pm.setMillisToPopup(5);


			while ((y = file_read.read(f,0,1024))>0)// Read the file and write on socket
			 {
			  out.write(f,0,y);
			  current = current + y ;
			  pm.setProgress(current);		// Monitor the current activity...
			 }

			out.close();
			br_socket.close();			// Close the streams...
		}
				
	}
	catch(Exception es)
	{}
	try
	 {
	   file_socket.close();			// Close the socket...
	 }
	catch (Exception e)
	 {
	  System.out.println( "Some Error while Closing : "+e );
	 }
						
    }		// End Constructor....
  }		// END UPLOAD FILE...

}		// End File_Gui...
