/*
 * This is login class. This class is used for login the user has to enter his login name 
 * and through this Frame user can open the Share frame . The ip address of the System and 
 * login is sent to the Server (Login.asp).When the user wants to logout again the 
 * information is sent to the Server (Logout.asp)
 */

import java.util.*;
import java.net.*;
import java.io.*;
import javax.swing.*;
import javax.swing.event.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.Color;

public class  Login extends JFrame implements ActionListener
{

// This is Constructor part of this login class
 public Login()
{

		// set the title of this frame
        setTitle("Login");

		// set layout to null
		getContentPane().setLayout(null);

		// Set size to 310 and 150
        setSize(310,150);

		// get the insets
		Insets insets = getInsets();

		// write the window closing event.
		addWindowListener(new WindowAdapter()
		{ public void WindowClosing(WindowEvent e)
			{ 
				System.exit(0);
			}
		});

        compname = new JLabel("© www.dreamtechsoftware.com");
		compname.setBounds( 50 + insets.left ,insets.top + 2, 200, 10);
		compname.setForeground(Color.red);
		getContentPane().add(compname);
		//This is the label displayed just befor the TextFeild
		l_login = new JLabel("Login ID :");
        //This method sets the cordinates where this label will be drawn
		l_login.setBounds( 10 + insets.left ,insets.top + 20, 60, 20);
        //Label is added to the Frame
		getContentPane().add(l_login);


        //This textfeild is for the user to enter his login name
        login_field = new JTextField(50);
        //This method sets the cordinates where this textfeild will be drawn
		login_field.setBounds( 70 + insets.left ,insets.top + 20, 220, 20);        
        //TextFeild is added to the Frame
		getContentPane().add(login_field);

        /*CheckBox is used to store login information 
		* If Checked UserInfo.ini file is created which will store the user name
		*/
        c_remember = new JCheckBox("Remember my Login ID");
        //This method sets the cordinates where this textfeild will be drawn
		c_remember.setBounds( 70 + insets.left ,insets.top + 40, 220, 20);
        //Registering this component for user events
		c_remember.addActionListener(this);
        //TextFeild is added to the Frame 
		getContentPane().add(c_remember);


		/* This button is for sharing a file. Pressing this button will open a filechooser dialog
		* where the user can choose the files to be shared 
		*/
        bshared_files = new JButton("Shared Files");
        //This method sets the cordinates where this textfeild will be drawn
		bshared_files.setBounds( 0 + insets.left ,insets.top + 70, 110, 40);                
        //Registering this component for user events  */
		bshared_files.addActionListener(this);
        //TextFeild is added to the Frame
		getContentPane().add(bshared_files);


		//This button is for Login. Pressing this button will register the user with the server
		blogin = new JButton("Login");
        //This method sets the cordinates where this textfeild will be drawn
		blogin.setBounds( 111 + insets.left ,insets.top + 70, 90, 40);                
        //Registering this component for user events
		blogin.addActionListener(this);
		//TextFeild is added to the Frame
		getContentPane().add(blogin);

		/*This button is for Logout. Pressing this button will make the user logout and his entry wiil
		*  be removed from the server
		*/
		bcancel = new JButton("Cancel");
        // This method sets the cordinates where this textfeild will be drawn
		bcancel.setBounds( 202 + insets.left, insets.top + 70 , 90, 40);
        // Registering this component for user events 
		bcancel.addActionListener(this);
		// TextFeild is added to the Frame 
		getContentPane().add(bcancel);

	try
	{
		// Store the ip address of local host	
        InetAddress localHostAddress = InetAddress.getLocalHost();

		// local ip address converted into string
        String local_address = localHostAddress.toString();
         		// its local ip address seperated by "/"
        st = new StringTokenizer(local_address, "/");
 
		// While loop run till tokens are present
        while(st.hasMoreTokens())
        {
			// login computer name stored in this variable
		    machine_owner_name = st.nextToken();

			// login computer ip address stored in this variable
			ip_address = st.nextToken();
	    }

    }

	catch (Exception e)
	{	System.out.println("Error: " + e);
	}

	try
	{

		String r_line = "";

		// Opened a Stream to read from UserInfo.ini file 
		user_stream = new DataInputStream(new BufferedInputStream(new FileInputStream("UserInfo.ini")));
		
		// Buffered reader object instantiated to read from the stream
		name_in_buffer_stream = new BufferedReader(new InputStreamReader(user_stream));

		// This loop runs until the end of the stream
		while((r_line = name_in_buffer_stream.readLine()) != null)
		{
			// string divide on the basis of " = "
			r_st = new StringTokenizer(r_line, " = ");
     
			// while loop run till any token present 
			while(r_st.hasMoreTokens())
	        {
				// store the text "username"
				user = r_st.nextToken();

				// store the name of the user
			    name = r_st.nextToken();

				// display the name of login user in textfiled 
				login_field.setText(name);
				
				// checkbox  c_remember set true
				c_remember.setSelected(true);
		    }
     
		}

		// close the user_stream 
		user_stream.close();

		// close the name_in_buffer_stream
		name_in_buffer_stream.close();
	}
	catch (IOException e)
	{
	}
}
/* This method is used to handle the user generated events  */
public void actionPerformed(ActionEvent e)
{
	//The getSource method returns the object of the component which has generated an event  
	Object pan =e.getSource();

	//The code in the if condition is executed if the user click the login button 
	if(pan.equals(blogin))
	{
	try
	{

				int count = 0;
				full_string = "";
				String search_name = "";
				
				// First time search the "Share.ini" file open the stream for it
				BufferedReader first_search = new 
				BufferedReader(new FileReader("Share.ini"));
				
		// while loop run till first_search stream not become null
		while ((search_name = first_search.readLine()) != null)
			{

				count++;
                String filename = "";
                String filesize = "";
                String mask = "";

				// String divided in the basis of "="
				st_xml = new StringTokenizer(search_name, "=");

				// while loop run till tokens present in StringTokenizer
				while(st_xml.hasMoreTokens())
                {
					// in this variable store the file name and directory
					filename = st_xml.nextToken();

						/* if filename string not end by "\" then enter in this condition
						 * otherwise go to else part */
				        if( !filename.endsWith("\\") )
							{
								// store the rights of file
								mask = st_xml.nextToken();

								// Store the file size in this veriable
						        filesize = st_xml.nextToken(); 

								// take only file name 
					            String  word = filename.substring((filename.lastIndexOf("\\") + 1 ));			

								// store the file name with "*"
								full_string = full_string + word + "*";
							

							}
						else
							{
								// store the rights of file
								 mask = st_xml.nextToken();

								// store the directory with full path name "*"
								full_string = full_string + filename + "*";
							
							}
		           }

			}
		
			first_search.close();		

		}
		catch(IOException ex)
		{
		}

	try
	{
		// store the user name which is enterd by user in textfield
		name = login_field.getText();

		
		// full_string value is null means first time "Share.ini" file does not exist
		if (full_string.equals(""))
			{
				full_string = "";
			}
		else
			{
				// make one string for share.ini file list
//				full_string = full_string.substring(0, (full_string.length() - 1));
			}
		/* Login.asp file on the server is called and three parameterd are passed ip_address,share file
		* and directory list 
		*/
	String final_string = "";
	for(int i=0;i<full_string.length();i++)
	{
		if(full_string.charAt(i) == ' ')
			final_string += "%20";
		else	final_string += full_string.charAt(i);
	}
	// urlName = "http://SERVERNAME/login.asp?USERID=" + name + "&IP=" + ip_address + "&share=" + full_string;
		urlName = "http://SERVERNAME/login.asp?USERID=" + name + "&IP=" + ip_address + "&share=" + final_string;
	    // maket the object for URL and send the asp path
		  URL url = new URL(urlName);

		  // open the connection of url
		  URLConnection connection = url.openConnection();

		  // connect with url
		  connection.connect();

		  //Open a stream to read from the URL
		InputStream is = connection.getInputStream();


		InputStreamReader in = new InputStreamReader(is);


		BufferedReader response_stream = new BufferedReader(in);//new InputStreamReader(connection.getInputStream()));
		
    	  // Character array created 
		  char[] b = new char[32];
		  int i =0;
		  
		  // Make the string buffer object which store the response send by server
		  StringBuffer sb = new StringBuffer();
	  while ((i = response_stream.read(b,0,32))>0)
		{
			// charecter by charecter append in the string buffer
		     sb = sb.append(b,0,i);
	    }
	
		// make the object of newstsrtclass 
		newstartclass = new StartNewClass();
		
		//call the constector of that class
		newstartclass.startnew();

	}
	catch(Exception exception)
	{
	        System.out.println("Error :" + exception);
	}


	}

	// if user click on "shared files" button  then enter in this condition
	else if(pan.equals(bshared_files))
	{
		// make the object of Shareddilog class 
        sh_files = new Shareddilog();
	
		// calling the function of this Shareddilog class
        sh_files.shared_files();
	}

	// if user click on "cancel" button this code is executed
	else if(pan.equals(bcancel))
	{
		try
		{

			// Call the logout.asp file giving parameter the ip address of the system
			urlName = "http://SERVERNAME/logout.asp?IP=" + ip_address;
		//urlName = "http://SERVERNAME/logout.asp?IP=" + ip_address;

			// url object instantiated
		    URL url = new URL(urlName);
			
			// URL called by opnening a Stream
			url.openStream();
		}
		catch(Exception exception)
		{
		    System.out.println("Error :" + exception);
		}
		System.exit(0);
	}

	// if user check "Remember my Login ID" checkbox the code in this condition is executed
	else if(pan.equals(c_remember))
	{

			if(c_remember.isSelected())
			{

				String u_name = "";

			/* login text field is not null then enter in this condition otherwise enter in
			 * else condition */
			if((u_name = login_field.getText()) != null)
				{
					try
						{

						// open the stream of "UserInfo.ini" for writing the file
				        out = new DataOutputStream(
				        new FileOutputStream("UserInfo.ini"));

						// write in this file Byte by Byte
				        out.writeBytes("username = " + u_name);

						//close the out Stream
				        out.close();
						}
						catch (IOException tr)
						{}
				}
			}
			else
			{

			// Make the file object store the "UserInfo.ini" data into this variable
	        File user_list = new File("UserInfo.ini");
			
			// Find the path of the file
	        String actual_path = user_list.getAbsolutePath();

			
			//Make the object for deleting  the file
	        File delete_file = new File(actual_path);
	        String d_file = "";
			if((d_file = login_field.getText()) != null)
				{
					// delete the file 
			        boolean io = delete_file.delete();

					// The textfeild is cleared (blank)
				    login_field.setText("");
				}
			}
	}
}

