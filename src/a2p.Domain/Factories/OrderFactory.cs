using a2p.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2p.Domain.Factories
{

    public static class OrderFactory
    {
        public static Order FromPayload(
            string orderNumber,
            DateOnly orderDate,
            string customerTitle,
            string customerNumber,
            string? projectNumber,
            string deliveryAddress,
            DateOnly? finishProductionUntil,
            DateOnly? correctionAvailableUntil,
            string? responsibleManager)
        {
            // Validation logic
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentException("Order number is required");

            return new Order(
                orderNumber.Trim(),
                orderDate,
                customerTitle.Trim(),
                customerNumber.Trim(),
                string.IsNullOrWhiteSpace(projectNumber) ? null : projectNumber.Trim(),
                deliveryAddress.Trim(),
                finishProductionUntil,
                correctionAvailableUntil,
                string.IsNullOrWhiteSpace(responsibleManager) ? null : responsibleManager.Trim());
        }
    }
}
