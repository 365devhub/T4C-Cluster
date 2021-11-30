using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Container;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram
{
    public static class MessageWriter
    {
        private static IDictionary<Type, Action<object, DatagramBody>> _writers = new Dictionary<Type, Action<object, DatagramBody>>();

        static MessageWriter()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes<DatagramTypeAttribute>().Any());
            foreach (var type in types)
            {
                if (_writers.ContainsKey(type))
                    continue;
                var attribute = type.GetCustomAttribute<DatagramTypeAttribute>();
                if (attribute == null || attribute.Direction == DatagramDirectionEnum.In)
                    continue;

                PrepareType(type);
            }
        }

        public static DatagramBody ToDatagramBody(object message)
        {
            var datagramBody = new DatagramBody();

            var attr = message.GetType().GetCustomAttribute<DatagramTypeAttribute>(true);

            datagramBody.WriteByte(0);
            datagramBody.WriteByte(0);
            datagramBody.WriteByte(0);
            datagramBody.WriteByte(0);
            datagramBody.WriteUInt16((ushort)attr.DatagramType);

            _writers[message.GetType()].Invoke(message, datagramBody);
            return datagramBody;
        }

        public static DatagramBody ToInternalDatagramBody(object message)
        {
            var datagramBody = new DatagramBody();
            _writers[message.GetType()].Invoke(message, datagramBody);
            return datagramBody;
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
            _writers.Add(type, new Action<object, DatagramBody>((obj, datagram) =>
            {
                foreach (var act in orderedAction)
                    act.Invoke(obj, datagram);
            }));
        }


        private static void PrepareBool(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeBoolAttribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(bool) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                datagram.WriteBool((bool?)property.GetValue(obj));
            }));
        }
        private static void PrepareInt8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt8Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(byte) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteByte((byte)property.GetValue(obj));
                else
                    datagram.WriteByte((byte?)property.GetValue(obj));
            }));
        }
        private static void PrepareInt16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt16Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(short) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteInt16((short)property.GetValue(obj));
                else
                    datagram.WriteInt16((short?)property.GetValue(obj));
            }));
        }
        private static void PrepareInt32(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt32Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(int) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteInt32((int)property.GetValue(obj));
                else
                    datagram.WriteInt32((int?)property.GetValue(obj));
            }));
        }
        private static void PrepareInt64(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeInt64Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(long) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteInt64((long)property.GetValue(obj));
                else
                    datagram.WriteInt64((long?)property.GetValue(obj));
            }));
        }

        private static void PrepareUInt16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt16Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(ushort) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteUInt16((ushort)property.GetValue(obj));
                else
                    datagram.WriteUInt16((ushort?)property.GetValue(obj));
            }));
        }
        private static void PrepareUInt32(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt32Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(uint) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteUInt32((uint)property.GetValue(obj));
                else
                    datagram.WriteUInt32((uint?)property.GetValue(obj));
            }));
        }
        private static void PrepareUInt64(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeUInt64Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(ulong) && !propType.IsEnum)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                if (propType.IsEnum)
                    datagram.WriteUInt64((ulong)property.GetValue(obj));
                else
                    datagram.WriteUInt64((ulong?)property.GetValue(obj));
            }));
        }

        private static void PrepareString8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeString8Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(string))
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                datagram.WriteString8((string)property.GetValue(obj));
            }));
        }
        private static void PrepareString16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeString16Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (propType != typeof(string))
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                datagram.WriteString16((string)property.GetValue(obj));
            }));
        }
        private static void PrepareArray8(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeArray8Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = property.PropertyType;

            if (!propType.IsArray)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            var eleType = propType.GetElementType();

            PrepareType(eleType);

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                var array = property.GetValue(obj) as Array;
                var length = (byte)(array == null ? 0 : array.Length);
                datagram.WriteByte(length);

                for (var i = 0; i < length; i++)
                {
                    var retdatagram = (DatagramBody)ToInternalDatagramBody(array.GetValue(i));
                    datagram.PushBuffer(retdatagram.GetBufferCopy());
                }
            }));
        }
        private static void PrepareArray16(PropertyInfo property, DatagramTypeAttribute datagramTypeAttr, DataTypeArray16Attribute typaAttr, Dictionary<int, Action<object, DatagramBody>> actions)
        {
            if (typaAttr == null)
                return;

            Type propType = property.PropertyType;

            if (!propType.IsArray)
                throw new InvalidCastException($"{datagramTypeAttr.DatagramType}.{property.Name}");

            var eleType = propType.GetElementType();
            PrepareType(eleType);

            actions.Add(typaAttr.Index, new Action<object, DatagramBody>((obj, datagram) =>
            {
                var array = property.GetValue(obj) as Array;
                var length = (ushort)(array == null ? 0 : array.Length);
                datagram.WriteUInt16(length);

                for (var i = 0; i < length; i++)
                {
                    var retdatagram = (DatagramBody)ToInternalDatagramBody(array.GetValue(i));
                    datagram.PushBuffer(retdatagram.GetBufferCopy());
                }
            }));
        }
    }
}
