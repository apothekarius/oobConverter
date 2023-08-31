namespace OobConverter.Models {
    internal class RawUnitData {
        public string? UnitData { get; set; }
        public List<RawUnitData>? Subordinates { get; set; }
        public RawUnitData? Parent { get; set; }
        public bool HasNationality { get; set; }
        public bool IsComposite => Subordinates != null;
        public bool IsElementary => Subordinates == null;
        public bool IsRoot => Parent == null;
    }
}