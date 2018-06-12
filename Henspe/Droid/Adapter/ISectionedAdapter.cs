using System.Collections.Generic;

namespace Henspe.Droid.Adapters
{
    public interface ISectionedAdapter
    {
        List<SectionedListAdapter.Section> GetSections();
    }
}