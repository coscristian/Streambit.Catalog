using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streambit.Catalog.Domain.Aggregates.LanguageAggregate
{
    public class Language
    {
        public Guid LanguageId { get; private set; }
        public string Name { get; private set; }
        public string EnglishName { get; private set; }
        public string Iso6391 { get; private set; }
    }
}
