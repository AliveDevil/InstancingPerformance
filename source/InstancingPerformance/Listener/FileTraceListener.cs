using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace InstancingPerformance.Listener
{
	public class FileTraceListener : TraceListener
	{
		private ActionBlock<string> actions;
		private BufferBlock<string> buffer;
		private FileStream fileStream;
		private DateTime lastFlush;
		private StreamWriter writer;

		public FileTraceListener() : this(string.Empty)
		{
		}

		public FileTraceListener(string name) : base(name)
		{
			fileStream = new FileStream("Performance.log", FileMode.Append, FileAccess.Write, FileShare.Read);
			writer = new StreamWriter(fileStream);
			buffer = new BufferBlock<string>();
			actions = new ActionBlock<string>(s => writer.Write(s));
			buffer.LinkTo(actions);
		}

		public override void Close()
		{
			buffer.Complete();
			buffer.Completion.ContinueWith(t => actions.Complete());
			actions.Completion.Wait();
			writer.Flush();
			writer.Dispose();
		}

		public override void Write(string message)
		{
			buffer.SendAsync(message);
		}

		public override void WriteLine(string message)
		{
			buffer.SendAsync($"{message}\n");
		}
	}
}
