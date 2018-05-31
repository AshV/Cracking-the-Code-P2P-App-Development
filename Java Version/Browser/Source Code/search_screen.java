import javax.swing.*;
import javax.swing.event.*;
import java.awt.event.*;
import java.awt.*;
import java.util.*;

/* This class implements Searching for files on the Listener....*/ 

public class search_screen extends JFrame implements ActionListener
{
	JLabel label;							//
	JTextField search_text;					//  GUI Components for various purposes..
	JButton search_ok,search_cancel;		// 	
	
	Container contentpane;					//  For the placeing the GUI Componments..
	
	String root_information;
	String ip_address;
	String flag_info = "";
	
	/* When this class is called from the "file_gui.class" by pressing the search 
	   button it is provided with three parameters...
	   
	   root ... The root/directory on which search is being performed. This cannot be 
				the root directory...
		
	   ip	... The ip address of the listener's machine...
	   
	   flag ... The flag information of the root/ directory...

    */

	public search_screen(String root, String ip, String flag )
	{
		super("Search");				
		setSize(270,110);					// Size of the Search Screen Frame...
	
		contentpane = getContentPane();
		contentpane.setLayout(null);		// Setting the Layout to Absolute Layout..
		
		label = new JLabel("Search Criteria ");	// Initializing the GUI Component.
		label.setBounds(10,10,100,25);			// Positioning the GUI Component. 
	
		search_text = new JTextField(30);		// Initializing the GUI Component.
		search_text.setBounds(110,10,150,25);	// Positioning the GUI Component. 
				
		search_ok = new JButton("Search");		// Initializing the GUI Component.
		search_ok.setMnemonic('S');				// Setting the Mnemonic..
		search_ok.setBounds(45,50,80,25);		// Positioning the GUI Component. 
		
		search_cancel = new JButton("Cancel");	// Initializing the GUI Component.
		search_cancel.setMnemonic('C');			// Setting the Mnemonic..
		search_cancel.setBounds(150,50,80,25);	// Positioning the GUI Component. 
		
		search_ok.addActionListener(this);		// Add action listener to all the 
		search_cancel.addActionListener(this);	// Buttons...

		// Adding the window  Listener to the gui window
		// ie. Code for the "cross"...

		addWindowListener (new java.awt.event.WindowAdapter () {
        public void windowClosing (java.awt.event.WindowEvent evt) {
       	   setVisible(false);
       	}
        }
        );
		
		contentpane.add(label);					//
		contentpane.add(search_text);			//
		contentpane.add(search_ok);				//	Adding the GUI Buttons.....
		contentpane.add(search_cancel);			//
	
		
		root_information = root;	  // 
		ip_address = ip;			  // Initialzing the variables with parameters...		
		flag_info = flag;			  //	

	}

	public void actionPerformed(ActionEvent ae)
	{
		if (ae.getSource() == search_ok)		// When Ok Button Is pressed....
		{
			this.setVisible(false);				// Hide this GUI screen

			// another method of the class add_on is used now (search request) this 
			// is  used send to the listene the search criteria. For sending the search
			// criteria the function search_request takes four parameters....
			// 1.  root_information .. On Which the search request is being made..
			// 2.  search text ie. the text entered by the user on the text field 
			//	   provided.  
			// 3.  ip_address of the listener's machine....
			// 4.  Flag information of the directory on which request is made...

			
			add_on search_with_condition = new add_on();
			search_with_condition.search_request(root_information,search_text.getText(),ip_address,flag_info);
		}	
		else if (ae.getSource() == search_cancel)  // When Cancel Button Is pressed....
		{
			this.setVisible(false);				   // Hide this GUI screen	
		}
	}
	
}
