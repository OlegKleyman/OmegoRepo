using System;
using System.ComponentModel;
using System.Globalization;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents a converter that can convert a string to an object and vice versa.
    /// </summary>
    public class ReflectiveStringTypeConverter : TypeConverter
    {
        /// <summary>
        ///     Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="ITypeDescriptorContext" /> object that provides a format context.
        /// </param>
        /// <param name="sourceType"> A System.Type that represents the type you want to convert from. </param>
        /// <returns> true if this converter can perform the conversion; otherwise, false. </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        ///     Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="ITypeDescriptorContext" /> object that provides a format context.
        /// </param>
        /// <param name="destinationType"> A System.Type that represents the type you want to convert to. </param>
        /// <returns> true if this converter can perform the conversion; otherwise, false. </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        ///     Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context"> An System.ComponentModel.ITypeDescriptorContext that provides a format context. </param>
        /// <param name="culture"> The System.Globalization.CultureInfo to use as the current culture. </param>
        /// <param name="value"> The System.Object to convert. </param>
        /// <returns> An System.Object that represents the converted value. </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var type = Type.GetType(value.ToString(), true);
                return Activator.CreateInstance(type);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        ///     Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context"> An System.ComponentModel.ITypeDescriptorContext that provides a format context. </param>
        /// <param name="culture"> A System.Globalization.CultureInfo. If null is passed, the current culture is assumed. </param>
        /// <param name="value"> The System.Object to convert. </param>
        /// <param name="destinationType"> </param>
        /// <returns> The System.Type to convert the value parameter to. </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return value.GetType().FullName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}