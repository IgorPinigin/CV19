using LibraryBaikal.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Data
{
    class DbInitializer
    {
        private readonly BaikalDB _db;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(BaikalDB db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public void Initialize() 
        { 
        
        }
    }
}
