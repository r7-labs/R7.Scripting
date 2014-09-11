using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace R7.Scripting
{
	/*
	NAUTILUS_SCRIPT_SELECTED_FILE_PATHS: список выделенных файлов, разделённых переводом строки (только в локальном случае)
	NAUTILUS_SCRIPT_SELECTED_URIS: список адресов (URI) выделенных файлов, разделённых переводом строки
	NAUTILUS_SCRIPT_CURRENT_URI: текущий адрес URI
	NAUTILUS_SCRIPT_WINDOW_GEOMETRY: положение и размер текущего окна 
	NAUTILUS_SCRIPT_NEXT_PANE_SELECTED_FILE_PATHS: список выделенных файлов, разделённых переводом строки, в неактивной панели окна раздельного вида (только в локальном случае)	
	NAUTILUS_SCRIPT_NEXT_PANE_SELECTED_URIS: список адресов (URI) выделенных файлов, разделённых переводом строки, в неактивной панели окна раздельного вида
	NAUTILUS_SCRIPT_NEXT_PANE_CURRENT_URI: текущий адрес URI в неактивной панели окна раздельного вида
	*/

	// THINK: Support Marlin?
	public enum FileManager { Unknown, Nautilus, Nemo, Marlin }

	// TODO: Rename to FileManager?
	// TODO: Unmake static?


	/// <summary>
	/// Helper for Nautilus 
	/// </summary>
	public class NauHelper
	{
		private static Version version = null;
		public static Version Version 
		{
			get 
			{
				if (version == null)
				{
					version = new Version (
						Regex.Match (Command.RunToString (FileManager.ToString().ToLowerInvariant(), "--version"), @"\d\.\d\.\d").Value
					);
				}
				return version;
			}
		}

		protected static FileManager fileManager = FileManager.Unknown;
		public static FileManager FileManager
		{
			get 
			{ 
				if (fileManager == FileManager.Unknown)
				{
					if (Environment.GetEnvironmentVariable ("NAUTILUS_SCRIPT_CURRENT_URI") != null)
					{
						fileManager = FileManager.Nautilus;
					}
					else if (Environment.GetEnvironmentVariable("NEMO_SCRIPT_CURRENT_URI") != null)
					    fileManager = FileManager.Nemo;
				}
				return fileManager;
			}
		}

		protected static string Env (string suffix)
		{
			return FileManager.ToString().ToUpperInvariant() + "_" + suffix;
		}

		protected static string EnvValue (string suffix)
		{
			return Environment.GetEnvironmentVariable(Env(suffix));
		}

		public static string ScriptDirectory
		{
			get 
			{
				if (FileManager == FileManager.Nautilus)
				if (Version.Major >= 3 && Version.Minor >= 6)
				{
					// in Nautilus 3.6 location of scripts directory changed
					return Path.Combine (Environment.GetEnvironmentVariable ("HOME"), ".local/share/nautilus/scripts");
				}
				else
				{
					return Path.Combine (Environment.GetEnvironmentVariable ("HOME"), ".gnome2/nautilus-scripts");
				}
				else if (FileManager == FileManager.Nemo)
				{
					return Path.Combine (Environment.GetEnvironmentVariable ("HOME"), ".gnome2/nemo-scripts");
				}
				else
					return string.Empty;
			}
		}

		[Obsolete("Use NauHelper.FileManager property")]
		public static bool FromNau {
			get { return !string.IsNullOrWhiteSpace(EnvValue ("SCRIPT_CURRENT_URI")); }
			
		}

		public static string CurrentDirectory
		{
			get
			{
				if (!string.IsNullOrWhiteSpace (EnvValue ("SCRIPT_CURRENT_URI")))
					return UrlDecode (EnvValue ("SCRIPT_CURRENT_URI").Remove (0, "file://".Length));
				else
					return Directory.GetCurrentDirectory ();
			}
		}
		
		public static bool IsSomethingSelected
		{
			get { return !string.IsNullOrWhiteSpace (EnvValue("SCRIPT_SELECTED_URIS")); }
			
		}
		
		public static bool IsNothingSelected
		{
			get { return !IsSomethingSelected; }
		}
		
		protected static string[] selectedFiles = new string[0];
		public static string[] SelectedFiles
		{
			get
			{
				if (selectedFiles.Length == 0)
				{
					string filesVar = EnvValue ("SCRIPT_SELECTED_URIS");
					selectedFiles = filesVar.Split (new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

					// TODO: Support not only local files
					for (var i = 0; i < selectedFiles.Length; i++)
						selectedFiles [i] = UrlDecode (selectedFiles [i].Remove (0, "file://".Length));
				}
				return selectedFiles;
			}
		}
				
		public static string FixUrlEncoding (string url)
		{
			url = url.Replace ("+", "%2B");
			url = url.Replace ("$", "%24");
			url = url.Replace ("&", "%26");
			url = url.Replace (",", "%2C");
			//url = url.Replace ("/", "%2F");
			url = url.Replace (":", "%3A");
			url = url.Replace (";", "%3B");
			url = url.Replace ("=", "%3D");
			url = url.Replace ("?", "%3F");
			//url = url.Replace ("@", "%40");
			
			return url;
		}

			
		public static string UrlDecode (string url)
		{
			return HttpUtility.UrlDecode(FixUrlEncoding(url));
		}
		
	}
}
