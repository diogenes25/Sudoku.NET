using DE.ONNEN.Sudoku.SudokuExternal;
using DE.ONNEN.Sudoku.SudokuExternal.SolveTechniques;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DE.ONNEN.Sudoku
{
	public static class SudokuSolveTechniqueLoader
	{
		public static SolveTechniqueInfo GetSolveTechnicInfo(string filename)
		{
			ASolveTechnique solveTechnic = LoadSolveTechnic(filename, null);
			SolveTechniqueInfo info = new SolveTechniqueInfo();
			if (solveTechnic is ISolveTechnique)
			{
				info = ((ISolveTechnique)solveTechnic).Info;
			}
			else
			{
				throw new NotImplementedException(string.Format("The type {0} is not implemented in file {1}", typeof(ASolveTechnique).ToString(), filename));
			}
			return info;
		}

		public static ASolveTechnique LoadSolveTechnic(string filename, ISudokuHost host)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename", "the parameter filename cannot be null");
			}

			Assembly solvetechnic = Assembly.LoadFrom(filename);
			string typeName = typeof(ASolveTechnique).ToString();
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

			List<ASolveTechnique> result = new List<ASolveTechnique>();
			foreach (Type type in mytype)
			{
				object obj = Activator.CreateInstance(type);
				result.Add((ASolveTechnique)obj);
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