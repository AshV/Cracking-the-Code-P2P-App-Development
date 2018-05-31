import javax.swing.*;
import javax.swing.event.*;
import java.awt.event.*;
import java.awt.*;
import java.util.*;
import java.net.*;
import java.io.*;

/* This Class is used to implement Client search on the p2p client... This class has 
   two main features.... 
   1. To search for a file on all the connected listener's...
   2. To search for a file on a particular subset of the connected listener's...
*/

public class search_window extends JFrame implements ActionListener
{
	JLabel label_file_name;					  //
	JLabel label_computer_name;				 //
	JTextField search_file_name;			//   GUI Components for various purposes..
	JTextField search_computer_name;	   //
	JButton search_ok,search_cancel;      //
	
	Container contentpane;
	
	String information[][];
	JFrame papa_window;					// To Keep information about the parent frame..

	// The constructor of the class search window takes two arguments..
    //  1..  The parent frame...
	//	2..  The name about all the present liseners and their IP addresses stored in 
	//  	 2-D array info.. 

	// The arguments are then initialized to the objects papa_window and String 2-D array
	// information....
	
	search_window(JFrame ancestor, String info[][])
	{
		
		super("Search Window...");
		setSize(375,160);				// Set The Size of the Frame...
		papa_window = ancestor;

		contentpane = getContentPane();
		contentpane.setLayout(null);		// Setting the Layout to Absolute Layout..
		
		label_file_name = new JLabel("Search for File Names "); // Initializing the GUI.
		label_file_name.setBounds(10,10,130,25); // Positioning the GUI Component. 
	
		search_file_name = new JTextField(30);	    // Initializing the GUI Component.
		search_file_name.setBounds(180,10,160,25);  // Positioning the GUI Component. 

		label_computer_name = new JLabel("Search On Computer Names "); // Initializing the
																	   // GUI Component.
		label_computer_name.setBounds(10,45,170,25);	// Positioning the GUI Component. 
	
		search_computer_name = new JTextField(30);	// Initializing the GUI Component.
		search_computer_name.setBounds(180,45,160,25);// Positioning the GUI Component. 
			
		search_ok = new JButton("Search");		// Initializing the GUI Component.
		search_ok.setMnemonic('S');				// Setting the Mnemonic.. 		
		search_ok.setBounds(75,90,80,25);		// Positioning the GUI Component. 

		
		search_cancel = new JButton("Cancel");	// Initializing the GUI Component.
		search_cancel.setMnemonic('C');			// Setting the Mnemonic.. 		
		search_cancel.setBounds(180,90,80,25);  // Positioning the GUI Component. 

			
		search_ok.addActionListener(this);		// Add action listener to all the 
		search_cancel.addActionListener(this);  // buttons...


		// Adding the window  Listener to the gui window
		// ie. Code for the "cross"...

		addWindowListener (new java.awt.event.WindowAdapter () {
        public void windowClosing (java.awt.event.WindowEvent evt) {
       	   setVisible(false);
       	}
        }
        );
		
		contentpane.add(label_file_name);		//
		contentpane.add(search_file_name);		//
		contentpane.add(label_computer_name);	//	Adding the GUI Buttons.....
		contentpane.add(search_computer_name);	//
		contentpane.add(search_ok);				//
		contentpane.add(search_cancel);			//
	
		information = info;	// Initializing the variable information with the parameter 
							// info...
	
	}

	public void actionPerformed(ActionEvent ae)
	{
		if (ae.getSource() == search_ok)		// When Ok Button Is pressed....
		{
	
		  String us = " ";	// Two variables namely us(user search) and fs (file search)
		  String fs = " ";   // are used to store the value that the user enters in the 
							// appropriate textfields...

		  boolean search = true;   // Temporary variable used in computation..

			us = search_computer_name.getText();  // Storing the value
			fs = search_file_name.getText();	// Storing the value
			
			
			if ((us.equals(""))&&(fs.equals("")))	// Apply check condition
			{
			JOptionPane.showMessageDialog(this,"Please Enter some search Criteria ","Peer 2 Peer Client",JOptionPane.ERROR_MESSAGE);
			search = false;

			}
			else if (us.equals(""))				// if any of the field is left empty..
			{
				us = "*";
			}
			else if (fs.equals(""))				// if any of the field is left empty..	
			{
			 fs = "*";
			}
			if (search)		// Start the search procedure... as both the variables
			{				// have been assigned the value...
				this.setVisible(false);	// hide the search window...
				papa_window.setVisible(false);	// Hide th parent window as well...
				// Call the main class client with the parameters...
				// 1.. Search..
				// 2.. user search criteria..
				// 3.. file search criteria..
				// 4.. Information about the present users...

				client search_result = new client("Search",us,fs,information);
				search_result.setSize(465,410);  // Set the size of the GUI called..
				search_result.show();			 // Display the GUI...	
			}
	
		}	
		else if (ae.getSource() == search_cancel)	// When Cancel Button Is pressed....
		{
			this.setVisible(false);				  // Hide this GUI screen	
		}
	}
	
}
