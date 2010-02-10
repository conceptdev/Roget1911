using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoTouch.Foundation;

namespace Roget
{
	[Preserve(AllMembers=true)]
	public class RogetCategory
	{
		public RogetCategory ()
		{
			PartsOfSpeech = new List<RogetPartOfSpeech>();
		}
	
		[XmlAttribute]
		public string Name {get;set;}
		[XmlAttribute]
		public string Number {get;set;}
		[XmlAttribute]
		public string Description { set; get; }
		
		public List<RogetPartOfSpeech> PartsOfSpeech {get;set;}
		
		/// <summary>
		/// Used for numeric comparisons
		/// </summary>
		public int Index 
		{
			get
			{
				Console.WriteLine("Get index from {0} for {1}", Number, Name);
				return Number.ToNumber();
			}
		}
	}
}
