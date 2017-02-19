using System.Configuration;

namespace Pobezhdatel.DB
{
    /// <summary>
    /// Manually created part of this Data Context class to create 
    /// default constructor where connection string is taken from 
    /// .config file but not stored in the .dbml file.
    /// </summary>
    public partial class PobezhdatelDbDataContext
    {
        /// <summary>
        /// Create a new Data Context where connection string 
        /// is taken from external .config file.
        /// </summary>
        public PobezhdatelDbDataContext()
            : base(ConfigurationManager.ConnectionStrings["TestDBConnectionString"].ToString())
        {
            OnCreated();
        }
    }
}