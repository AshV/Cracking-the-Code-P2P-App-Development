//  parses XML File

import java.awt.*;
import java.io.*;
import java.util.*;
import org.xml.sax.*;
import org.apache.xerces.parsers.SAXParser;
/*	This is the wrapper class of the XML parser class as it calls the Xml parser and in turn when the xml parser, parses the xml file it generates the call backs on the class MyContentHandler... the various parsed documents can be used by using the varoius functions in this class.This class returns a vector to the class which calls this class XMLParserServer.java the vector consists of the data provided by the XML document.
*/
public class XMLParserServer
{
	String attributevalue;
	Vector value1;
	// Function used for calling the parser it has a parameter called "uri" //which has the information of the file to be parsed...
	public void perform(String uri)
	{
		System.out.println("Parsing XML File : " + uri + "\n\n" );
		try
		{
			// Generate an object of the XMLParser class..
			XMLReader parser = new SAXParser(); 
			// Generate an Object of the MyContentHandler Class it is in this //class that the xml parser generates the call backs...				
			MyContentHandler contHandler = new MyContentHandler();
		    parser.setContentHandler(contHandler);
		    // call the parse function of the XMLParser class with the file //information as the parameter...
			parser.parse(uri);
			value1 = contHandler.returnvector();
	    }
		catch(IOException e)
		{
			System.out.println("Error reading uri : " +e.getMessage());
		}
		catch(SAXException e)
		{
			System.out.println("Error in parsing : " +e.getMessage());
		}
	}
	// This function returns the vector generated after xmlparsing is //complete..
	public Vector yakreturn()
	{
		return value1;
	}
}

class MyContentHandler implements ContentHandler
{ 
	private Locator locator;
	Vector value = new Vector();
	public Vector returnvector()
	{
		return value;
	}		
	// Only this function is used by us for our purpose....
	public void startElement(String namespaceURI, String localName, String rawName, Attributes atts) throws SAXException
	{	
		System.out.println("Name of the tag " + localName);
		System.out.println(" NO of Attributes " + atts.getLength());
		for(int i= 0; i<atts.getLength();i++)
		{
			System.out.println("Value of i :" + i);
			if(atts.getValue(i) == null)
			{
				System.out.println("Entered if Statement");
			}
			else
			{
				System.out.println(" Attribute : "  + atts.getLocalName(i) + atts.getValue(i));
				value.add(atts.getValue(i));
				 
			}
		}
		if(!namespaceURI.equals(""))
		{
		}
		else
		{
		}
		}	

	public void  characters( char[] ch, int start , int end ) 
	{}
	public void  startDocument() 
	{}
	public void  endDocument() 
	{} 
	public void endElement(String nameSpaceURI, String localName, String rawName) {}
	public void startPrefixMapping(String prefix, String uri) {}
	public void endPrefixMapping(String prefix) {}
	public void ignorableWhitespace(char[] ch, int start, int end) {}
	public void processingInstruction(String target, String data) {}
	public void setDocumentLocator(Locator locator) {}
	public void skippedEntity(String name) {}
	static String ty;
}