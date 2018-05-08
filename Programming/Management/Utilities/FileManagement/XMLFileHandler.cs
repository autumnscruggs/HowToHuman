using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class XMLFileHandler
{
    public static string ObjectToXml<T>(T objectToSerialise)
    {
        StringWriter Output = new StringWriter(new StringBuilder());
        XmlSerializer xs = new XmlSerializer(objectToSerialise.GetType());
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        xs.Serialize(Output, objectToSerialise, ns);
        return Output.ToString();
    }
}
