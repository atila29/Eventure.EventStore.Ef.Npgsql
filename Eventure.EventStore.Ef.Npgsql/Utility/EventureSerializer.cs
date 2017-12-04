using System.IO;

namespace Eventure.EventStore.Ef.Npgsql.Utility
{
    public static class EventureSerializer
    {
        public static object Deserialize(byte[] arrBytes)
        {
            using (var ms = new MemoryStream())
            {
                var binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(ms);

                return obj;
            }
        }
        
        public static byte[] Serialize(object obj)
        {
            if(obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] arrBytes) where T : class => Deserialize(arrBytes) as T;
    }
}