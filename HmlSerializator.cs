using System;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lesson11C
{
    [XmlRoot("Data.Root")]
	public class DataRoot
    {
        [XmlArray("Data.Root.Names")]
        [XmlArrayItem("Name")]
        public List<string> Names = new List<string>();


        [XmlElement(typeof(DataEntry))]
        [XmlElement(typeof(DataExtendedEntry))]
        public List<DataEntry> DataEntryList = new List<DataEntry>();



    }

    [XmlType("Data.Entry")]
    public class DataEntry
    {
        [XmlAttribute]
        public string LinkedEntry;
        [XmlElement("Data.Name")]
        public string DataName;
    }

    [XmlType("Data#ExtendedEntry")]
    public class DataExtendedEntry : DataEntry
    {
        [XmlElement("Data#Extended")]
        public string DataExtended;
    }
}

