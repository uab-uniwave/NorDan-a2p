namespace a2p.Domain.Entities;

using System;
using System.Collections.Generic;

public sealed class Order
{
    public Guid Id { get; private set; }

    /// <summary>Business order number (e.g. "2507Z02751/2").</summary>
    public string OrderNumber { get; private set; }

    /// <summary>Date when order was created/received.</summary>
    public DateOnly OrderDate { get; private set; }

    /// <summary>Customer display name (e.g. "NorDan AS").</summary>
    public string CustomerTitle { get; private set; }

    /// <summary>Customer unique number (e.g. "979776233").</summary>
    public string CustomerNumber { get; private set; }

    /// <summary>Optional external project number (e.g. "NO.2428167").</summary>
    public string? ProjectNumber { get; private set; }

    /// <summary>Full delivery address text.</summary>
    public string DeliveryAddress { get; private set; }

    /// <summary>Planned finish of production.</summary>
    public DateOnly? FinishProductionUntil { get; private set; }

    /// <summary>Deadline for making corrections.</summary>
    public DateOnly? CorrectionAvailableUntil { get; private set; }

    /// <summary>Responsible manager for this order.</summary>
    public string? ResponsibleManager { get; private set; }

    
    public List<String> Files  { get; private set; }
    public DateTime CreatedUtc { get; private set; }
    public DateTime ModifiedUtc { get; private set; }

    private Order() { } // for serializers / EF

    private Order(
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
        Id = Guid.NewGuid();
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        CustomerTitle = customerTitle;
        CustomerNumber = customerNumber;
        ProjectNumber = projectNumber;
        DeliveryAddress = deliveryAddress;
        FinishProductionUntil = finishProductionUntil;
        CorrectionAvailableUntil = correctionAvailableUntil;
        ResponsibleManager = responsibleManager;

        CreatedUtc = DateTime.UtcNow;
        ModifiedUtc = CreatedUtc;
    }



    public void UpdateDates(DateOnly? finishProdUntil, DateOnly? correctionUntil)
    {
        FinishProductionUntil = finishProdUntil;
        CorrectionAvailableUntil = correctionUntil;
        Touch();
    }

    private void Touch() => ModifiedUtc = DateTime.UtcNow;
}