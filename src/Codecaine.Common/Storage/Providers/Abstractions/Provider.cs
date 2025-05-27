using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Providers.Abstractions
{
    public abstract class Provider
    {
        /// <summary>
        /// The storage location Container/Bucket Name
        /// </summary>
        public string StorageLocation { get; set; } = string.Empty;


    }
}
