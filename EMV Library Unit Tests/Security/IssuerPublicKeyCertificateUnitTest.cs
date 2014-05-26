using NUnit.Framework;
using WSCT.Helpers.Security;

namespace WSCT.EMV.Security
{
    [TestFixture]
    internal class IssuerPublicKeyCertificateUnitTest
    {
        private PuTTyPrivateKeyBody acPrivateKey = new PuTTyPrivateKeyBody(
            "AAABAArPQSPazk967rCtnqNPJAl2cD2oakHKkximIqcmft6OuCKOjMmn8kueqkBmTtOpsaXLLaNKhD7/jtzTjLZh0N1F/YJY6qgbdgvs6zIllIPYX7uW7rq4CIBOP9S6lhFbt/5l6t/+EQqQrXrTOh0hiGMuTkbzAq1L6fsd8QtWTnfQRYddWpsUefl3AVh2oT2U6bpHovaB+rJjbp0D5yaccX7e8c2o9oU3mGqt6ll9POad5hCAXfOxaeOSTNnD9nblwp25uQRsm1i2neA41XeGu3JTgjImo3itx4lqAV1dJ8+1mr4GKT9g68IslkDkUj4SuSS36C5i23pASr6JQFRZnmEAAACBAO6ZYlAmIe4c2a4CPf4smUOt89PlNc6QbfcfcXxCbIz7esfTTAgAUFgaesgj6GKTgnOfVpLA5WbxyWj2bKYjXE5ps8D7p495VD9ANviuhog3spPkN8j1NLn7NcvVQxEUFlHolFtzt+vq+NkYloR/cFJwD0cuZi2OB+nbui9zQSFjAAAAgQCPCoGhRH39hj9lM8gBLfEWBy+UUSZkdbDY+J6SswoA9yC+DrSKROZY8/iWFm5NB3eaVwBe3o4g/6RKIuZnCPidh+tDqg8zSd0Bk7sEUrXXbAYODMEVK/kS/XdIQ4CHhblAiKfuMHQGRBhfyYnmwhnZud0pUTVYGq9/U0W2tkRWNwAAAIEA2C3XNK0PR+MtE+a3Fl/ydDDTyNgyUvt3SteQPFHacaI/ZVtbo1RNJRuXHTeshk5S+Sl809KQYCHChstB77+5bwDK/YqIWcqBQl79uTZAgi+MpuoUXaJ+G9f7e9EFGoraNaV+CN8+pIDYrehpzDUjEPUmqETKtxd9CZeivhtet2g=");

        private Ssh1PublicKeyBody acPublicKey = new Ssh1PublicKeyBody(
            "AAAAB3NzaC1yc2EAAAABJQAAAQEAhVF4ujVF1EF/2F1PM3q8dLS9ox0egMJrhVZWDYVyDjWJqjNzDGysT0+JGkMhh9eO/MndiJcJs6U3TjEcyWEP/l82nPNOGVKv6Gn/anonr2yctEWAVDMTg8UTP6flgMCH7D5QypLSLPhblYJ3Z0g8xzsawF/LrqhFw8ac4ShycF+B1A35jfHL/SojzmfD/LbxrpbswUnxnj55qJWHJwAFugPWynnIBY8I3NRTNLSet0AjbIYjB6pMks9m7G6XkWahiubuhvI+s/2GoVmbGLoSJb6SW4ATnDe/Qh3PmECDm49cQ4hGXIH4ieHLV8sMPxvCRB31Zl7DNyWtskdU5IFuRQ==");

        private Ssh1PublicKeyBody issuerPublicKey = new Ssh1PublicKeyBody(
            "AAAAB3NzaC1yc2EAAAABJQAAAQEAhVF4ujVF1EF/2F1PM3q8dLS9ox0egMJrhVZWDYVyDjWJqjNzDGysT0+JGkMhh9eO/MndiJcJs6U3TjEcyWEP/l82nPNOGVKv6Gn/anonr2yctEWAVDMTg8UTP6flgMCH7D5QypLSLPhblYJ3Z0g8xzsawF/LrqhFw8ac4ShycF+B1A35jfHL/SojzmfD/LbxrpbswUnxnj55qJWHJwAFugPWynnIBY8I3NRTNLSet0AjbIYjB6pMks9m7G6XkWahiubuhvI+s/2GoVmbGLoSJb6SW4ATnDe/Qh3PmECDm49cQ4hGXIH4ieHLV8sMPxvCRB31Zl7DNyWtskdU5IFuRQ==");

        [Test]
        public void Test()
        {
        }
    }
}