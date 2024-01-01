using System.Reflection;

namespace ByReplace.Test.Common.Helpers
{
    internal static class FileInspect<T>
    {
        public static Type Type => typeof(T);

        public static object[] GetAttributes(bool inherit = false)
        {
            return Type.GetCustomAttributes(inherit);
        }

        public static ConstructorInfo[] GetConstructors()
        {
            return Type.GetConstructors();
        }

        public static PropertyInfo[] GetProperties()
        {
            return Type.GetProperties();
        }

        public static Type[] GetInterfaces()
        {
            return Type.GetInterfaces();
        }
    }
}
