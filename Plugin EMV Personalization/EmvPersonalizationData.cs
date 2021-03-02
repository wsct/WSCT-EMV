using WSCT.EMV.Card;
using WSCT.EMV.Personalization;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    internal record EmvPersonalizationData(
        EmvPersonalizationModel Model,
        WSCT.EMV.Personalization.EmvPersonalizationData Data,
        EmvIssuerContext IssuerContext,
        EmvIccContext IccContext,
        PINBlock PinBlock);
}
