using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace MXRaven.DAL
{
    #region "Code"
    /// <summary>
    /// RavenDb session helper
    /// </summary>
    public class MXRavenDbSessionHelper
    {
        //---All new lazy singleton that's thread safe.---
        private static Lazy<IDocumentStore> _lazyDocStore = new Lazy<IDocumentStore>(() => InitializeSessionFactory());

        private MXRavenDbSessionHelper() { }

        private static IDocumentStore SessionFactory
        {
            get
            {
                return _lazyDocStore.Value;
            }
        }

        public static IDocumentSession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        private static IDocumentStore InitializeSessionFactory()
        {
            var _docStore = new DocumentStore { ConnectionStringName = "RavenDBConnString", DefaultDatabase = "MXMunky" }; //One more way is this : _store = new DocumentStore { Url = "http://localhost:7000" };
            _docStore.Initialize();
            _docStore.Conventions.IdentityPartsSeparator = "-";

            //this is for creating lucene indexes. If you don't have any just remove this line. 
            IndexCreation.CreateIndexes(typeof(Location).Assembly, _docStore);

            return _docStore;
        }
    }
}

    #endregion

    #region "Steps for Setting up RavenDb"

    /*
     Setting up RavenDb.net - 
     
     1) Download the latest stable build from the official website http://ravendb.net
     2) Extract it and run this command in the console- 
       C:\RavenDB-Build-2261\Server>Raven.Server.exe /install
      That's it this installs the RavenDb as a windows service. Type in your browser the default port http://localhost:8080
      If you want a different port to be used just alter the first line in "Raven.Server.exe.config" file at "C:\RavenDB-Build-2261\Server" folder as
        <add key="Raven/Port" value="7000"/>
     
      and restart the service. Now when you access it through http://localhost:7000, Create a database.
          
     3) Add the connection string to your web/app config as 
        <connectionStrings>
            <add name="RavenDBConnString" connectionString="Url=http://localhost:7000" />
        </connectionStrings>
     4) Provide reference to two DLLs from "C:\RavenDB-Build-2261\Client" folder
        i) Raven.Abstractions.dll
        ii) Raven.Client.Lightweight.dll
     
     Note: This helper class is about setting up a session factory for RavenDb(using .Net 4 thread safe singleton pattern). For a complete tutorial/documentation, refer their official website(http://ravendb.net).
          I can post some useful test code very soon, that I've been using for doing my PoC. But referring the ravenDb website is the best.
     
     */
    #endregion

    #region "Sample usage"

    /*
        sample for a document structure-
        
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
    
    //Create index for searching a location by name.
    public class Location_ByName_FullTextSearch : AbstractIndexCreationTask<Location>
    {
        public Location_ByName_FullTextSearch()
        {
            Map = locations => from x in locations
                              select new { x.Name };
            
            Index(x => x.Name, FieldIndexing.Analyzed);
        }
    }
 
 * Sample usage - 
        public void DoSomeTest()
        {
            using (var session = MXRavenDbSessionHelper.OpenSession())
            {
            //Just Refer the documentation at ravendb.net website, it's much easier to grasp than I providing some examples here and there.
                //insert some documents

                //Retrieve them

                //Update 

                //Finally delete them

            }        
        }
 
 * 
 */

    #endregion

    

    
