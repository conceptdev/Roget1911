using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoTouch.Foundation;

namespace Roget
{
	[Preserve(AllMembers=true)]
    public class RogetHierarchy
    {
        public RogetHierarchy()
        {
            Classes = new List<RogetClass>();
        }

        [XmlAttribute]
        public string Name { set; get; }
		
        public List<RogetClass> Classes { set; get; }
    }
}