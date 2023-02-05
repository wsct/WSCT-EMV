using WSCT.EMV.Card;
using WSCT.EMV.Personalization;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    internal record EmvApplicationInformation(
        EmvPersonalizationModel Model,
        EmvPersonalizationData Data,
        EmvIssuerContext IssuerContext,
        EmvIccContext IccContext,
        PINBlock PinBlock);
}
