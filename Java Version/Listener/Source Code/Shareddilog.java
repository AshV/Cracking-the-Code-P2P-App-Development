import java.util.*;
import java.awt.*;
import java.awt.event.*;
import java.io.*;
import javax.swing.*;
import javax.swing.event.*;
import javax.swing.border.*;

/*
 * This class mainly used for sharing files and directories for user to use.Through this class 
 * the listener can add the files and directory. The major disadvantage is that the listener  
 * cannot remove the already shared file/folder.
 */

class  Shareddilog extends JDialog  implements ActionListener 
{

public void shared_files()
        {
		// Create a object of the class JDialog
        sharedilog = new JDialog();
		// Set the Layout as null then draw the gui components according to our need
        sharedilog.getContentPane().setLayout(null);
		sharedilog.setTitle("Share Dialog");
		
		// Adding the window  Listener to the gui window
		// ie. Code for the "cross"...
		addWindowListener( new WindowAdapter()
        { public void windowClosing(WindowEvent e)
                {
                      dispose(); 
                }
        });
		share_file = new JButton("Share File");	// Initializing the GUI Component.
		share_file.setBounds(10,7,100,20);	// Positioning the GUI Component. 
		share_file.addActionListener(this);// Add action listener 
		sharedilog.getContentPane().add(share_file);	//	Adding the GUI Buttons.....
		
		share_folder = new JButton("Share Folder");// Initializing the GUI Component.
		share_folder.setBounds(110,7,150,20);// Positioning the GUI Component. 
		share_folder.addActionListener(this);// Add action listener 
		sharedilog.getContentPane().add(share_folder);//	Adding the GUI Buttons.....
		
		l_type = new JLabel();// Initializing the GUI Component.
		l_type.setText("Type a filename here or click browse to select");
		l_type.setBounds(15,35, 380, 20);// Positioning the GUI Component. 
		sharedilog.getContentPane().add(l_type);//	Adding the GUI Component
		
		
		t_type = new JTextField(150);	// Initializing the GUI Component
		t_type.setBounds(15,60, 380, 20);// Positioning the GUI Component. 
		sharedilog.getContentPane().add(t_type);//	Adding the GUI Component

		browse = new JButton("Browse");// Initializing the GUI Component
		browse.setBounds(315,85, 80, 20);// Positioning the GUI Component. 
		browse.addActionListener(this);// Add action listener 
		sharedilog.getContentPane().add(browse);//	Adding the GUI Component

		
		c_entry = new JCheckBox("Add this entry only");// Initializing the GUI Component
		c_entry.setBounds(275,110, 150, 20);// Positioning the GUI Component. 
		sharedilog.getContentPane().add(c_entry);//	Adding the GUI Component

		// When this button is pressed then the information regarding the file/folder shared will be added on to shard.ini file. 
		shared_it = new JButton("Share it");// Initializing the GUI Component
		shared_it.setBounds(250,165, 80, 20);// Positioning the GUI Component. 
		shared_it.addActionListener(this);// Add action listener 
		sharedilog.getContentPane().add(shared_it);//	Adding the GUI Component

		// This button for close the Dialog
		close = new JButton("Close");// Initializing the GUI Component
		close.setBounds(340,165, 80, 20);// Positioning the GUI Component. 
		close.addActionListener(this);// Add action listener 
		sharedilog.getContentPane().add(close);//	Adding the GUI Component

		// This RadioButton to give read only permision to the file
		read_only = new JRadioButton("Read only", true);// Initializing the GUI Component
		read_only.setBounds(10,140,80,20);// Positioning the GUI Component. 
		sharedilog.getContentPane().add(read_only);//	Adding the GUI Component

		// This RadioButton to give the read & write  permision to the file
		read_write =new JRadioButton("Read/Write", false);
		read_write.setBounds(10,165,80,20);// Positioning the GUI Component.
		sharedilog.getContentPane().add(read_write);//	Adding the GUI Component
		group = new ButtonGroup();

		group.add(read_only); // Add the radiobutton to the radiobuttongroup
		group.add(read_write);// Thereby enabling only one button at a time. 

		// Make vector to read Share.ini file and insert into it.
		v_file_list = new Vector();


	try
		{
			// String r_line iniatialize as null;
			String r_line = "";

			// Open a DataInputStream to the file "Share.ini" for Reading the data from the file.
			data_in = new DataInputStream(new BufferedInputStream(new FileInputStream("Share.ini")));
			// Open a bufferedreader to the file "Share.ini" for Reading the data from the file.
			data_buffer_in = new BufferedReader(new InputStreamReader(data_in));

		// Reading the file buffer
		while((r_line = data_buffer_in.readLine()) != null)
			{
			// add the data into a vector
				v_file_list.add(r_line);
			}

			// close DataInputStream
			data_in.close();
			// close the input Buffer
			data_buffer_in.close();
		}
		catch (IOException e)
		{}




		sharedilog.setSize(440, 225);
		sharedilog.show();

	}

// The part Below pertains to the Action Performed when a Button is pressed...
	
