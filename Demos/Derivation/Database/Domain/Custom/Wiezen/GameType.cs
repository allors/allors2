using System;
using System.Collections.Generic;

namespace Allors.Domain
{
    public partial class GameType
    {
        public bool IsSoloSlim
        {
            get { return this.UniqueId == GameTypes.SoloSlimId; }
        }

        public bool IsMiserie
        {
            get { return this.UniqueId == GameTypes.MiserieId; }
        }
        public bool IsMiserieOpTafel
        {
            get { return this.UniqueId == GameTypes.MiserieOpTafelId; }
        }
        public bool IsSolo
        {
            get { return this.UniqueId == GameTypes.SoloId; }
        }
        public bool IsAlleenGaan
        {
            get { return this.UniqueId == GameTypes.AlleenGaanId; }
        }
        public bool IsVragenEnMeegaan
        {
            get { return this.UniqueId == GameTypes.VragenEnMeegaanId; }
        }
        public bool IsAbondance
        {
            get { return this.UniqueId == GameTypes.AbondanceId; }
        }
        public bool IsTroel
        {
            get { return this.UniqueId == GameTypes.TroelId; }
        }
    }
}
