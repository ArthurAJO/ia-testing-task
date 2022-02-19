using System.Collections.Generic;
using DuplicateFinderWPF.Models;

namespace DuplicateFinderWPF.Interface
{
    public interface IDuplicateFinder
    {
        IEnumerable<IDuplicate> CollectCandidates(string p);

        IEnumerable<IDuplicate> CollectCandidates(string p, CompareMode m);
        
        IEnumerable<IDuplicate> CheckCandidates(IEnumerable<IDuplicate> dups);
    }
}