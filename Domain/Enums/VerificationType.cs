using System.ComponentModel;

namespace InfinityNetServer.BuildingBlocks.Domain.Enums
{
    public enum VerificationType
    {
        [Description("VerifyByCode")]
        VerifyByCode,

        [Description("VerifyByToken")]
        VerifyByToken,

        [Description("ResetByCode")]
        ResetByCode,

        [Description("ResetByToken")]
        ResetByToken
    }
}
