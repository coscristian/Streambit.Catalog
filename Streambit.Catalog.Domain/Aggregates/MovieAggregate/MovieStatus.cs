using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public enum MovieStatus
    {
        Rumored,
        Planned,
        InProduction,
        PostProduction,
        Released,
        Canceled
    }
}
