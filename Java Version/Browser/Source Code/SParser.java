import java.awt.*;
import java.io.*;
import java.util.*;
import org.xml.sax.*;
import org.apache.xerces.parsers.SAXParser;

/* This is the wrapper class of the XML parser class as it calls the Xml parser and in turn 
   when the xml parser, parses the xml file it generates the call backs on the class 
   MyContentHandler... the various parsed documents can be used by using the varoius functions in this class.

   This class returns a vector to the class which calls this class SParser.java the vector
   consists of the data provided by the XML document.

*/

public class SParser
{
	Vector values = new Vector();  // Initialzing a object(values) of the class vector...
								   // It is this object which is returned to the class 
								   // called... 	
   public Vector perform(String uri)	// Function used for calling the parser
   {									// it has a parameter called "uri" which has the 	
										// information of the file to be parsed...
		
		try
		{
			XMLReader parser = new SAXParser();	// Generate an object of the
												// XMLParser class... 
			
			// Generate an Object of the MyContentHandler Class it is in this class that
			// the xml parser generates the call backs...													

			MyContentHandler contHandler = new MyContentHandler();  
										
		    parser.setContentHandler(contHandler);
		    parser.parse(uri);	// call the parse function of the XMLParser class with the 
								// file information as the parameter...
			
			values = contHandler.values_attributes();
			
	    }
		catch(IOException e)
		{
			System.out.println("Error reading uri : " +e.getMessage());
		}
		catch(SAXException e)
		{
			System.out.println("Error in parsing : " +e.getMessage());
		}

			return values;	// Return the vector generated after xmlparsing is complete..

	}

	
}

class MyContentHandler implements ContentHandler
{ 
	private Locator locator;
	
     Vector values = new Vector();
	 	 int j = 0;
	
	public Vector values_attributes()
	{
	   return (values);
	}
	public void startElement(String namespaceURI, String localName, String rawName, Attributes atts) throws SAXException  // Only this function is used by us for our 
										 //  purpose....
 	{	
		for(int i= 0; i<atts.getLength(); i++)
		{
		 values.add(j,(Object)atts.getValue(i)) ;
		 j++;  
		}
	}
	public void  characters( char[] ch, int start , int end ) 
	{}
	public void  startDocument() {}
	public void  endDocument() {}
	public void endElement(String nameSpaceURI, String localName, String rawName) {}
	public void startPrefixMapping(String prefix, String uri) {}
	public void endPrefixMapping(String prefix) {}
	public void ignorableWhitespace(char[] ch, int start, int end) {}
	public void processingInstruction(String target, String data) {}
	public void setDocumentLocator(Locator locator) {}
	public void skippedEntity(String name) {}

}