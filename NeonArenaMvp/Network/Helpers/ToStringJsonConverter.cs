//using System.Text.Json.Serialization;

//namespace NeonArenaMvp.Network.Helpers
//{
//    public class ToStringJsonConverter : JsonConverter<Coords>
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return true;
//        }

//        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//        {
//            writer.WriteValue(value?.ToString());
//        }

//        public override bool CanRead
//        {
//            get { return false; }
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
