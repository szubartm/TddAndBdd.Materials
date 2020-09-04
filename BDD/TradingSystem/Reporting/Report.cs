using System;
using Common;

namespace TradingSystem.Reporting
{
    public class Report
    {
        public string Message { get; protected set; }
        public DateTime TimeStamp { get; } = DateTime.Now;
    }

    public class OrderReceivedReport : Report
    {
        public OrderReceivedReport(IOrder order)
        {
            Message = $"ClientOrder received: {order}";
        }
    }

    public class NewOrderProcessedReport : Report
    {
        public NewOrderProcessedReport(IOrder order)
        {
            Message = $"ClientOrder processed: {order}";
        }
    }

    public class OrderValidationFailed : Report
    {
        public OrderValidationFailed(IOrder order)
        {
            Message = $"ClientOrder validation failed: {order}";
        }
    }

    public class OrderFinishedProcessing : Report
    {
        public OrderFinishedProcessing(IOrder order)
        {
            Message = $"ClientOrder finished processing: {order}";
        }
    }
}