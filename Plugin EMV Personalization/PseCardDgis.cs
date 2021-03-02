using System.Collections.Generic;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    internal record PseCardDgis(string Fci, List<string> Records);
}
