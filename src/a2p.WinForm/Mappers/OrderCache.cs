using a2p.Shared.Application.Domain.Entities;

using DocumentFormat.OpenXml.Drawing.Charts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2p.WinForm.Mappers
{

        public static class OrderCache
        {
            public static List<A2POrder>? CachedOrders { get; private set; }

            public static void StoreOrder(List<A2POrder> orders)
            {
            CachedOrders = orders;
            }

            public static void Clear()
            {
            CachedOrders = null;
            }

            public static bool HasOrder => CachedOrders != null;
        }
    }
