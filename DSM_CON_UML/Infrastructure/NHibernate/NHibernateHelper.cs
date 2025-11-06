using System;
using System.IO;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;

namespace Infrastructure.NHibernate
{
    public static class NHibernateHelper
    {
        private static ISessionFactory? _sessionFactory;

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;

            var cfg = new Configuration();

            // Try to load NHibernate.cfg.xml from this assembly folder
            var baseDir = AppContext.BaseDirectory;
            var configPath = Path.Combine(baseDir, "NHibernate", "NHibernate.cfg.xml");
            if (!File.Exists(configPath))
            {
                // Try repository relative path
                configPath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "NHibernate", "NHibernate.cfg.xml");
            }

            if (File.Exists(configPath))
            {
                cfg.Configure(configPath);
            }
            else
            {
                // fallback: try to load embedded resource or default settings
                cfg.Configure();
            }

            // Resolve |DataDirectory| token in connection string if present
            try
            {
                if (cfg.Properties != null && cfg.Properties.TryGetValue("connection.connection_string", out var conn))
                {
                    var connStr = conn ?? string.Empty;
                    if (connStr.Contains("|DataDirectory|"))
                    {
                        var dataDirObj = AppDomain.CurrentDomain.GetData("DataDirectory");
                        var dataDir = dataDirObj != null ? dataDirObj.ToString() ?? string.Empty : Path.Combine(Directory.GetCurrentDirectory(), "Data");
                        var replaced = connStr.Replace("|DataDirectory|", dataDir);
                        // Normalize backslashes
                        replaced = replaced.Replace("/", "\\");
                        cfg.SetProperty("connection.connection_string", replaced);
                    }
                }
            }
            catch
            {
                // If replacement fails, continue and let NHibernate raise the error
            }

            // Register all mapping files in Mappings folder
            var mappingsDir = Path.Combine(Path.GetDirectoryName(configPath) ?? baseDir, "Mappings");
            if (Directory.Exists(mappingsDir))
            {
                var files = Directory.GetFiles(mappingsDir, "*.hbm.xml", SearchOption.TopDirectoryOnly);
                foreach (var f in files.OrderBy(x => x))
                {
                    cfg.AddFile(f);
                }
            }

            _sessionFactory = cfg.BuildSessionFactory();
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
}

