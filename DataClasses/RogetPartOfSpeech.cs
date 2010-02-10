using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoTouch.Foundation;

namespace Roget
{
	[Preserve(AllMembers=true)]
	public class RogetPartOfSpeech
	{
		public RogetPartOfSpeech ()
		{
			Lines = new List<string>();
		}

		[XmlAttribute]
		public PartOfSpeech PartOfSpeech {get;set;}
	
		public List<string> Lines {get;set;}
	}
}
