using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business.Other
{
	public class Common
	{
		/// <summary>
		/// Lưu nội dung vào file nằm trong một thư mục có tên của file đó
		/// </summary>
		/// <param name="path">Đường dẫn thư mục</param>
		/// <param name="filename">Tên file</param>
		/// <param name="content">Nội dung</param>
		public static void SaveToFile(string path, string filename, string content)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			File.WriteAllText(path + "\\" + filename, content);
		}

		/// <summary>
		/// Lưu nội dung vào file cùng vị trí với chương trình
		/// </summary>
		public static void SaveToFile(string filename, string content)
		{
			File.WriteAllText(filename, content);
		}

		/// <summary>
		/// Đọc nội dung từ file, nếu file không tồn tại, trả về chuỗi trống
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ReadFile(string path)
		{
			if (File.Exists(path))
			{
				using (StreamReader reader = new StreamReader(path))
				{
					return reader.ReadToEnd();
				}
			}
			else
				return string.Empty;

		}
	}
}
