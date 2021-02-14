using WSCT.EMV.Personalization;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    internal record CardData(EmvPersonalizationModel Model, EmvPersonalizationData Data, EmvIssuerContext IssuerContext, EmvIccContext IccContext);
}