	public void actionPerformed(ActionEvent e)
		{
			Object source = e.getSource();

		// When User clicks on "Share file" Button 
		if(source == share_file)
			{
				l_type.setText("Type a filename here or click browse to select");
				b_cho_f = true;

			}
		// When user clicks on "Share folder" Button
		else if(source == share_folder)
			{
				l_type.setText("Type a folder name here:");
				b_cho_d = true;
			}
		else if(source == browse)
			{
				// Initialize object for JFileChooser
				fileselection = new JFileChooser();
				fileselection.setCurrentDirectory(new File("."));

		//		File pp1 = fileselection.getCurrentDirectory();

		if ( b_cho_f == true )
			{
				fileselection.setFileSelectionMode(0);
				b_cho_f = false;
			}
		// When c_cho_d the set mode is "1" .It means show the directroy
		else if(b_cho_d == true )
			{
				fileselection.setFileSelectionMode(1);
				b_cho_d = false; 
			}

		// Readonly right means the mask is set to 0;
		if (read_only.isSelected())
			{
				r_reights = "0"; 
			}
	   // readwrite right means the mask is set to 1;
		else if (read_write.isSelected())
			{
				r_reights = "1"; 
			}
			// show the JFileChooser
			int  pp = fileselection.showOpenDialog(this);
		if (pp == 0)
			{
				// selected the current file or directroy
				file_list = fileselection.getSelectedFile();
				// Find the length of the file
				lengthoffile = file_list.length();

				t_type.setText("");
				// Display the  text on to the JTextField..
				t_type.setText(file_list.toString());


		try
			{
				
				s_line = "";
				
				// Open a DataInputStream to the file "Share.ini" for Reading the data from the file.
				data_in = new DataInputStream(new BufferedInputStream(new FileInputStream("Share.ini")));
				// Open a Inputbuffer to the file "Share.ini" for Reading the data from the file.
			
				data_buffer_in = new BufferedReader(new InputStreamReader(data_in));
				// 
				value_all_ready_present = false;
					
					while((s_line = data_buffer_in.readLine()) != null)
						{
							// 
							first_time_entry = true;
							//Divide String on the basis of "="
							st = new StringTokenizer(s_line, "=");
						// while loop run tile token present
						while(st.hasMoreTokens())
							{
								
								if (st.nextToken().equalsIgnoreCase(file_list.toString()) == true )
									{
										
										value_all_ready_present = true;
										Box b = Box.createVerticalBox();
										b.add(Box.createGlue());
										b.add(new JLabel("This file is all ready exits"));
										getContentPane().add(b, "Center");
										setSize(180, 100);
										setVisible(true);
										JPanel p2 = new JPanel();
										// Press the "OK" button then close the messagebox
										JButton ok = new JButton("OK");
										p2.add(ok);
										getContentPane().add(p2, "South");
										ok.addActionListener(new ActionListener()
										{ public void actionPerformed(ActionEvent evt)
											{	setVisible(false); 
											}
										});
										System.out.println("This file is all ready exits");
										}
									else
									{
									}
							}

						}
						// close DataInputStream
						data_in.close();
						// close the input Buffer
						data_buffer_in.close();
					}
					catch (IOException ex)
					{}


			// if files and directories are not present in share.ini
			if(value_all_ready_present == false || first_time_entry == false)
				{
				// Check whether file_list is directroy or file
				if (!file_list.isDirectory())
					{
						// Add Files list in vector
						v_file_list.add(file_list.toString() + "=" + r_reights + "=" + lengthoffile);
					}
					else
					{
						// Add directries list in vector
						v_file_list.add(file_list.toString() + "\\=" + r_reights );
					}
				}
			}
		}
	
	else if(source == shared_it) // When share button button is pressed. 
		{
			try
			   {
					// Open a DataOutputStream to the file "Share.ini" for Reading the data from the file.
					data_out = new DataOutputStream(
						(new FileOutputStream("Share.ini")));
				
				for (int t = 0; t < v_file_list.size() ; t++)
					{
						// Write the all data present in the vector byte-by-byte in the Share.ini file  
						data_out.writeBytes(v_file_list.get(t) + "\n");
					}
				// Close the object of DataOutputStream
				data_out.close();
				}
				catch (IOException ex)
				{}

		if (c_entry.isSelected()) // When add this file only checkbox is checked
			{
				// Close the share dialog box 		
				sharedilog.dispose();

			}

		}
		
		else if(source == close)	// Close button is pressed.
			{
			
				sharedilog.dispose(); 
			}

		}

		
		JFileChooser fileselection; 
		JButton share_file;
		JButton share_folder;
		JLabel l_type;
		JTextField t_type;
		JButton browse;
		JCheckBox c_entry;
		JButton shared_it;
		JButton close;
		JRadioButton read_only ;
		JRadioButton read_write ;	
		ButtonGroup group;
		static boolean  b_cho_f = false;
		static boolean b_cho_d = false;
		static boolean first_time_entry = false;
		static boolean value_all_ready_present = false;
		DataOutputStream data_out;
		DataInputStream data_in;
		BufferedReader data_buffer_in;
		StringTokenizer st;
		// list of files
		static File file_list;
		// Store the length of files
		long lengthoffile;
		// Store the list all files which are all ready shared 
		Vector v_file_list;
		// Give the rights to the file and Directroy, "0" for read only and "1" for read and write 
		String r_reights;
		// Main Dialog box 
		JDialog  sharedilog;
		String s_line;
	}
