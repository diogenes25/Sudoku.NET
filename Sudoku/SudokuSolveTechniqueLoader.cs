using DE.Onnen.Sudoku.SolveTechniques;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace DE.Onnen.Sudoku
{
	public static class SudokuSolveTechniqueLoader
	{
		public static SolveTechniqueInfo GetSolveTechnicInfo(string fileName)
		{
			ASolveTechnique solveTechnic = LoadSolveTechnic(fileName, null);
			SolveTechniqueInfo info = new SolveTechniqueInfo();
			if (solveTechnic is ISolveTechnique)
			{
				info = ((ISolveTechnique)solveTechnic).Info;
			}
			else
			{
				throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "The type {0} is not implemented in file {1}", typeof(ASolveTechnique).ToString(), fileName));
			}
			return info;
		}

		public static ASolveTechnique LoadSolveTechnic(string fileName, ISudokuHost host)
		{
			if (String.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName", "the parameter filename cannot be null");
			}

			Assembly solvetechnic = Assembly.LoadFrom(fileName);
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
				throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "The type {0} is not implemented in file{1}", typeName, fileName));
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