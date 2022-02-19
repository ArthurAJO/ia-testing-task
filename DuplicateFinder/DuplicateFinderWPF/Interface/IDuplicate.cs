using System.Collections.Generic;

namespace DuplicateFinderWPF.Interface
{
    public interface IDuplicate
    {
        IEnumerable<string> FilePaths { get; }
    }
}