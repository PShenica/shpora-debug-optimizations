﻿using System.Numerics;

namespace DotTraceExamples
{
	public static class ComplexArrayExtensions
	{
		public static Complex[] DivideByNumber(this Complex[] data, double divisor)
		{
			var result = new Complex[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				result[i] = data[i] / divisor;
			}

			return result;
		}

		public static double SumModules(this Complex[] data)
		{
			var sum = 0.0;
			foreach (var t in data)
			{
				sum += t.Magnitude;
			}

			return sum;
		}
	}
}