using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate;
using NHibernate.Event;

namespace MXUtilities
{
    /*
     The very first thing to be done is to install NHibernate and FluentNhibernate via nuget.
     */

    /// <summary>
    /// NHibernate Session Helper(setting up a session factory for Nhibernate using .Net 4 Lazily initialized thread safe singleton pattern).
    /// Similar code can easily be found on nhforge.org, but I've made it to use the new Lazy feature for singleton implementation.
    /// </summary>
    public class MXNHibernateSessionHelper
    {
        //---All new lazy singleton that's thread safe.---

        private MXNHibernateSessionHelper() { }

        private static Lazy<ISessionFactory> _lazySessionFactory = new Lazy<ISessionFactory>(() => InitializeSessionFactory());

        private static ISessionFactory SessionFactory
        {
            get
            {
                return _lazySessionFactory.Value;
            }
        }

        private static ISessionFactory InitializeSessionFactory()
        {
            ISessionFactory _sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Server=local;Database=MXTest;User ID=developer;Password=pwd")
                //.ShowSql()
            )
            .Cache(c => c.ProviderClass(typeof(NHibernate.Cache.HashtableCacheProvider).AssemblyQualifiedName).UseQueryCache())
                        
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>()) //Instead of User, put one of your nhibernate entity class here.
            
            .BuildSessionFactory();
            
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {            
                return SessionFactory.OpenSession();
        }
    }//End of class "MXNHibernateSessionHelper"
}
