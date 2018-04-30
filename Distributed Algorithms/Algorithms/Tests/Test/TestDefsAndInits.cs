using DistributedAlgorithms.Algorithms.Base.Base;
using System.Collections.Generic;


namespace DistributedAlgorithms.Algorithms.Tests.Test
{

	#region /// \name Enums for TestProcess
	public class m
	{
 
		public enum message1
		{
			PrevAttr, S1
		}
 
		public enum MessageTypes
		{
			Message1
		}
	}
	#endregion

	#region /// \name partial class for TestProcess
	public partial class TestProcess: BaseProcess
	{
 
		public AttributeDictionary MessageDataFor_Message1( bm.PrmSource prmSource = bm.PrmSource.Default,
			AttributeDictionary dictionary = null,
			System.Boolean prevAttr = false,
			System.String s1 = "")
		{
 
			if ( prmSource == bm.PrmSource.MainPrm)
			{
				return dictionary;
			}
			dictionary = new AttributeDictionary();
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.m+ork_message1";
 
			if ( prmSource == bm.PrmSource.Default)
			{
				dictionary.Add( m.message1.PrevAttr, new Attribute { Value = false ,Changed = false } );
				dictionary.Add( m.message1.S1, new Attribute { Value = "" ,Changed = false } );
				return dictionary;
			} 

			dictionary.Add( m.message1.PrevAttr, new Attribute { Value = prevAttr ,Changed = false } );
			dictionary.Add( m.message1.S1, new Attribute { Value = s1 ,Changed = false } );
			return dictionary;
		}
 
		public void SendMessage1(AttributeDictionary  fields = null, 
			SelectingMethod selectingMethod = SelectingMethod.All,
			List<int> ids = null)
		{
			if(fields is null)
			{
				MessageDataFor_Message1();
			}
			Send(m.MessageTypes.Message1, fields, selectingMethod, ids, 0, 0);
		}
	}
	#endregion

	#region /// \name Enums for TestNetwork
	public class n
	{
 
		public enum pak
		{
			Version
		}
 
		public enum ork
		{
	
		}
	}
	#endregion

	#region /// \name partial class for TestNetwork
	public partial class TestNetwork: BaseNetwork
	{
		const string prevAttr = "PrevAttr";
		const string s1 = "S1";
		const m.MessageTypes Message1 = m.MessageTypes.Message1;
		const string type = "Type";
		const string edited = "Edited";
		const string id = "Id";
		const string algorithm = "Algorithm";
		const string centrilized = "Centrilized";
		const string directedNetwork = "DirectedNetwork";
		const string version = "Version";
		const string breakpoints = "Breakpoints";
		const string singleStepStatus = "SingleStepStatus";
		const string name = "Name";
		const string initiator = "Initiator";
		const string round = "Round";
		const string terminationStatus = "TerminationStatus";
		const string messageQueue = "MessageQ";
		const string receivePort = "ReceivePort";
		const string baseAlgorithmData = "BaseAlgorithmData";
		const string internalEvents = "InternalEvents";
		const string frameColor = "FrameColor";
		const string frameWidth = "FrameWidth";
		const string frameHeight = "FrameHeight";
		const string frameLeft = "FrameLeft";
		const string frameTop = "FrameTop";
		const string frameLineWidth = "FrameLineWidth";
		const string background = "Background";
		const string foreground = "Foreground";
		const string text = "Text";
		const string breakpointsFrameColor = "BreakpointsFrameColor";
		const string breakpointsFrameWidth = "BreakpointsFrameWidth";
		const string breakpointsBackground = "BreakpointsBackground";
		const string breakpointsForeground = "BreakpointsForeground";
		const string sourceProcess = "SourceProcess";
		const string destProcess = "DestProcess";
		const string cs2 = "Cs2";
		const string cs1 = "Cs1";
		const string cs3 = "Cs3";
		const string sourcePort = "SourcePort";
		const string destPort = "DestPort";
		const string lineColor = "LineColor";
		const string headColor = "HeadColor";
		const string presentationTxt = "PresentationTxt";
		const string messagesFrameColor = "MessagesFrameColor";
		const string messagesFrameWidth = "MessagesFrameWidth";
		const string messagesBackground = "MessagesBackground";
		const string messagesForeground = "MessagesForeground";
 
		public System.Int32 Version
		{
			 get { return pa[n.pak.Version]; }
			 set { pa[n.pak.Version] = value; }
		}



		public override int GetVersion()
		{
			try
			{
				return pa[n.pak.Version];
			}
			catch
			{
				return 0;
			}
		}
 
		protected override void InitPrivateAttributes()
		{
			AttributeDictionary dictionary = pa;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.n+pak";
			dictionary.Add(n.pak.Version, new Attribute { Value = 5 ,Editable = false ,Changed = false } );
			base.InitPrivateAttributes();
		}
 
		protected override void InitOperationResults()
		{
			AttributeDictionary dictionary = or;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.n+ork";
			base.InitOperationResults();
		}
	}
	#endregion

	#region /// \name Enums for TestProcess
	public class p
	{
 
		public enum pak
		{
	
		}
 
		public enum ork
		{
			S1
		}
	}
	#endregion

