using System.Collections.Generic;

namespace WSCT.GUI.Plugins.EMVExplorer.Services
{
    public sealed class EmvTreeNode
    {
        public string Label { get; set; } = string.Empty;
        public string? TagHex { get; set; }
        public string? Value { get; set; }
        public bool IsError { get; set; }
        public bool IsExpanded { get; set; } = true;
        public List<EmvTreeNode> Children { get; } = [];

        public EmvTreeNode(string label, bool isError = false)
        {
            Label = label;
            IsError = isError;
        }

        public void SetExpandedRecursive(bool expanded)
        {
            IsExpanded = expanded;
            foreach (var child in Children)
            {
                child.SetExpandedRecursive(expanded);
            }
        }
    }

    public enum EmvLogEntryKind
    {
        Normal,
        Header,
        Highlight,
        Error
    }

    public sealed record EmvDetailedLogEntry(string Text, EmvLogEntryKind Kind = EmvLogEntryKind.Normal);

    public sealed record EmvSignedDataItem(int Index, string Tag, string DataName, string Value);

    public sealed class EmvPublicKeysSnapshot
    {
        public string Aid { get; set; } = string.Empty;
        public string Rid { get; set; } = string.Empty;
        public string CaIndex { get; set; } = string.Empty;
        public string CaModulus { get; set; } = string.Empty;
        public string CaExponent { get; set; } = string.Empty;

        public string IssuerRecoveredData { get; set; } = string.Empty;
        public string IssuerHashResult { get; set; } = string.Empty;
        public string IssuerModulus { get; set; } = string.Empty;
        public string IssuerExponent { get; set; } = string.Empty;

        public string IccRecoveredData { get; set; } = string.Empty;
        public string IccHashResult { get; set; } = string.Empty;
        public string IccModulus { get; set; } = string.Empty;
        public string IccExponent { get; set; } = string.Empty;
    }

    public sealed class EmvSdaSnapshot
    {
        public string RecoveredData { get; set; } = string.Empty;
        public string HashResult { get; set; } = string.Empty;
        public string DataAuthenticationCode { get; set; } = string.Empty;
    }

    public sealed class EmvDdaSnapshot
    {
        public string RecoveredData { get; set; } = string.Empty;
        public string HashResult { get; set; } = string.Empty;
        public string IccDynamicData { get; set; } = string.Empty;
    }

    public sealed class EmvCardLogSnapshot
    {
        public bool IsPresent { get; set; }
        public string SfiHex { get; set; } = string.Empty;
        public string LengthHex { get; set; } = string.Empty;
        public string LogFormatText { get; set; } = string.Empty;
        public List<string> Columns { get; } = [];
        public List<List<string>> Rows { get; } = [];
        public bool CanSave => Rows.Count > 0;
    }

    public sealed class EmvTvrSnapshot
    {
        // Byte 1
        public bool OfflineDataAuthenticationNotPerformed { get; set; }
        public bool SdaFailed { get; set; }
        public bool IccDataMissing { get; set; }
        public bool TerminalExceptionFile { get; set; }
        public bool DdaFailed { get; set; }
        public bool CdaFailed { get; set; }

        // Byte 2
        public bool IccAndTerminalVersionsDifferent { get; set; }
        public bool ExpiredApplication { get; set; }
        public bool NotYetEffectiveApplication { get; set; }
        public bool ServiceNotAllowed { get; set; }
        public bool NewCard { get; set; }

        // Byte 3
        public bool CardholderVerificationFailed { get; set; }
        public bool UnrecognisedCvm { get; set; }
        public bool PinTryLimitExceeded { get; set; }
        public bool PinpadError { get; set; }
        public bool PinNotEntered { get; set; }
        public bool OnlinePinEntered { get; set; }

        // Byte 4
        public bool TransactionExceedFloorLimit { get; set; }
        public bool LowerConsecutiveOfflineLimitExceeded { get; set; }
        public bool UpperConsecutiveOfflineLimitExceeded { get; set; }
        public bool TransactionRandomlySelectedOnline { get; set; }
        public bool MerchantForcedTransactionOnline { get; set; }

        // Byte 5
        public bool DefaultTdolUsed { get; set; }
        public bool IssuerAuthenticationFailed { get; set; }
        public bool ScriptProcessingFailedBeforeGenerateAC { get; set; }
        public bool ScriptProcessingFailedAfterGenerateAC { get; set; }
    }

    public sealed record EmvNotification(string Title, string Message, string AlertClass);
}
