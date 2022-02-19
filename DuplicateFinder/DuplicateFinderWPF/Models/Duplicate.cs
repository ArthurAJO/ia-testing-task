using System.Collections.Generic;
using DuplicateFinderWPF.Interface;

namespace DuplicateFinderWPF.Models
{
    public class Duplicate : IDuplicate
    {
        public IEnumerable<string> FilePaths { get; }
        
        public Duplicate(IEnumerable<string> filePaths)
        {
            this.FilePaths = filePaths;
        }
    }
}