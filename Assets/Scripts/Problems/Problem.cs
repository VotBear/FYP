using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ProblemList")]
public class ProblemList {
	
	[XmlElement("ProblemSet")]
	public List<ProblemSet> problemSetList;
	 
	public static ProblemList LoadProblemList(StringReader xml){ 
		var serializer = new XmlSerializer(typeof(ProblemList));

		return serializer.Deserialize(xml) as ProblemList;
	}
}
 
public class ProblemSet { 
	[XmlAttribute("ID")]
	public string ID;

	public string Title;

	[XmlElement("Problem")]
	public List<Problem> problemList;
	
}

public class Problem {

	[XmlAttribute("ID")]
	public string ID;

	public string Title;
	public string Content;
	public string Objective;
	public string Requirements;
	public int AnswerType;
	public int Answer;
	public int Answer2;
} 