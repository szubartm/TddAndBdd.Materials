namespace Common
{
    public enum Side
    {
        Buy = 0,
        Sell = 1
    }

    public enum TimeInForce
    {
        Day = 1,
        IOC = 2,
        FOK = 3
    }

    public enum State
    {
        UNACK = 0,
        LIVE,
        PMOD,
        PFILLED,
        FILLED,
        PCANCEL,
        CANCELLED,
        REJECTED,
        CLOSED
    }

    public enum OrderChangeType
    {
        Rejection,
        Cancellation,
        Modification,
        Acknowledge,
        Fill
    }
}