	// The main function of the application
	public static void main(String[] args) 
	{

				
                JFrame login = new Login();
                login.show();
	}

		// Decleraing login button 
        JButton blogin;

		// Declearing cancel button
        JButton bcancel;

		// Declearing ShareFile button
        JButton bshared_files;

		// Declearing TextFeild for entering username
        JTextField login_field;

		// Label declared
        JLabel l_login;
		JLabel compname;

		// declearing Checkbox
        JCheckBox c_remember;
		
		// This string will conatins the url address 
        String urlName;

		// Declearing StringTokenizer
		StringTokenizer st_xml;

		// Declearing StringTokenizer
		private StringTokenizer st;

		// Declearing StringTokenizer
        private StringTokenizer r_st;

		// This string contains the ip address of the Users System
        static private String ip_address;

		static private String machine_owner_name;

		// Store the "UserName"
        static private String user;

		// Store the login name 
        static private String name;
		//Declearing DatadInputStream
		private DataOutputStream out;
        //Declearing DatadInputStream
		private DataInputStream user_stream;
        //Declearing Buffered Reader
		private BufferedReader name_in_buffer_stream;

		// Declearing StartNewClass which call the listener
        StartNewClass newstartclass;

		// Declearing Shareddilog which call the filechooser frame for sharing file & directory
        Shareddilog sh_files;

		String full_string;
}


/* this class start another Thread.In the application login screen runs on one thread and listener
* runs on another Thread
*/
class StartNewClass extends Thread {
	public void startnew()
	{
		// call the run function
        start();
	}

	public void run()
	{
		try
		{
        
			// make the object for MultiServer class
			server = new MultiServer();
			// call the function of this class
			server.multiaccess();
		}
		catch(IOException ex)
		{}
	}
	// defined the object of this class
       public MultiServer server;
}
