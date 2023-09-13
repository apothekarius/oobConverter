using System.ComponentModel.DataAnnotations;

namespace OobConverter.Enums {
    [Flags]
    internal enum Keywords {
        [Display(Name = "")] None = 0,
        [Display(Name = "IF")] IndirectFire = 1,
        [Display(Name = "Mines")] Mines = 2,
        [Display(Name = "Para")] Para = 8,
        [Display(Name = "Bridge")] Bridge = 16,
        [Display(Name = "Jet")] Jet = 64,
        [Display(Name = "WMD")] ChemicalNuclear = 128,
        [Display(Name = "Night Flying")] NightFlying = 256,
        [Display(Name = "Thermal IS")] ThermalImagingSights = 1024,
        [Display(Name = "Low Reliability")] LowReliability = 4096
    }
}