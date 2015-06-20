using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Primitives;
using Microsoft.Win32;

namespace InstancingPerformance.Core
{
	public class Statistics
	{
		private FileInfo file;
		private List<Record> records;

		public Statistics(DrawMode drawMode)
		{
			file = new FileInfo($"Performance-{GetMachineGuid()}-{drawMode}.csv");
			records = new List<Record>();
		}

		public void AddRecord(TimeSpan frameTime, int bufferedVertices, int drawnVertices, int drawnTriangles, int chunks)
		{
			records.Add(new Record(frameTime, bufferedVertices, drawnVertices, drawnTriangles, chunks));
		}

		public string GetMachineGuid()
		{
			using (ManagementObject os = new ManagementObject("Win32_OperatingSystem=@"))
				return (string)os["SerialNumber"];
		}

		public void Write()
		{
			int count = 0;
			TimeSpan time = TimeSpan.Zero;
			Queue<TimeSpan> frames = new Queue<TimeSpan>(60);

			using (var fileStream = file.Create())
			using (var writer = new StreamWriter(fileStream))
			{
				writer.WriteLine("Frame;Time;Frame Time;Frames per Second;Chunk Count;Vertex Count;Visible Vertex Count;Triangle Count");
				foreach (var record in records)
				{
					count += 1;
					frames.Enqueue(record.FrameTime);
					while (frames.Count > 60)
						frames.Dequeue();
					double framesPerSecond = 0;
					if (frames.Count >= 10)
						framesPerSecond = 60.0 / frames.Sum(f => f.TotalSeconds);

					writer.WriteLine($"{count};{time.TotalSeconds};{record.FrameTime.TotalSeconds};{framesPerSecond};{record.ChunkCount};{record.BufferedVertices};{record.DrawnVertices};{record.Triangles}");

					time += record.FrameTime;
				}
			}
		}

		public class Record
		{
			public TimeSpan FrameTime { get; }
			public int BufferedVertices { get; }
			public int DrawnVertices { get; }
			public int Triangles { get; }
			public int ChunkCount { get; }

			public Record(TimeSpan frameTime, int bufferedVertices, int drawnVertices, int drawnTriangles, int chunks)
			{
				FrameTime = frameTime;
				BufferedVertices = bufferedVertices;
				DrawnVertices = drawnVertices;
				Triangles = drawnTriangles;
				ChunkCount = chunks;
			}
		}
	}
}
