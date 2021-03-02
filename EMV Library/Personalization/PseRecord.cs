namespace WSCT.EMV.Personalization
{
    public record PseRecord(
        byte Index,
        string AdfName,
        string ApplicationLabel,
        string PreferredName,
        byte? PriorityIndicator,
        string DiscretionaryData
    );
}
