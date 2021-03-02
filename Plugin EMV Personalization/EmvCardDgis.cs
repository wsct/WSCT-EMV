using System.Collections.Generic;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    internal record EmvCardDgis(string Fci, string Gpo, string Acid, List<string> Records, string Pin);
}
