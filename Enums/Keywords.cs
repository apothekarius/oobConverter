namespace OobConverter.Enums {
    [Flags]
    internal enum Keywords {
        None = 0,
        IndirectFire = 1,
        Mines = 2,
        SupplyDrop = 8, //spec
        Bridge = 16,
        Jet = 64,
        ChemicalNuclear = 128,
        NightFlying = 256,
        ThermalImagingSights = 1024,
        UnknownKeyword2048 = 2048,
        LowReliability = 4096
    }
}