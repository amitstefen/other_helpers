using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.IO;

using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;

namespace MXLuceneConsoleApp
{
    /*
     First Install the lucene.net from here https://nuget.org/packages/Lucene.Net/3.0.3     
     */

    #region "Code"

    /// <summary>
    /// This helper class applies a singleton pattern to create Searcher and Writer objects as it's recommended to create them only once.
    /// Also the searcher gets reinitialized, if any write happens.    
    /// </summary>
    public class MXLuceneIndexHelper
    {
        private static IndexSearcher _searcher;        
        private static Directory _directory;
        private static Lazy<IndexWriter> _writer = new Lazy<IndexWriter>(() => new IndexWriter(_directory, new StandardAnalyzer(Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED));

        //this private constructor makes it a singleton now.
        private MXLuceneIndexHelper() { }

        //Static constructor, opening the directory once for all.
        static MXLuceneIndexHelper()
        {
            _directory = FSDirectory.Open(new DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndexDir"));
        }

        public static IndexSearcher IndexSearcher
        {
            get
            {
                if (_searcher == null)
                {
                    InitializeSearcher();
                }
                else if (!_searcher.IndexReader.IsCurrent())
                {                    
                    //_searcher.IndexReader.Reopen(); 

                    //refreshing the underlying Reader above doesn't do the trick, so I'm reinitializing the Searcher.
                    _searcher.Dispose();                    
                    InitializeSearcher();
                }

                return _searcher;
            }
        }

        public static IndexWriter IndexWriter
        {
            get 
            {                
                //_writer.SetRAMBufferSizeMB(30.0);
                return _writer.Value; 
            }
        }

        private static void InitializeSearcher()
        {
            _searcher = new IndexSearcher(_directory, false);
            
        }
    }//End of class

    #endregion
}

#region "usage comments"

/*
 Sample usage - 
 
   public void DoSomeTest()
   {     
      var writer = MXLuceneIndexHelper.IndexWriter;
        
      //Write some documents here with the help of above writer.
 
     //Search on the lucene Index using the below searcher.
      var searcher = MXLuceneIndexHelper.IndexSearcher;
      
    
    //For good understanding about lucene; please read the "Lucene In Action, 2nd Edition" book. Also, stackoverflow and other sites are good source.
   }
 
 */

#endregion
