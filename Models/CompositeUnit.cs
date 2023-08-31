using OobConverter.Enums;

namespace OobConverter.Models {
    internal class CompositeUnit {
        public string? Name { get; set; }
        public FormationType FormationType { get; set; }
        public List<CompositeUnit>? SubordinateComposites { get; set; }
        public List<ElementaryUnit>? SubordinateElementaries { get; set; }
        public CompositeUnit? ParentUnit { get; set; }
        public string? Nationality { get; set; }

        public override string ToString() {
            return $"{Name};{DisplayNameRenderer.Get(FormationType)}";
        }
    }
}