using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OobConverter.Enums;

namespace OobConverter {
    internal static class DisplayNameRenderer {
        private static Dictionary<UnitType, string> _unittype;
        private static Dictionary<MovementType, string> _movementtype;
        private static Dictionary<FormationType, string> _formationtype;
        private static Dictionary<Keywords, string> _keywords;

        static DisplayNameRenderer() {
            _formationtype = Enum.GetValues<FormationType>().ToDictionary(x => x, x => typeof(FormationType).GetMember(x.ToString()).Single().GetCustomAttribute<DisplayAttribute>().Name);
            _movementtype = Enum.GetValues<MovementType>().ToDictionary(x => x, x => typeof(MovementType).GetMember(x.ToString()).Single().GetCustomAttribute<DisplayAttribute>().Name);
            _unittype = Enum.GetValues<UnitType>().ToDictionary(x => x, x => typeof(UnitType).GetMember(x.ToString()).Single().GetCustomAttribute<DisplayAttribute>().Name);
            _keywords = Enum.GetValues<Keywords>().ToDictionary(x => x, x => typeof(Keywords).GetMember(x.ToString()).Single().GetCustomAttribute<DisplayAttribute>().Name);
        }

        public static string Get<T>(T val) where T : Enum {
            return val switch {
                FormationType x => _formationtype.ContainsKey(x) ? _formationtype[x] : UnknownValue(x),
                MovementType x => _movementtype.ContainsKey(x) ? _movementtype[x] : UnknownValue(x),
                UnitType x => _unittype.ContainsKey(x) ? _unittype[x] : UnknownValue(x),
                Keywords x => _keywords.ContainsKey(x) ? _keywords[x] : UnknownValue(x),
                _ => throw new InvalidDataException()
            };
        }

        private static string UnknownValue<T>(T val) where T : Enum {
            return $"Unknown {typeof(T).Name} ({val})";
        }
    }
}