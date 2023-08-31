using System.ComponentModel.DataAnnotations;

namespace OobConverter.Enums {
    internal enum UnitType {
        [Display(Name = "Fighter")] FI,
        [Display(Name = "Bomber")] BO,
        [Display(Name = "Reconnaissance A/C")] RA,
        [Display(Name = "Headquarters")] HQ,
        [Display(Name = "Artillery")] ART,
        [Display(Name = "Reconnaissance")] REC,
        [Display(Name = "Anti-Air")] AA,
        [Display(Name = "Engineers")] ENG,
        [Display(Name = "Armor")] ARM,
        [Display(Name = "Infantry")] INF,
        [Display(Name = "Guided Missile")] GM,
        [Display(Name = "Rocket Artillery")] ROC,
        [Display(Name = "Anti-Tank")] AT,
        [Display(Name = "Airborne")] PAR,
        [Display(Name = "Supply")] SUP
    }
}