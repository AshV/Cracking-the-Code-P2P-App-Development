//class for xml writer(returns a String for generating an XML File) 

import java.awt.*;
import java.io.*;
import java.util.*;
import java.lang.*;


 public class XmlWriter
{
	 int flag = 1 ;
	 StringBuffer sbuf  = new StringBuffer();	 
	 
	String name1 =  "", fame =  "", type = "", size = "", mask = "" ;

	void requestFString(String filetype, String filename) 
	{ 
		sbuf = sbuf.delete(0, sbuf.capacity());
		//String ch = filetype;
		sbuf.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		sbuf.append("<p2p_lng>");
		sbuf.append("<request type=\"" +filetype+ "\">");
		
		if (filetype.equals("SEARCH") || filetype.equals("DOWNLOAD") || filetype.equals("UPLOAD") )
		{
			sbuf.append("<scope type=\"" +filename+"\" mask=''>");
			sbuf.append("</scope>");
		}
	}

	 void  responseFString(String filetype, String filename, String filesize, String mask) 
	{ 
	    //System.out.println(" Last : " + filetype ) ;
		//String ch = filetype;
		//if (flag == 1)
		//{
		//	flag = 0;
		//}
		
		if (filetype.equals("SHOWFILES") || filetype.equals("SEARCH") )
			{
			if (filesize.trim().length() == 0)
			{
				sbuf = sbuf.append("<fileinfo filename=\"" +filename+"\" mask=\"" +mask+"\"/>");
			}
			else 
			{
			sbuf = sbuf.append("<fileinfo filename=\"" +filename+"\" mask=\"" +mask+"\" filesize=\"" +filesize+ "\"/>");
			//System.out.println(" Last : " + sbuf.toString()) ;
			}
		}
		if (filetype.equals("ERROR") )
		{
			//sbuf.append("<errorinfo errcode=\"" +extended error code+"\" severity=\"" +message+"\" description=\"" +description of possible error+ "\">");
		}

	}

	public String returnHeader(String filetype) throws IOException
		{
			StringBuffer sb = new StringBuffer();
			sb.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.append("<p2p_lng>");
			sb.append("<response type=\"" +filetype+ "\">");

			String temp_s = sb.toString();
 			sb = sb.delete(0,sb.capacity());
			return(temp_s);
		}

	public   String returnResponse() throws IOException
	{ 
		String tt;
		sbuf.append("</response>");
		sbuf.append("</p2p_lng>");
		tt = sbuf.toString();
		//System.out.println(sbuf.toString()) ;   // last response Statement to print
		sbuf = sbuf.delete(0, sbuf.capacity());
		return tt;

	}

	public  String returnRequest() throws IOException
	{ 
		String tt;
		sbuf.append("</request>");
		sbuf.append("</p2p_lng>");
		tt = sbuf.toString();
		//System.out.println(sbuf.toString()) ;   // last request Statement to print
		sbuf = sbuf.delete(0, sbuf.capacity());
		return tt;

	}
}