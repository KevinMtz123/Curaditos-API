using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
   public enum Status { Created = 1, SentToWpp = 2, Confirmed = 3, Cancelled = 4}
    public enum TypePromotion
    {
        Percentage = 1,
        Fixed = 2,
        XForY = 3
    }
}
