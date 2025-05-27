using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException(string message) : base(message) { }
    }
}
