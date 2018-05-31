namespace Listener
{
    using System;

    /// <summary>
    ///    Summary description for ThreadIndex.
    /// </summary>
    public class ThreadIndex
    {
	   private object[] myArray = new object[100]; 
		private int Index;

        public ThreadIndex()
        {
            // TODO: Add Constructor Logic here
            //
			Index = 0;
        }

		public int TotalThreads()
		{
			return Index;
		}

		public object this [int index]   // indexer declaration
		{
		   get 
		   {
		      // Check the index limits
		      if (index < 0 || index >= 100)
		         return null;
		      else
		         return myArray[index];
		   }
		   set 
		   {
		      if (!(index < 0 || index >= 100))
			  {
				  myArray[index] = (object)value;
				  ++Index;
			  }
		   }
		}
	}
}
