//
// Copyright © 2006 - 2011 Nauck IT KG		http://www.nauck-it.de
//
// Author:
//	Daniel Nauck		<d.nauck(at)nauck-it.de>
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace OpenLawOffice.Data.PostgreSQLProvider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;

    internal static class SerializationHelper
    {
        internal static string SerializeToBase64(object value)
        {
            return Convert.ToBase64String(SerializeToBinary(value));
        }

        internal static T DeserializeFromBase64<T>(string value)
        {
            return DeserializeFromBinary<T>(Convert.FromBase64String(value));
        }

        internal static string SerializeToXml<T>(T value, string nameSpace)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(T), nameSpace);
                UTF8Encoding utf8Encoding = new UTF8Encoding();
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, utf8Encoding);
                xmlWriter.Formatting = Formatting.Indented;
                xmlFormatter.Serialize(xmlWriter, value);
                return utf8Encoding.GetString(stream.ToArray());
            }
        }

        internal static void SerializeToXmlFile<T>(string fileName, T value, string nameSpace)
        {
            using (FileStream stream = File.Create(fileName))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T), nameSpace);
                formatter.Serialize(stream, value);

                stream.Close();
            }
        }

        internal static T DeserializeFromXml<T>(string value, string nameSpace)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();

            using (MemoryStream stream = new MemoryStream(utf8Encoding.GetBytes(value)))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(T), nameSpace);
                return (T)xmlFormatter.Deserialize(stream);
            }
        }

        internal static T DeserializeFromXmlFile<T>(string fileName, string nameSpace)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T), nameSpace);
                T result = (T)formatter.Deserialize(stream);

                stream.Close();
                return result;
            }
        }

        internal static byte[] SerializeToBinary(object value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(stream, value);

                return stream.ToArray();
            }
        }

        internal static void SerializeToBinaryFile(string fileName, object value)
        {
            using (FileStream stream = File.Create(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);

                stream.Close();
            }
        }

        internal static T DeserializeFromBinary<T>(byte[] value)
        {
            using (MemoryStream stream = new MemoryStream(value))
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                return (T)binFormatter.Deserialize(stream);
            }
        }

        internal static T DeserializeFromBinaryFile<T>(string fileName)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                T result = (T)formatter.Deserialize(stream);

                stream.Close();
                return result;
            }
        }
    }
}
