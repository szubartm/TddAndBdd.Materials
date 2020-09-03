using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Behavioral.Tools.SeparateProcess
{
    public static class Tools
    {
        public static StringContent PrepareStringContent<T>(T item)
        {
            return new StringContent(
                JsonSerializer.Serialize(item),
                Encoding.Unicode,
                "application/json");
        }
    }
}