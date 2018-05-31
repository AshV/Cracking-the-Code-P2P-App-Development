import javax.swing.*;
import javax.swing.event.*;
import java.awt.event.*;
import java.awt.*;
import java.net.*;
import java.io.*;
import javax.swing.table.*;
import java.util.*;

/* This is the main Class: it caters to the GUI and the starting the Peer 2 Peer Client */ 

public class client extends JFrame implements ActionListener
{
	private JButton client_open;			//
	private JButton client_refresh;			//
	private JButton client_search;			//	
	private JButton client_quit;			//   GUI Components for various purposes..
	private JButton client_view_files;		//		
	private JTable client_listing;			//
	private JScrollPane client_scroller;	//
	int count = 0;
	Container contentpane;					//  For the placeing the GUI Componments 
	JLabel copy;

	TableModel default_table;				//	 
	String names[] = {"Users Connected"};	//	For JTable GUI Component
	Object data[][] ={{null},{null},{null},{null},{null},{null},					 {null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null},{null}};
	
	private URLConnection urlconnection;	//
	private InputStream url_inputstream;	// For Connecting and gaining the information..
	private Socket client_socket;
	
	boolean go_on = true;
	boolean check = true;

	Vector values = new Vector();
	
	
	String information[][];
	int g;
	add_on connection;

	/* The constructor for the Main Class Takes Four Arguments.....*/

	//	param ... For the state in which this class is called (Search / Other ).
	//	us ... Will come into use when param is in Search Mode..  
	//	fs ... Will come into use when param is in Search Mode..
	//	present_users ... Will come into use when param is in Search Mode..

	client(String param, String us, String fs, String present_users[][])
	{
	    
	    
        setTitle ("Peer 2 Peer Client...");
	
		contentpane = getContentPane();		
		contentpane.setLayout(null);		// Setting the Layout to Absolute Layout..
		
		copy = new JLabel("© www.dreamtechsoftware.com");
		copy.setBounds(20,5,200,15);
		contentpane.add(copy);
		
		client_open = new JButton("Open");	// Initializing the GUI Component.
		client_open.setMnemonic('O');		// Setting the Mnemonic..
		client_open.setBounds(20,20,80,35);	// Positioning the GUI Component. 
		
		client_refresh = new JButton("Refresh"); // Initializing the GUI Component.
		client_refresh.setMnemonic('R');		 // Setting the Mnemonic.. 		
		client_refresh.setBounds(100,20,80,35);	// Positioning the GUI Component. 

		client_search = new JButton("Search");	// Initializing the GUI Component.
		client_search.setMnemonic('S');			// Setting the Mnemonic..
		client_search.setBounds(180,20,80,35);	// Positioning the GUI Component. 

		client_view_files = new JButton("View Files"); // Initializing the GUI Component.
		client_view_files.setMnemonic('V');			   // Setting the Mnemonic..	
		client_view_files.setBounds(260,20,100,35);	  // Positioning the GUI Component. 
		client_view_files.setEnabled(false);

		client_quit = new JButton("Quit");	// Initializing the GUI Component.
		client_quit.setMnemonic('Q');		// Setting the Mnemonic..
		client_quit.setBounds(360,20,80,35);// Positioning the GUI Component. 	

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
			 
		// Positioning and Initializing the GUI Component (Table)... 

		client_listing = new JTable(default_table);
		client_listing.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
		client_listing.getTableHeader().setReorderingAllowed(false);  
        client_listing.setBounds(10,55,440,300);
		client_listing.setGridColor(new Color(255,255,255));
		client_listing.setBackground(new Color(255,255,255));

		// Adding Scroll Facility to the JTable by adding JScrollpane Component

			 
		client_scroller = new JScrollPane();
		client_scroller.setBounds(10,55,440,300);
		client_scroller.setViewportView(client_listing);// Adding the Table...	

		contentpane.add(client_scroller);	// Placeing the scroller on to the gui window..
		
		// The Rest of the GUI conponents will be placed on the window as and when needed.

		information = present_users;  // A String array(2 D) is initialized with the 
									  // present_users for reference..

		// The Class add_on is a muli utility class which has fuctions defined in it which
		// are used by many other classes in this peer 2 peer client project...

		 connection = new add_on();//Object Connection of class add_on is created.. 
		
		// A function start_connection of the class add_on is called... which performs 
		// 2 tasks on the basis of parameters passed on to it...

		//	param ... For the state in which this class is called (Search / Other ).
		//	us ... Will come into use when param is in Search Mode..  
		//	fs ... Will come into use when param is in Search Mode..
		//	present_users ... Will come into use when param is in Search Mode..
		
		information = connection.start_connection(param,us,fs,present_users);

		// The function returns a String 2 D Array which has the users list along with the
		// shared files (if the param was Search) and the ip address of the user..
		
		// Another functionm of this class is called which is responsible for placing the ]
		// information provided by the above function on to the Table...  
		
		place_info_table(information);
		
		// Now according to the parameters passed in the class the various GUI components
		// are enabled or disabled....

		if (param.equalsIgnoreCase("search"))
		{
		  client_view_files.setEnabled(true);	// View Files Button is Enabled...
		}

		// Adding the window  Listener to the gui window
		// ie. Code for the "cross"...
		addWindowListener (new java.awt.event.WindowAdapter () {
        public void windowClosing (java.awt.event.WindowEvent evt) {
        System.exit(0);
        }
        }
        );
						
		contentpane.add(client_open);		// 
		contentpane.add(client_refresh);	//
		contentpane.add(client_search);		//	Adding the GUI Buttons.....
		contentpane.add(client_view_files);	//
		contentpane.add(client_quit);		//
	
	}		// End of the constructor.......(client)...
	
