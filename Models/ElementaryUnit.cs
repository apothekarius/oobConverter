using OobConverter.Enums;

namespace OobConverter.Models
{
    internal class ElementaryUnit {
        public string? Name { get; set; }
        public Keywords Keywords { get; set; }
        public int HardAttack { get; set; }
        public int HardRange { get; set; }
        public int SoftAttack { get; set; }
        public int SoftRange { get; set; }
        public int Defense { get; set; }
        public int AntiAirAttack { get; set; }
        public int AntiAirRange { get; set; }
        public int Assault { get; set; }
        public int Speed { get; set; }
        public int VictoryPoints { get; set; }
        public int Amount { get; set; }
        public UnitQuality Quality { get; set; }
        public string? Description { get; set; }
        public UnitType UnitType { get; set; }
        public MovementType MovementType { get; set; }
        public FormationType FormationType { get; set; }
        public CompositeUnit? ParentUnit { get; set; }
        public string? Nationality { get; set; }

        public int Depth => ParentUnit.Depth + 1;

        public override string ToString() {
            return $"{DisplayNameRenderer.Get(FormationType)};{Name};{Nationality};{Description};{DisplayNameRenderer.Get(UnitType)};{DisplayNameRenderer.Get(MovementType)};{Amount};{Quality};{HardAttack}/{HardRange};{SoftAttack}/{SoftRange};{AntiAirAttack}/{AntiAirRange};{Assault};{Defense};{Speed};{Keywords}";
        }
    }
}