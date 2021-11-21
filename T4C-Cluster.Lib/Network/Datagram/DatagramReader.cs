using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Container;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram
{
    public static class DatagramReader
    {
        private static IDictionary<Type, Action<object, DatagramBody>> _readers = new Dictionary<Type, Action<object, DatagramBody>>();
        private static IDictionary<DatagramTypeEnum, Func<object>> _datagrameTypeToObject = new Dictionary<DatagramTypeEnum, Func<object>>();


        static DatagramReader()
        {
            init();
        }

        static private void init()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes<DatagramTypeAttribute>().Any());
            foreach (var type in types)
            {
                if (_readers.ContainsKey(type))
                    continue;
                var attributes = type.GetCustomAttributes<DatagramTypeAttribute>();

                if (attributes == null || attributes.Any() == false || attributes.All(x => x.Direction == DatagramDirectionEnum.Out))
                    continue;

                PrepareType(type);
                foreach (var attr in attributes)
                    _datagrameTypeToObject.Add(attr.DatagramType, () => (object)Activator.CreateInstance(type));
            }
        }

        public static IRequest Read(DatagramBody datagram)
        {
            datagram.SetCursorPosition(4);
            Func<object> reqActivator = null;
            if (!_datagrameTypeToObject.TryGetValue((DatagramTypeEnum)datagram.ReadUInt16().GetValueOrDefault(), out reqActivator))
                return null;

            IRequest message = (IRequest)reqActivator.Invoke();
            _readers[message.GetType()](message, datagram);
            return message;
        }
        private static void PrepareType(Type type)
        {

            var properties = type.GetProperties();
            var attr = type.GetCustomAttribute<DatagramTypeAttribute>();

            var actions = new Dictionary<int, Action<object, DatagramBody>>();
            foreach (var property in properties)
            {
                PrepareBool(property, attr, property.GetCustomAttribute<DataTypeBoolAttribute>(), actions);
                PrepareInt8(property, attr, property.GetCustomAttribute<DataTypeInt8Attribute>(), actions);
                PrepareInt16(property, attr, property.GetCustomAttribute<DataTypeInt16Attribute>(), actions);
                PrepareInt32(property, attr, property.GetCustomAttribute<DataTypeInt32Attribute>(), actions);
                PrepareInt64(property, attr, property.GetCustomAttribute<DataTypeInt64Attribute>(), actions);
                PrepareUInt16(property, attr, property.GetCustomAttribute<DataTypeUInt16Attribute>(), actions);
                PrepareUInt32(property, attr, property.GetCustomAttribute<DataTypeUInt32Attribute>(), actions);
                PrepareUInt64(property, attr, property.GetCustomAttribute<DataTypeUInt64Attribute>(), actions);
                PrepareString8(property, attr, property.GetCustomAttribute<DataTypeString8Attribute>(), actions);
                PrepareString16(property, attr, property.GetCustomAttribute<DataTypeString16Attribute>(), actions);
                PrepareArray8(property, attr, property.GetCustomAttribute<DataTypeArray8Attribute>(), actions);
                PrepareArray16(property, attr, property.GetCustomAttribute<DataTypeArray16Attribute>(), actions);

            }
            var orderedAction = actions.OrderBy(x => x.Key).Select(x => x.Value);
            _readers.Add(type, new Action<object, DatagramBody>((obj, datagram) =>
            {
                foreach (var act in orderedAction)
                    act.Invoke(obj, datagram);
            }));
        }

        private static void PrepareBool(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeBoolAttribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(bool))
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, datagram.ReadBool());
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareInt8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt8Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(byte) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");





            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(Enum.ToObject(propType, datagram.ReadByte()), propType) : datagram.ReadByte()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareInt16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt16Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(Int16) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt16(), propType) : (Int16)datagram.ReadUInt16()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareInt32(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt32Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(Int32) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt32(), propType) : (Int32)datagram.ReadUInt32()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareInt64(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt64Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(Int64) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt64(), propType) : (Int64)datagram.ReadUInt64()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }

        private static void PrepareUInt16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt16Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(UInt16) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt16(), propType) : (UInt16)datagram.ReadUInt16()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareUInt32(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt32Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(UInt32) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt32(), propType) : (UInt32)datagram.ReadUInt32()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareUInt64(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt64Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(UInt64) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, (propType.IsEnum ? Convert.ChangeType(datagram.ReadUInt64(), propType) : (UInt64)datagram.ReadUInt64()));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareString8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeString8Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(string))
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, datagram.ReadString8());
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareString16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeString16Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(string))
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    property.SetValue(obj, datagram.ReadString16());
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareArray8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeArray8Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = property.PropertyType;

            if (!propType.IsArray)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            var eleType = propType.GetElementType();
            var canConsruct = eleType.GetConstructors().Any(con => con.GetParameters().Count() == 0);


            if (!canConsruct)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");


            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    var nb = datagram.ReadByte();
                    var array = Array.CreateInstance(eleType, nb.Value);
                    for (int i = 0; i < nb; i++)
                    {

                        array.SetValue(Activator.CreateInstance(eleType), i);
                    }


                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
        private static void PrepareArray16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeArray16Attribute dataTypeAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (dataTypeAttr == null)
                return;

            Type propType = property.PropertyType;

            if (!propType.IsArray)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            var eleType = propType.GetElementType();
            var canConsruct = eleType.GetConstructors().Any(con => con.GetParameters().Count() == 0);


            if (!canConsruct)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");


            actions.Add(dataTypeAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                try
                {
                    var nb = datagram.ReadUInt16();
                    var array = Array.CreateInstance(eleType, nb.Value);
                    for (int i = 0; i < nb; i++)
                    {
                        array.SetValue(Activator.CreateInstance(eleType), i);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(datagramTypeAttr.DatagramType + " : " + property.Name, ex);
                }
            }));
        }
    }
}
