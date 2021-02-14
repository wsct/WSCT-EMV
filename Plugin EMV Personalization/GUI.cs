using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSCT.EMV.Commands;
using WSCT.EMV.Personalization;
using WSCT.Helpers;
using WSCT.Helpers.Json;
using WSCT.ISO7816;
using WSCT.Wrapper;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    /// <summary>
    /// Plugin dedicated form.
    /// </summary>
    public partial class Gui : Form
    {
        private const string DefaultFolder = "Personalization";
        private string PersonalizationFolder { get => guiFolderBrowserDialog.SelectedPath; }

        #region >> Constructor

        /// <inheritdoc />
        public Gui()
        {
            InitializeComponent();

            Icon = Common.Resources.Icons.WSCT;

            guiFolderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();
            if (Directory.Exists(DefaultFolder))
            {
                guiFolderBrowserDialog.SelectedPath = Path.Combine(Directory.GetCurrentDirectory(), DefaultFolder);
            }
        }

        #endregion

        #region >> gui * Click

        private async void guiDoRunPersonalization_Click(object sender, EventArgs e)
        {
            guiLogs.Clear();

            var emvAidAsString = guiEMVAppletAID.Text;
            byte[] emvAid;

            LogActionStart($"Verifying AID: {emvAidAsString}");

            try
            {
                emvAid = emvAidAsString.FromHexa();

                LogSuccess();
            }
            catch (Exception _)
            {
                LogFailure("The AID is malformed");
                return;
            }


            var cardData = await Task.Run(LoadPersonalizationFolder);

            var cardDgis = await Task.Run(() => BuildDgi(cardData));

            await Task.Run(() => TransmitToApplet(cardDgis, emvAid));
        }

        private void guiDoSelectPersonalizationFolder_ClickAsync(object sender, EventArgs e)
        {
            if (guiFolderBrowserDialog.ShowDialog() == DialogResult.OK && guiFolderBrowserDialog.SelectedPath != "")
            {
            }
        }

        #endregion

        private CardData LoadPersonalizationFolder()
        {
            var model = LoadDataFromFile<EmvPersonalizationModel>(@"emv-card-model.json");
            var data = LoadDataFromFile<EmvPersonalizationData>(@"emv-card-data.json");
            var issuerContext = LoadDataFromFile<EmvIssuerContext>(@"emv-issuer-context.json");
            var iccContext = LoadDataFromFile<EmvIccContext>(@"emv-icc-context.json");

            return new CardData(model, data, issuerContext, iccContext);
        }

        private CardDgis BuildDgi(CardData cardData)
        {
            var builder = new DgiBuilder(cardData.Model, cardData.Data, cardData.IssuerContext, cardData.IccContext);

            var fci = builder.BuildDgi(cardData.Model.Fci);
            var gpo = builder.BuildDgi(cardData.Model.Gpo);
            var acid = builder.BuildDgi(cardData.Model.Acid);

            var records = new List<string>();
            foreach (var record in cardData.Model.Records)
            {
                LogActionStart($"Building DGI Record '{record.Sfi}.{record.Index}': ");

                try
                {
                    records.Add(builder.BuildDgi(record));

                    LogSuccess();
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }

            return new CardDgis(fci, gpo, acid, records);
        }

        private T LoadDataFromFile<T>(string fileName)
        {
            T result = default;

            LogActionStart($"Loading '{fileName}': ");

            try
            {
                result = Path.Combine(PersonalizationFolder, fileName).CreateFromJsonFile<T>();

                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return result;
        }

        private void TransmitToApplet(CardDgis cardDgis, byte[] emvAid)
        {
            LogActionStart($"SELECT {emvAid}");
            TransmitToCard(new EMVSelectByNameCommand(emvAid));

            LogActionStart($"STORE DATA (FCI)");
            TransmitToCard(new EMVStoreDataCommand(0, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Fci));

            LogActionStart($"STORE DATA (GPO)");
            TransmitToCard(new EMVStoreDataCommand(1, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Gpo));

            LogActionStart($"STORE DATA (ACID)");
            TransmitToCard(new EMVStoreDataCommand(2, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Acid));

            // Records
            byte dgiId = 2;
            foreach (var record in cardDgis.Records)
            {
                dgiId++;
                LogActionStart($"STORE DATA (Record {record.Substring(0, 4)}");
                TransmitToCard(new EMVStoreDataCommand(dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, record));
            }

            LogActionStart($"STORE DATA (PIN)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, "8010021234"));

            LogActionStart($"STORE DATA (KEY1)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, "810181801B7275A03FD397B2E5B14E4E9ADFBB491AA2F2CD28F55E623FEED0E564576351157EAE3E94505999DF9E6AA457B977F4A36AFA54FB52ECA0FB373608E48E545B716C25DCC8CF4343490A500A8DF26A2D81777969D4F584842E771BA36563EB63B7EBC87AEA35FC3A208D6A1D01795E326873597FCFD0CB7339A889A3075EF921"));

            LogActionStart($"STORE DATA (KEY2)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, "81038180986965A7274E4165C127C6847AF8EA7ED5CBDAA10F46BF192C70BEB5B83B355A70E7FFB941BCE0440C7A7E552D4256F8B427C0F7395A9301D8841FBC6186CD087F44AD46BC322811ECD90A5FF1B9F93229BA8F85F9C1C5B791DC02073034853D1F89B9E3D9543C1803EAFF85C340AB6C0A352D626B56B961DC88A41FF6E1562D"));
        }

        private void TransmitToCard(CommandAPDU command)
        {
            try
            {
                var crp = new CommandResponsePair(command);
                var errorCode = crp.Transmit(SharedData.CardChannel);

                if (errorCode != ErrorCode.Success)
                {
                    LogFailure($"ErrorCode={errorCode}");
                }
                else if (crp.RApdu.StatusWord != 0x9000)
                {
                    LogFailure($"SW={crp.RApdu.StatusWord:X4}");
                }
                else
                {
                    LogSuccess();
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        private void LogActionStart(string message)
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogActionStart(message)));
                return;
            }

            guiLogs.AppendText($"{message}: ");
        }

        private void LogException(Exception exception)
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogException(exception)));
                return;
            }

            guiLogs.AppendText($"Failure {Environment.NewLine}");
            guiLogs.AppendText($"{exception.Message}{Environment.NewLine}");
        }

        private void LogFailure(string message)
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogFailure(message)));
                return;
            }

            guiLogs.SelectionColor = Color.DarkRed;
            guiLogs.AppendText($"Failure {Environment.NewLine}");
            guiLogs.AppendText($"{message}{Environment.NewLine}");
        }

        private void LogSuccess()
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(LogSuccess));
                return;
            }

            guiLogs.SelectionColor = Color.DarkGreen;
            guiLogs.Invoke(new MethodInvoker(() => guiLogs.AppendText($"Success {Environment.NewLine}")));
        }
    }
}