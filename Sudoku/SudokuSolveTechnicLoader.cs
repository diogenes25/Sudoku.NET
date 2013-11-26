using System;
using System.Collections.Generic;
using System.Reflection;
using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechnics;

namespace de.onnen.Sudoku
{
	public static class SudokuSolveTechnicLoader
	{
		public static SolveTechnicInfo GetSolveTechnicInfo(string filename)
		{
			ASolveTechnic solveTechnic = LoadSolveTechnic(filename, null);
			SolveTechnicInfo info = new SolveTechnicInfo();
			if (solveTechnic is ISolveTechnic)
			{
				info = ((ISolveTechnic)solveTechnic).Info;
			}
			else
			{
				throw new NotImplementedException(string.Format("The type {0} is not implemented in file {1}", typeof(ASolveTechnic).ToString(), filename));
			}
			return info;
		}

		public static ASolveTechnic LoadSolveTechnic(string filename, ISudokuHost host)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename", "the parameter filename cannot be null");
			}

			Assembly solvetechnic = Assembly.LoadFrom(filename);
			string typeName = typeof(ASolveTechnic).ToString();
			Type[] types = solvetechnic.GetTypes();
			List<Type> mytype = new List<Type>();
			bool typeFound = false;
			foreach (Type t in types)
			{
				if (t.BaseType != null && t.BaseType.FullName == typeName)
				{
					mytype.Add(t);
					typeFound = true;
				}
			}

			if (!typeFound)
			{
				throw new NotImplementedException(string.Format("The type {0} is not implemented in file{1}", typeName, filename));
			}

			List<ASolveTechnic> result = new List<ASolveTechnic>();
			foreach (Type type in mytype)
			{
				object obj = Activator.CreateInstance(type);
				result.Add((ASolveTechnic)obj);
			}
			if (result != null && result.Count > 0)
			{
				if (host != null)
				{
					result[0].Host = host;
				}
				return result[0];
			}

			return null;
		}
	}
}