	#region /// \name partial class for TestProcess
	public partial class TestProcess: BaseProcess
	{
		const string prevAttr = "PrevAttr";
		const string s1 = "S1";
		const m.MessageTypes Message1 = m.MessageTypes.Message1;
		const string type = "Type";
		const string edited = "Edited";
		const string id = "Id";
		const string algorithm = "Algorithm";
		const string centrilized = "Centrilized";
		const string directedNetwork = "DirectedNetwork";
		const string version = "Version";
		const string breakpoints = "Breakpoints";
		const string singleStepStatus = "SingleStepStatus";
		const string name = "Name";
		const string initiator = "Initiator";
		const string round = "Round";
		const string terminationStatus = "TerminationStatus";
		const string messageQueue = "MessageQ";
		const string receivePort = "ReceivePort";
		const string baseAlgorithmData = "BaseAlgorithmData";
		const string internalEvents = "InternalEvents";
		const string frameColor = "FrameColor";
		const string frameWidth = "FrameWidth";
		const string frameHeight = "FrameHeight";
		const string frameLeft = "FrameLeft";
		const string frameTop = "FrameTop";
		const string frameLineWidth = "FrameLineWidth";
		const string background = "Background";
		const string foreground = "Foreground";
		const string text = "Text";
		const string breakpointsFrameColor = "BreakpointsFrameColor";
		const string breakpointsFrameWidth = "BreakpointsFrameWidth";
		const string breakpointsBackground = "BreakpointsBackground";
		const string breakpointsForeground = "BreakpointsForeground";
		const string sourceProcess = "SourceProcess";
		const string destProcess = "DestProcess";
		const string cs2 = "Cs2";
		const string cs1 = "Cs1";
		const string cs3 = "Cs3";
		const string sourcePort = "SourcePort";
		const string destPort = "DestPort";
		const string lineColor = "LineColor";
		const string headColor = "HeadColor";
		const string presentationTxt = "PresentationTxt";
		const string messagesFrameColor = "MessagesFrameColor";
		const string messagesFrameWidth = "MessagesFrameWidth";
		const string messagesBackground = "MessagesBackground";
		const string messagesForeground = "MessagesForeground";
 
		public System.String S1
		{
			 get { return or[p.ork.S1]; }
			 set { or[p.ork.S1] = value; }
		}
 
		protected override void InitPrivateAttributes()
		{
			AttributeDictionary dictionary = pa;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.p+pak";
			base.InitPrivateAttributes();
		}
 
		protected override void InitOperationResults()
		{
			AttributeDictionary dictionary = or;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.p+ork";
			dictionary.Add(p.ork.S1, new Attribute { Value = "" ,Changed = false } );
			base.InitOperationResults();
		}
	}
	#endregion

	#region /// \name Enums for TestChannel
	public class c
	{
 
		public enum pak
		{
			Cs2, Cs1
		}
 
		public enum ork
		{
			Cs3, Type 
		}
	}
	#endregion

	#region /// \name partial class for TestChannel
	public partial class TestChannel: BaseChannel
	{
		const string prevAttr = "PrevAttr";
		const string s1 = "S1";
		const m.MessageTypes Message1 = m.MessageTypes.Message1;
		const string type = "Type";
		const string edited = "Edited";
		const string id = "Id";
		const string algorithm = "Algorithm";
		const string centrilized = "Centrilized";
		const string directedNetwork = "DirectedNetwork";
		const string version = "Version";
		const string breakpoints = "Breakpoints";
		const string singleStepStatus = "SingleStepStatus";
		const string name = "Name";
		const string initiator = "Initiator";
		const string round = "Round";
		const string terminationStatus = "TerminationStatus";
		const string messageQueue = "MessageQ";
		const string receivePort = "ReceivePort";
		const string baseAlgorithmData = "BaseAlgorithmData";
		const string internalEvents = "InternalEvents";
		const string frameColor = "FrameColor";
		const string frameWidth = "FrameWidth";
		const string frameHeight = "FrameHeight";
		const string frameLeft = "FrameLeft";
		const string frameTop = "FrameTop";
		const string frameLineWidth = "FrameLineWidth";
		const string background = "Background";
		const string foreground = "Foreground";
		const string text = "Text";
		const string breakpointsFrameColor = "BreakpointsFrameColor";
		const string breakpointsFrameWidth = "BreakpointsFrameWidth";
		const string breakpointsBackground = "BreakpointsBackground";
		const string breakpointsForeground = "BreakpointsForeground";
		const string sourceProcess = "SourceProcess";
		const string destProcess = "DestProcess";
		const string cs2 = "Cs2";
		const string cs1 = "Cs1";
		const string cs3 = "Cs3";
		const string sourcePort = "SourcePort";
		const string destPort = "DestPort";
		const string lineColor = "LineColor";
		const string headColor = "HeadColor";
		const string presentationTxt = "PresentationTxt";
		const string messagesFrameColor = "MessagesFrameColor";
		const string messagesFrameWidth = "MessagesFrameWidth";
		const string messagesBackground = "MessagesBackground";
		const string messagesForeground = "MessagesForeground";
 
		public System.String Cs2
		{
			 get { return pa[c.pak.Cs2]; }
			 set { pa[c.pak.Cs2] = value; }
		}
 
		public System.String Cs1
		{
			 get { return pa[c.pak.Cs1]; }
			 set { pa[c.pak.Cs1] = value; }
		}
 
		public System.String Cs3
		{
			 get { return or[c.ork.Cs3]; }
			 set { or[c.ork.Cs3] = value; }
		}
 
		protected override void InitPrivateAttributes()
		{
			AttributeDictionary dictionary = pa;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.c+pak";
			dictionary.Add(c.pak.Cs2, new Attribute { Value = "" ,Changed = false } );
			dictionary.Add(c.pak.Cs1, new Attribute { Value = "" ,Changed = false } );
			base.InitPrivateAttributes();
		}
 
		protected override void InitOperationResults()
		{
			AttributeDictionary dictionary = or;
			dictionary.selfEnumName = "DistributedAlgorithms.Algorithms.Tests.Test.c+ork";
			dictionary.Add(c.ork.Cs3, new Attribute { Value = "" ,Changed = false } );
			base.InitOperationResults();
		}
	}
	#endregion
}