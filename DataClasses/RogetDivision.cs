using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoTouch.Foundation;

namespace Roget
{
	/// <summary>
	/// I'm not really sure what a 'Division' is, linguistically
	/// </summary>
	[Preserve(AllMembers=true)]
	public class RogetDivision
    {
        public RogetDivision()
        {
            Sections = new List<RogetSection>();
        }

        [XmlAttribute]
        public string Name { set; get; }
		
        public List<RogetSection> Sections { get; set; }

    }
}
