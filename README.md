Utility code(miscellaneous)


This is actually a collection of utility objects; focusing on reuse, easy maintenance and extensibility.

#1) MXRavenDbSessionHelper - This helper class is about setting up a session factory for RavenDb(using .Net 4 thread safe singleton pattern). Also enclosed are some useful tips in setting up the NoSql RavenDb.

#2) MXLuceneSessionHelper - This helper class applies a thread safe singleton pattern to create Searcher and Writer objects as it's recommended to create them only once. Also the searcher gets reinitialized, if any write happens.

#3) MXNHibernateSessionHelper - setting up a session factory for Nhibernate using .Net 4 Lazily initialized thread safe singleton pattern. Similar code can easily be found on nhforge.org, but I've made it to use the new Lazy feature for singleton implementation.

#4) MXSqlBuilder - A simple SQL builder, easily extendable. I built it for using with dapper or raw ado.net as dapper's sqlbuilder looks to complex to use. The primary purpose is to build dynamic where condition to be used in search filters.
