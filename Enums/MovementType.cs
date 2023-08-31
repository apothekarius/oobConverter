using System.ComponentModel.DataAnnotations;

namespace OobConverter.Enums {
    internal enum MovementType {
        [Display(Name = "Airplane")] AIR,
        [Display(Name = "Helicopter")] HEL,
        [Display(Name = "Motorized")] MOT,
        [Display(Name = "Tracked")] TRK,
        [Display(Name = "Armored Car")] PM,
        [Display(Name = "Foot")] FT
    }
}