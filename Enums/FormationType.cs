using System.ComponentModel.DataAnnotations;

namespace OobConverter.Enums {
    internal enum FormationType {
        [Display(Name = "Army Group")] AG,
        [Display(Name = "Army")] A,
        [Display(Name = "Corps")] K,
        [Display(Name = "Division")] D,
        [Display(Name = "Brigade")] G,
        [Display(Name = "Regiment")] R,
        [Display(Name = "Battalion")] B,
        [Display(Name = "Company")] C,
        [Display(Name = "Platoon")] P
    }
}