	// The part Below pertains to the Action Performed when a Button is pressed...

	public void actionPerformed(ActionEvent ae)
	{
	 if (ae.getSource() == client_open)	// When open Button is pressed...
	 {
		  int row = 0;						// Temporary variable to get the index...
		  check = true;
	   	  row = client_listing.getSelectedRow();	// To get the index of the row which
													// is selected by the user...
		try
		{
		   if(information[row][1] != null)	// If the user index is not null...
			{
			  try
			  {
				InetAddress inet = InetAddress.getByName(information[row][1]);  
				client_socket = new Socket(inet,7070);	// Create a client_socket on the 
														// Listener's machine at port 7070. 
			
				// Get The output as well as the input Streams on that socket... 
				BufferedOutputStream out = new BufferedOutputStream(client_socket.getOutputStream());
			
				BufferedInputStream br_socket = new BufferedInputStream(client_socket.getInputStream());

				// Now a request is sent to the Listener to show all the shareable files 
				// of the particular user which was selected by the user..

				// To send a request a class of the name xmlwriter is used which has two 
				// functions of interest they are... requestFString(String, String) and
				// returnRequest(), this xmlwriter is a versatile class as it is used to 
				// generate xml requests for various purposes... SEARCH,UPLOAD, DOWNLOAD 
				// and SHOWFILES... therefore accordingly the requestFString takes two 
				// parameters in case of SHOWFILES the second parameters is not used.  

				XmlWriter writer = new XmlWriter();	// Initializing a object of xmlwriter..
				writer.requestFString("SHOWFILES"," "); //calling the function...
				String file_data = writer.returnRequest();	// getting the request in a 
															// temp variable file_data.

				byte file_bytes[] = file_data.getBytes();// getting byte array of string
			
				int file_size = file_bytes.length;	 // Getting the length of byte array
				
				byte b[] = new byte[1024];		// Initializing a new byte array of 1024.
				
				// another method of the class add_on is used now (apporpriatelength) this 
				// is  used so as to make the request sent by the client to the listener 
				// 1024 in length..(for c#) listener's...

				// The methos takes a byte array and it's length as parameters and return 
				// a byte array of length 1024 bytes....
				
				add_on upload = new add_on();
				b = upload.appropriatelength(file_bytes, file_size);
			
				out.write(b,0,1024);	// The byte array is written on the output stream

			
			// An output stream is also initialized this is used to store all the response
			// from the listener..	
			
				BufferedOutputStream out_file = new BufferedOutputStream(new FileOutputStream("response.xml"));

				int y = 0;					// Temporary variables....
				byte f[] = new byte[32];	// Temporary variables...

				while ((y = br_socket.read(f,0,32))>0)	// the socket input stream is read 
				 {
					out_file.write(f,0,y); 		// written on to the file output stream...
				 } 

				out.close();					//
				//br_file.close();				//   The filestream and socket streams are 
				br_socket.close();				//	 closed...
				out_file.close();				//
			
			  }									// End try..
		 	  catch(Exception e)				
			  {	
				client_socket = null;
				check = false;					// Check is made false...
			  }
			  
			  try												
			  {	
			     client_socket.close();			// Close the Client_socket...
			  }
			  catch (Exception e)
			  {
				 row = 0;
			  }
			
			  if (check)		// If the exception occurs then do not come here.... else  
		  	  {
				Vector parameters = new Vector();  // Temp Vector Declaration..
				
				// A class SParser is also used here this class has a function/method of 
				// the name perform which calls the xml parser to parse the xml file
				// generated by the response from the client soket...
				
				// the function perform returns a Vector which has the files/directories, 
				// along with their flag information and size in case of files....
				
				SParser sp = new SParser();
				parameters = sp.perform("response.xml");
				
				// The vector value returned by the xml parseris then passed as one of
				// the parameters to a class named file_gui this class is responsible for 
				// displaying GUI consisting of a table and some buttons along with the
				// root information and flag..

				// Initially since the class is called for the first time the parameter
				// for the root is given the name "ROOT" and the Flag is set to "0"..
			
				file_gui showfiles = new file_gui(parameters,information[row][1],"Root","0");
				showfiles.show();
				check = false;
			  }		// End if ...	
			}		// End If......
		}		// End Try.......
		catch(Exception e)
		{
		  row = 0;
		}
    }			// End if of ae.getSource()...

	else if (ae.getSource() == client_refresh)
	{
	  this.setVisible(false);		//hide the present window.....
	  String present_users[][] = {{" " ," "},{" ", " "}};	 // make the user list empty
	  
	  // Call the main class a  new ....	
	  client client = new client("start", " ", " ",present_users);
      client.setSize(465,410);
	  client.show();
			
	}			// End if of ae.getSource()...
	
	else if (ae.getSource() == client_quit)
	{
 		  System.exit(0);		// Close the connection and exit to system...
	}			// End if of ae.getSource()...
	
	else if (ae.getSource() == client_search)
	{
			// If search button is pressed a new class called search_window is called which
			// is reponsible for client searching it caters to the gui of the search_window 
			// as well....
			search_window search_users = new search_window(this, information);
			search_users.show();
			
	}		// End if of ae.getSource()...

	else if (ae.getSource() == client_view_files)
	{
			// This button activates only after the client_search button is used for 
			// searching the listener or a particular file name on all the possible 
			// users...
			
			// When this button is activated the user will see all the names of the users 
			// connected at that instant along with the number of files (satisfying a 
			// partculat search criteria) enclosed in brackett to see those files the user 
			// presses the viewfiles button after selecting a particular user and the 
			// listof the files is displayed to the user the user is however not able to 
			// download the files from that location...   
			int row = 0;

			row = client_listing.getSelectedRow();
			// for this another function of the add_on class is used returnfilenames which 
			// returns the filenames vector along with the name of the user..
			add_on search_result = new add_on();
			Vector filenames = search_result.returnfilenames();
			Vector results = new Vector();
			results.add(0,(Object)"files");

			// Out of this vector another vector is generated which consists of a files
			// or folders peratining to a paricular user who was highlighted when the 
			// viewfiles is pressed... the delimiters used are "?".
				
		
			for (int i = 1;i<filenames.size() ;i++ )
			{
				String temp = (String)filenames.get(i);
				String name = " ";
				name = temp.substring(0,temp.indexOf("?"));
				name = name.substring(0,information[row][1].length());
				if (name.equals(information[row][1]))
				{
					temp = temp.substring(temp.indexOf("?")+1);
					StringTokenizer st = new StringTokenizer(temp, "?");
					while (st.hasMoreTokens())
					{
						results.add((Object)st.nextToken());
					}
					temp = "";
				}
				
			}
			// when the vector is generated (results) it is passed on to the the file_gui
			// class which is responsible for displaying the files..., the root parameter 
			// is given the value as search result and the flag information as "0"... 
			file_gui showfiles = new file_gui(results,information[row][1],"Search Results","0");
			showfiles.show();

	}		// End if of ae.getSource()...

}			// End Action Listener......

// The function below places the information on to the gui and enables appropriate 
// buttons in between...

public void place_info_table(String information[][])
{
		if (!(information.length > 1))		// If no information[][] array is generated..
		{
		    JOptionPane.showMessageDialog(this,"Sorry There is no server at present to satisfy ur request. ","Peer 2 Peer Client",JOptionPane.ERROR_MESSAGE);
			client_open.setEnabled(false);
			client_refresh.setEnabled(false);// all buttons except quit are disabled.
			client_search.setEnabled(false);
			client_view_files.setEnabled(false);
			client_quit.setEnabled(true);

		}
		else
		{
			client_open.setEnabled(true);
			client_refresh.setEnabled(true);
			client_search.setEnabled(true);
			client_view_files.setEnabled(false);	 // else all the buttons except view
			client_quit.setEnabled(true);			 // files are enabled...
		
		}
		int j = 0;

		for (int i = 0;i<information.length ;i++ )		// Loops through information and 
		 {												// put the value on to the table..
			client_listing.setValueAt(information[i][0],j,0);
			j++;
			client_quit.setEnabled(true);
			client_refresh.setEnabled(true);
		 }	
		 
		client_open.addActionListener(this);
		client_refresh.addActionListener(this);			// Add action listener to all the 
		client_search.addActionListener(this);			// Buttons...
		client_view_files.addActionListener(this);
		client_quit.addActionListener(this);

}		// End... place_info_table()...

	

  public static void main(String[] args)  // this is the main function 
  {
		 String present_users[][] = {{" " ," "},{" ", " "}};
		 // Since client is called for the first time therefore the present users 
		 // array is left empty...   
		  client client = new client("start", " ", " ",present_users);
          client.setSize(465,410);
		  client.show();
  }

}		// End class client....

