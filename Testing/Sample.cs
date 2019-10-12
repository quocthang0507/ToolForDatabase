using System;

namespace DataAccess
{
	public class NhanVien
	{
		public String MANV { get; set; }
		public String HOTEN { get; set; }
		public String GIOITINH { get; set; }
		public DateTime NGAYSINH { get; set; }
		public Int32 LUONG { get; set; }
		public String MAPHONG { get; set; }
		public String SDT { get; set; }
		public DateTime NGAYBC { get; set; }

		public NhanVien()
		{
		}

		public NhanVien(String MANV)
		{
			this.MANV = MANV;
		}

		public NhanVien(String MANV, String HOTEN)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH, DateTime NGAYSINH)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
			this.NGAYSINH = NGAYSINH;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH, DateTime NGAYSINH, Int32 LUONG)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
			this.NGAYSINH = NGAYSINH;
			this.LUONG = LUONG;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH, DateTime NGAYSINH, Int32 LUONG, String MAPHONG)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
			this.NGAYSINH = NGAYSINH;
			this.LUONG = LUONG;
			this.MAPHONG = MAPHONG;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH, DateTime NGAYSINH, Int32 LUONG, String MAPHONG, String SDT)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
			this.NGAYSINH = NGAYSINH;
			this.LUONG = LUONG;
			this.MAPHONG = MAPHONG;
			this.SDT = SDT;
		}

		public NhanVien(String MANV, String HOTEN, String GIOITINH, DateTime NGAYSINH, Int32 LUONG, String MAPHONG, String SDT, DateTime NGAYBC)
		{
			this.MANV = MANV;
			this.HOTEN = HOTEN;
			this.GIOITINH = GIOITINH;
			this.NGAYSINH = NGAYSINH;
			this.LUONG = LUONG;
			this.MAPHONG = MAPHONG;
			this.SDT = SDT;
			this.NGAYBC = NGAYBC;
		}

		public override string ToString()
		{
			return MANV.ToString();
		}
	}
}
