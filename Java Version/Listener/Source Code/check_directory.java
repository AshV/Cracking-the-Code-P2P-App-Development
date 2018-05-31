import java.io.*;

/*
 * This class is only for searching purpose . namely
 * ., *., .*, *.*, ja*.*, java.*, java., *.ja*, *.java, ja*.ja* etc. In this case it can
 * search file as well directory . 
 */


public class check_directory
{
	
	// To store the list of files
	String[] files;

	// to store the fillter files only
	String[] fillterfiles;

	
	String[][] all_details;  

	// Number of files.
	int count = 0;

	//int  sizeoffile = 0;


	public String[][] wild_card(String left_half,String right_half, boolean left_half_flag, boolean right_half_flag, File path_file)
	{
					// store all the files  in files variable 
					files = path_file.list();

					fillterfiles = new String[files.length + 1];

		// If search criteria is *.*
		if ((left_half.equals(" "))&&(right_half.equals(" ")))
		{
			
				// Total number of files..
			for (int i = 0;i<files.length ;i++ )
			{
				
				// Search result..
				fillterfiles[count] = files[i];
					
				count++;

			}
		}

		// right side of file is "" and "*"
		else if (right_half.equals(" "))
		{
			String temp = "";
							// Total number of files..
			for (int i = 0;i<files.length ;i++ )
			{
				// Find the position of "." in file
				int index_dot = files[i].indexOf(".");
				
				if (index_dot != -1) // "."  is not found..
				{
				  // Store the substring till the index of dot is reached.
	  			  temp = files[i].substring(0,index_dot);
				}
				else
				  // Store the first[i] in temp veriable
				  temp = files[i];

				// temp veriable is equal to left_half and "*" not present in left side of file
				if ((temp.length() == (left_half.length()))&&(temp.equalsIgnoreCase(left_half))&&(!left_half_flag))
				{
					
					// files satisfying the search criteria.
					fillterfiles[count] = files[i];

					// increment the count value by 1
					count++;
				}
					// * is present in left half 
				if ((left_half_flag)&& (temp.length() >= left_half.length()))
				{

				
					if (left_half.equalsIgnoreCase(temp.substring(0,left_half.length())))
					{
				
					//	files satisfying the search criteria
					fillterfiles[count] = files[i];

					// increment the count value by 1
					count++;
					}
				}
				
			}	
		}
		
		// left side of file is "" and "*"
		else if (left_half.equals(" "))
		{

			String temp = "";

			// total number of files
			for (int i = 0;i<files.length ;i++ )
			{
				// Find the position of "." in file
				int index_dot = files[i].indexOf(".");

				if (index_dot != -1) // if dot is found..
				{
				  // Store value in temp "." position + 1 to last charecter 
	  			  temp = files[i].substring(index_dot+1);															
				}
				else
				  temp = " ";

				// temp variable is equal to right_half and "*" not present in left side of file
				if ((temp.length() == (right_half.length()))&&(temp.equalsIgnoreCase(right_half))&&(!right_half_flag))
				{
					// files satisfying the search criteria
					fillterfiles[count] = files[i];
					
					// increment the count value by 1
					count++;
				}

				/* "*" present in right side and temp (length) is greater than right_half length */
				if ((right_half_flag)&& (temp.length() >= right_half.length()) )
				{

					if (right_half.equalsIgnoreCase(temp.substring(0,right_half.length())))
					{
					
					// files satisfying the search criteria
					fillterfiles[count] = files[i];
					
					// increment the count value by 1
					count++;
					}
				}
			}		// End for loop....	
	
		  }			// Else if...
		  else
			{
				/* some character and "*" present in left side and some character and "*" present in right side */
				if ((right_half_flag)&&(left_half_flag))
				{
					// total number of files..
					for (int i = 0;i<files.length ;i++ )
					{
						/*  if file files[i] length is grater and equal to left_half length 
						 * then enter in this condition */
						if (files[i].length() >= left_half.length())
						{
							if (left_half.equalsIgnoreCase(files[i].substring(0,left_half.length())))				  			{
							  
 			  				  // Find the position of "." in file
							  int index = files[i].indexOf(".");

								if (index != -1)
							    {
								    // Store value in temp "." position + 1 to last charecter 
									String temp = files[i].substring(index+1);
				
									/*  if file temp length is grater and equal to right_half length 
									 */
									if (temp.length()>=right_half.length())
									{

									
									  if (temp.substring(0,right_half.length()).equalsIgnoreCase(right_half))
									  {
										
    	  								  
										// files satisfying the search criteria
										  fillterfiles[count] = files[i];
									 	  
					  					  // increment the count value by 1
										  count++;
									  }

									}					
							     }
							 }
						 }
					  }		
				  }

				/* only characters are  present in left side and some charecter and "*" present in right side */
				else if ((right_half_flag)&&(!left_half_flag))
				{

					// make complete filename out of the criteria. 
					String filename = left_half+"."+right_half;
				
					// total number of files..
					for (int i = 0;i<files.length ;i++ )
					{
						// if search criteria is less than the file length
						if (files[i].length() >= filename.length())
						{
							if (filename.equalsIgnoreCase(files[i].substring(0,filename.length())))
				  			{
							  
							  
							  //	files satisfying the search criteria
			  				  fillterfiles[count] = files[i];
				
				  			  // increment the count value by 1
							  count++;
							}
						}
					}
				}

				/* only characters are present in right side and some charecter and "*" present in left side */
				else if ((!right_half_flag)&&(left_half_flag))
				{
					// total number of files.
					for (int i = 0;i<files.length ;i++ )
					{
						/*  if length of files[i] is greater and equal to left_half length 
						  */
						if (files[i].length() >= left_half.length())
						{
							/* If left_half is equal to substring of files[i] first position to length of left_half */
							if (left_half.equalsIgnoreCase(files[i].substring(0,left_half.length())))
				  			{
							  // Find the position of "." in file
							  int index = files[i].indexOf(".");
							
							
								if (index != -1)
							    {
								    // Store value in temp "." position + 1 to last charecter 
									String temp = files[i].substring(index+1);

									// right_half is equal to temp then enter in this condition 
									if (temp.equalsIgnoreCase(right_half))
									{

									//	files satisfying the search criteria			
										fillterfiles[count] = files[i];
										
						  			    // increment the count value by 1
										count++;
									 	  
									}
							    }
							}
						}
					}			
				}
				else
				 {
					// Make complete filename to add strings 
					String filename = left_half+"."+right_half;
					
					// all files
					for (int i = 0;i<files.length ;i++ )
					{
						// filename is equal to files[i] then enter in this condition 
						if (filename.equalsIgnoreCase(files[i]))
				  	    {
					        

							//	files satisfying the search criteria
							fillterfiles[count] = files[i];
				
						    // increment the count value by 1
							count++;
				          
						 }	
					}
				 }

			}

		
		long selectedfilesize = 0;

	
		String filesizereturn = ""; 

	
		String maskoffile= "" ;

		all_details = new String[fillterfiles.length][3];

		// run till the length of fillterfiles array..
		for(int i = 0; fillterfiles[i] != null; i++)
		{
			//Make the file object by appending path of files 
			File finallist = new File(path_file + "\\" + fillterfiles[i]);
			

			// if current file object is directory then 
			if (finallist.isDirectory())
			{
				// no filesize  
				filesizereturn = "";

				//mask = 0
				maskoffile = "0";

				// in case of directroy add "\" in last
				fillterfiles[i] = fillterfiles[i] + "\\" ;
				
				
			}

			// if a file then 
			else
			{
				if (finallist.canRead())
				{
					// read mask
					maskoffile = "0";
				}

				else if(finallist.canWrite())
				{
					// write mask
					maskoffile = "1";
				}
			
			// find the file size
			selectedfilesize = finallist.length();


			filesizereturn	=	Long.toString(selectedfilesize);
			}
			
 /// Stroe the information in array..
			
			all_details[i][0]= fillterfiles[i];
			all_details[i][1]= filesizereturn;
			all_details[i][2]= maskoffile ;
		}
		// Return the array to the calling programme..	
		return all_details;
	}		
	
}
