using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wejo.Common.Domain.Interfaces;

namespace Wejo.GameService.Application.Commands
{
    public abstract class BaseH(IWejoContext context)
    {
        protected readonly IWejoContext _context = context;
    }


}
