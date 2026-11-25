using System;
using WSCT.EMV.Card;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer.Services
{
    internal sealed class EmvDetailedLogObserver
    {
        private readonly Action<string, EmvLogEntryKind> _appendLog;
        private readonly TlvDictionary? _tlvDictionary;

        public EmvDetailedLogObserver(Action<string, EmvLogEntryKind> appendLog, TlvDictionary? tlvDictionary)
        {
            _appendLog = appendLog;
            _tlvDictionary = tlvDictionary;
        }

        public void ObserveEmv(EmvApplication emv)
        {
            emv.BeforeSelectEvent += BeforeApplicationSelection;
            emv.AfterSelectEvent += AfterApplicationSelection;

            emv.BeforeGetProcessingOptionsEvent += BeforeGetProcessingOptions;
            emv.AfterGetProcessingOptionsEvent += AfterGetProcessingOptions;

            emv.BeforeReadApplicationDataEvent += BeforeReadApplicationData;
            emv.AfterReadApplicationDataEvent += AfterReadApplicationData;

            emv.BeforeGetDataEvent += BeforeGetData;
            emv.AfterGetDataEvent += AfterGetData;

            emv.BeforeReadLogFileEvent += BeforeReadLogFile;
            emv.AfterReadLogFileEvent += AfterReadLogFile;

            emv.BeforeVerifyPinEvent += BeforeVerifyPin;
            emv.AfterVerifyPinEvent += AfterVerifyPin;

            emv.BeforeInternalAuthenticateEvent += BeforeInternalAuthenticate;
            emv.AfterInternalAuthenticateEvent += AfterInternalAuthenticate;

            emv.BeforeGetChallengeEvent += BeforeGetChallenge;
            emv.AfterGetChallengeEvent += AfterGetChallenge;

            emv.BeforeGenerateAC1Event += BeforeGenerateAc1;
            emv.AfterGenerateAC1Event += AfterGenerateAc1;
        }

        public void ObservePse(PaymentSystemEnvironment pse)
        {
            pse.BeforeSelectEvent += BeforePseSelection;
            pse.AfterSelectEvent += AfterPseSelection;

            pse.BeforeReadEvent += BeforePseRead;
            pse.AfterReadEvent += AfterPseRead;
        }

        private void BeforePseSelection(object? sender, EmvEventArgs eventArgs)
        {
            _appendLog("P S E   S e l e c t i o n", EmvLogEntryKind.Header);
        }

        private void AfterPseSelection(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvDefinitionFile df)
            {
                return;
            }

            if (df.TlvFci is not null)
            {
                _appendLog($"  >> TLV: {df.TlvFci}", EmvLogEntryKind.Normal);
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv);
                }
            }
            else
            {
                _appendLog("  >> No PSE found", EmvLogEntryKind.Normal);
            }
        }

        private void BeforePseRead(object? sender, EmvEventArgs eventArgs)
        {
            _appendLog("P S E   R e a d", EmvLogEntryKind.Header);
        }

        private void AfterPseRead(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not PaymentSystemEnvironment pse)
            {
                return;
            }

            foreach (var record in pse.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv);
                }
            }

            _appendLog("PSE Applications found:", EmvLogEntryKind.Normal);

            foreach (var emvFound in pse.GetApplications())
            {
                var label = emvFound.TlvFromPSE is not null && emvFound.TlvFromPSE.HasTag(0x50)
                    ? _tlvDictionary?.CreateInstance(emvFound.TlvFromPSE.GetTag(0x50))?.ToString() ?? string.Empty
                    : string.Empty;
                _appendLog($"  >> Application '{emvFound.Aid}' [ {label} ] found", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeApplicationSelection(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvDefinitionFile df)
            {
                return;
            }

            _appendLog($"E M V   A I D   S e l e c t i o n   {df.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterApplicationSelection(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvDefinitionFile df)
            {
                return;
            }

            if (df.TlvFci is not null)
            {
                _appendLog($"  >> TLV: {df.TlvFci}", EmvLogEntryKind.Normal);
                foreach (TlvData tlv in df.TlvFci.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv);
                }
            }
            else
            {
                _appendLog("  >> AID not found", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeGetProcessingOptions(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   G e t P r o c e s s i n g O p t i o n s   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterGetProcessingOptions(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv || emv.TlvProcessingOptions is null)
            {
                return;
            }

            foreach (TlvData tlv in emv.TlvProcessingOptions.GetTags())
            {
                WriteTlv(tlv.Tag, tlv);
            }

            _appendLog($"    >> {emv.Aip}", EmvLogEntryKind.Normal);
            foreach (var file in emv.Afl.Files)
            {
                _appendLog($"    >> {file}", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeReadApplicationData(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   R e a d A p p l i c a t i o n D a t a   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterReadApplicationData(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            foreach (var record in emv.TlvRecords)
            {
                foreach (TlvData tlv in record.GetTags())
                {
                    WriteTlv(tlv.Tag, tlv);
                }
            }

            if (emv.Sda is not null)
            {
                _appendLog($"SDA: {emv.Sda}", EmvLogEntryKind.Normal);
            }
            else
            {
                _appendLog("SDA not supported or unknown Certification Authority", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeGetData(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   G e t D a t a   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterGetData(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            if (emv.TlvATC is not null)
            {
                WriteTlv(emv.TlvATC.Tag, emv.TlvATC);
            }
            if (emv.TlvLastOnlineATCRegister is not null)
            {
                WriteTlv(emv.TlvLastOnlineATCRegister.Tag, emv.TlvLastOnlineATCRegister);
            }
            if (emv.TlvPINTryCounter is not null)
            {
                WriteTlv(emv.TlvPINTryCounter.Tag, emv.TlvPINTryCounter);
            }
            if (emv.TlvLogFormat is not null)
            {
                WriteTlv(emv.TlvLogFormat.Tag, emv.TlvLogFormat);
            }
        }

        private void BeforeReadLogFile(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   R e a d L o g F i l e   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterReadLogFile(object? sender, EmvEventArgs eventArgs)
        {
        }

        private void BeforeVerifyPin(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   V e r i f y P i n   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterVerifyPin(object? sender, EmvEventArgs eventArgs)
        {
        }

        private void BeforeInternalAuthenticate(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   I n t e r n a l A u t h e n t i c a t e   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterInternalAuthenticate(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            if (emv.TlvSignedDynamicApplicationResponse is not null)
            {
                WriteTlv(emv.TlvSignedDynamicApplicationResponse.Tag, emv.TlvSignedDynamicApplicationResponse);
            }

            if (emv.Dda is not null)
            {
                _appendLog($"DDA: {emv.Dda}", EmvLogEntryKind.Normal);
            }
            else
            {
                _appendLog("DDA not supported or unknown Certification Authority", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeGetChallenge(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   G e t C h a l l e n g e   {emv.Aid}", EmvLogEntryKind.Header);
        }

        private void AfterGetChallenge(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            if (emv.IccChallenge is not null)
            {
                _appendLog($"    >> {emv.IccChallenge.ToHexa()}", EmvLogEntryKind.Normal);
            }
        }

        private void BeforeGenerateAc1(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            _appendLog($"E M V   G e n e r a t e A C 1   {emv.Aid}", EmvLogEntryKind.Header);
            _appendLog($"    >> Requested AC: {emv.RequestedAC1Type}", EmvLogEntryKind.Normal);
            _appendLog($"    >> Unpredictable Number: {emv.TlvGenerateAC1UnpredictableNumber}", EmvLogEntryKind.Normal);
        }

        private void AfterGenerateAc1(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv || emv.TlvGenerateAC1Response is null)
            {
                return;
            }

            WriteTlv(emv.TlvGenerateAC1Response.Tag, emv.TlvGenerateAC1Response);
            _appendLog($"    >> Cryptogram Information Data: {emv.Cid1}", EmvLogEntryKind.Normal);
            _appendLog($"    >> Application Cryptogram Counter: {emv.AtcFromAC1}", EmvLogEntryKind.Normal);
        }

        private void WriteTlv(uint tagId, TlvData tlv)
        {
            _appendLog($"  >> TLV {tagId:X2}: [ {tlv.GetTag(tagId)} ]", EmvLogEntryKind.Normal);

            if (_tlvDictionary is null || !tlv.HasTag(tagId))
            {
                return;
            }

            var instance = _tlvDictionary.CreateInstance(tlv.GetTag(tagId));
            if (instance is null)
            {
                return;
            }

            _appendLog($"     >> {instance:N}: {instance}", EmvLogEntryKind.Highlight);
        }
    }
}
