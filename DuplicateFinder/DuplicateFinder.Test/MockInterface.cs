using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;
using System.Threading.Tasks;


namespace Mock.Interface
{
        public interface IDirectoryHelper
        {
            ICollection<string> GetFiles(string path);
        }

}
