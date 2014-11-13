using System;
using WSCT.EMV.Personalization;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.CardPersonalisationConsole
{
    class Program
    {
        private const string OutputJsonFileName = "emv-cardpersonalisation-dgi.json";

        static void Main(/*string[] args*/)
        {
            RegisterPcl.Register();

            Console.WriteLine("wsct-emvcp :: EMV Card Personalisation");

            var model = @"emv-card-model.json".CreateFromJsonFile<EmvPersonalizationModel>();
            var data = @"emv-card-data.json".CreateFromJsonFile<EmvPersonalizationData>();
            var context = @"emv-issuer-context.json".CreateFromJsonFile<EmvIssuerContext>();

            var builder = new DgiBuilder(model, data, context);

            var container = new DgiContainer();

            try
            {
                Console.WriteLine("Building DGI for FCI");
                var dgiFci = builder.GetDgi(model.Fci);
                Console.WriteLine("FCI: {0}", dgiFci);
                container.Fci = dgiFci;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                Console.WriteLine("Building DGI for GPO");
                var dgiGpo = builder.GetDgi(model.Gpo);
                Console.WriteLine("GPO: {0}", dgiGpo);
                container.Gpo = dgiGpo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var record in model.Records)
            {
                try
                {
                    Console.WriteLine("Building DGI for Record {0}.{1}", record.Sfi, record.Index);
                    var dgiRecord = builder.GetDgi(record);
                    Console.WriteLine("DGI for Record {0}.{1}: {2}", record.Sfi, record.Index, dgiRecord);
                    container.Records.Add(dgiRecord);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine("Saving DGI in {0} file", OutputJsonFileName);
            container.WriteToJsonFile(OutputJsonFileName, true);
        }
    }
}
