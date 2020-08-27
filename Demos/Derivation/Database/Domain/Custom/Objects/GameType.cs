namespace Allors.Domain
{
    public partial class GameType
    {
        public bool IsSoloSlim => this.UniqueId == GameTypes.SoloSlimId;

        public bool IsMisery => this.UniqueId == GameTypes.MiseryId;
        public bool IsOpenMisery => this.UniqueId == GameTypes.OpenMiseryId;
        public bool IsSolo => this.UniqueId == GameTypes.SoloId;
        public bool IsSmallSlam => this.UniqueId == GameTypes.SmallSlamId;
        public bool IsProposalAndAcceptance => this.UniqueId == GameTypes.ProposalAndAcceptanceId;
        public bool IsAbondance => this.UniqueId == GameTypes.AbondanceId;
        public bool IsTrull => this.UniqueId == GameTypes.TrullId;
    }
}
