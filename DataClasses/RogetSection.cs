using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoTouch.Foundation;

namespace Roget
{
	[Preserve(AllMembers=true)]
	public class RogetSection
	{
		public RogetSection () {
			Sections = new List<RogetSection>();
		}
		[XmlAttribute]
		public string Name {get;set;}
		[XmlAttribute]
		public string StartCategory {get;set;}
		[XmlAttribute]
		public string EndCategory{get;set;}
		
		public List<RogetSection> Sections{get;set;}
		
	}
}
