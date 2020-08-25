using System;
using System.Collections.Generic;

namespace Allors.Domain
{
    public partial class GameTypes
    {
        public static readonly Guid SoloSlimId = new Guid("7a369e7f-4eec-4023-8174-a52cab4a4d79");
        public static readonly Guid MiserieId = new Guid("b24d304d-6f5b-4f21-b6cd-3fac42dbf87d");
        public static readonly Guid MiserieOpTafelId = new Guid("ad662e27-c3ab-462d-9dda-20d570a25783");
        public static readonly Guid AlleenGaanId = new Guid("f82471af-ee9d-424f-9919-bbfb656e4cdf");
        public static readonly Guid SoloId = new Guid("a0c2ad9b-07cc-4cf5-8672-0747bcc1bea3");
        public static readonly Guid VragenEnMeegaanId = new Guid("c3d3eb58-d98d-45fc-af69-3f8da567e7ec");
        public static readonly Guid TroelId = new Guid("a8f35a76-7b7d-43f9-9514-681fef1d566f");
        public static readonly Guid AbondanceId = new Guid("323b16d4-49ad-46d6-9ff5-f65c6f4c5675");

        private UniquelyIdentifiableSticky<GameType> cache;

        public GameType SoloSlim => this.Cache[SoloSlimId];

        public GameType Miserie => this.Cache[MiserieId];

        public GameType MiserieOpTafel => this.Cache[MiserieOpTafelId];

        public GameType AlleenGaan => this.Cache[AlleenGaanId];

        public GameType Solo => this.Cache[SoloId];

        public GameType VragenEnMeegaan => this.Cache[VragenEnMeegaanId];

        public GameType Troel => this.Cache[TroelId];

        public GameType Abondance => this.Cache[AbondanceId];


        private UniquelyIdentifiableSticky<GameType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<GameType>(this.Session));

        protected override void CustomSetup(Setup setup)
        {
            CreateGameType("Solo slim", SoloSlimId);
            CreateGameType("Miserie", MiserieId);
            CreateGameType("Miserie op tafel", MiserieOpTafelId);
            CreateGameType("Alleen gaan", AlleenGaanId);
            CreateGameType("Solo", SoloId);
            CreateGameType("Vragen en meegaan", VragenEnMeegaanId);
            CreateGameType("Troel", TroelId);
            CreateGameType("Abondance", AbondanceId);
        }

        private GameType CreateGameType(string name, Guid uniqueId)
        {
            return new GameTypeBuilder(this.Session)
                            .WithName(name)
                            .WithUniqueId(uniqueId)
                            .WithIsActive(true)
                            .Build();
        }
    }
}
