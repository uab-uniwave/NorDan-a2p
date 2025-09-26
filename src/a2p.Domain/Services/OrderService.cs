using a2p.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2p.Domain.Services
{
    public sealed class OrderService
    {
        public void AddFile(Order order, OrderFile file)
        {
            order.AddFile(file);
            // maybe some additional domain rules
        }
    }
}
