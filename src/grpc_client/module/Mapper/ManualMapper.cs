using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace grpc_client.module.Mapper
{
    public class ManualMapper
    {

        internal static Dictionary<(Type, Type), Func<object, ManualMapper, object>> mappers = new();


        public Tout? Map<Tout>(object? obj)
        {
            if (obj is null)
                return default; // to pass null through as in `a?.field`

            var key = (obj.GetType(), typeof(Tout));

            if (!mappers.ContainsKey(key))
                throw new Exception($"ManualMapper conversion was not found ({key.Item1.FullName} -> {key.Item2.FullName})");

            return (Tout)mappers[key](obj, this);
        }

        public IEnumerable<Tout> MapEnumerable<Tin, Tout>(IEnumerable<Tin> list)
        {
            var key = (typeof(Tin), typeof(Tout));

            if (!mappers.ContainsKey(key))
                throw new Exception($"ManualMapper conversion was not found ({key.Item1.FullName} -> {key.Item2.FullName})");

            foreach (var item in list)
                yield return (Tout)mappers[key](item, this);
        }


    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MapManually : Attribute;

    public class MapBuilder
    {
        public MapBuilder Register(Type type) // a bit of reflection magic
        {
            foreach(var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.GetCustomAttributes<MapManually>().Any())
                    continue; // will be added manually with other type(s)

                var p = method.GetParameters();
                if (p.Length != 2 || p[1].ParameterType != typeof(ManualMapper))
                    continue; // checking signature

                var key = (p[0].ParameterType, method.ReturnType);

                Type delegateType = typeof(Func<object, ManualMapper, object>);
                var func = method.CreateDelegate(delegateType) as Func<object, ManualMapper, object>;

                if (!ManualMapper.mappers.ContainsKey(key))
                {
                    ManualMapper.mappers.Add(key, func);
                    //Console.WriteLine($"ManualMapper method registered: {type.FullName}:{method.Name} as {key.Item1.FullName} -> {key.Item2.FullName}");
                }
                else
                    throw new Exception($"ManualMapper method registration: trying to add already defined conversion ({key.Item1.FullName} -> {key.Item2.FullName}) ({type.FullName}:{method.Name})");
            }

            return this;
        }

        public MapBuilder Register<Tin, Tout>(Func<Tin, ManualMapper, Tout> func)
        {
            var key = (typeof(Tin), typeof(Tout));
            if (!ManualMapper.mappers.ContainsKey(key))
            {
                ManualMapper.mappers.Add(key, Wrap(func));
                //Console.WriteLine($"ManualMapper method registered: {key.Item1.FullName} -> {key.Item2.FullName}");
            }
            else
                throw new Exception($"ManualMapper method registration: trying to add already defined conversion ({key.Item1.FullName} -> {key.Item2.FullName})");
            return this;
        }

        static Func<object, ManualMapper, object> Wrap<Tin, Tout>(Func<Tin, ManualMapper, Tout> func) =>
            // (object a, ManualMapper m) => func((Tin)a, m)!;
            func as Func<object, ManualMapper, object>;
    }
}
