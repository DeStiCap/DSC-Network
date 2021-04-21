namespace DSC.Network
{
    public enum DSC_NetworkEventType
    {
        StartNetwork        =   1 << 0,
        StopNetwork         =   1 << 1,
        NetworkStart        =   1 << 2,
        StartConnectTimeout =   1 << 3,
        Disconnect          =   1 << 4
    }
}
