using System.Reflection;

namespace FastEndpoints;

public static class EndpointScanner
{
    public static IEnumerable<Type> GetAllEndpointTypes()
    {
        return Assembly.GetCallingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && InheritsFromEndpoint(x));
    }

    private static bool InheritsFromEndpoint(Type type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Endpoint<>) || genericType == typeof(Endpoint<,>)) return true;
            }

            type = type.BaseType!;
        }

        return false;
    }
}