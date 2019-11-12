// <copyright file="UnitOfMeasure.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class UnitOfMeasure
    {
        public string GetName() => this.Name;

        // Quantity
        public bool Pack => Equals(this.UniqueId, UnitsOfMeasure.PackId);
        public bool Pair => Equals(this.UniqueId, UnitsOfMeasure.PairId);
        public bool Piece => Equals(this.UniqueId, UnitsOfMeasure.PieceId);
        public bool People => Equals(this.UniqueId, UnitsOfMeasure.PeopleId);

        // Length, width, distance, thickness
        public bool Millimeter => Equals(this.UniqueId, UnitsOfMeasure.MillimeterId);
        public bool Centimeter => Equals(this.UniqueId, UnitsOfMeasure.CentimeterId);
        public bool Meter => Equals(this.UniqueId, UnitsOfMeasure.MeterId);
        public bool Kilometer => Equals(this.UniqueId, UnitsOfMeasure.KilometerId);

        // Mass
        public bool Milligram => Equals(this.UniqueId, UnitsOfMeasure.MilligramId);
        public bool Gram => Equals(this.UniqueId, UnitsOfMeasure.GramId);
        public bool KilogramI => Equals(this.UniqueId, UnitsOfMeasure.KilogramId);
        public bool MetricTon => Equals(this.UniqueId, UnitsOfMeasure.MetricTonId);

        // Area
        public bool SquareMeter => Equals(this.UniqueId, UnitsOfMeasure.SquareMeterId);
        public bool Hectare => Equals(this.UniqueId, UnitsOfMeasure.HectareId);
        public bool SquareKilometer => Equals(this.UniqueId, UnitsOfMeasure.SquareKilometerId);

        // Volume
        public bool MilliLiter => Equals(this.UniqueId, UnitsOfMeasure.MilliLiterId);
        public bool CubicCentimeter => Equals(this.UniqueId, UnitsOfMeasure.CubicCentimeterId);
        public bool Liter => Equals(this.UniqueId, UnitsOfMeasure.LiterId);
        public bool CubicMeter => Equals(this.UniqueId, UnitsOfMeasure.CubicMeterId);

        // Velocity
        public bool MeterPerSecond => Equals(this.UniqueId, UnitsOfMeasure.MeterPerSecondId);
        public bool KilometerPerHour => Equals(this.UniqueId, UnitsOfMeasure.KilometerPerHourId);

        // Density
        public bool KilogramPerCubicMeter => Equals(this.UniqueId, UnitsOfMeasure.KilogramPerCubicMeterId);

        // Force
        public bool Newton => Equals(this.UniqueId, UnitsOfMeasure.NewtonId);

        // Pressure
        public bool KiloPascal => Equals(this.UniqueId, UnitsOfMeasure.KiloPascalId);

        // Power
        public bool Watt => Equals(this.UniqueId, UnitsOfMeasure.WattId);
        public bool KiloWatt => Equals(this.UniqueId, UnitsOfMeasure.KiloWattId);

        // Energy
        public bool KiloJoule => Equals(this.UniqueId, UnitsOfMeasure.KiloJouleId);
        public bool MegaJoule => Equals(this.UniqueId, UnitsOfMeasure.MegaJouleId);
        public bool KiloWattHour => Equals(this.UniqueId, UnitsOfMeasure.KiloWattHourId);

        // Temperature
        public bool DegreeCelsius => Equals(this.UniqueId, UnitsOfMeasure.DegreeCelsiusId);

        // Electric
        public bool Ampere => Equals(this.UniqueId, UnitsOfMeasure.AmpereId);
        public bool Volt => Equals(this.UniqueId, UnitsOfMeasure.VoltId);
    }
}
