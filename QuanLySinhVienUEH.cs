using System;                     // Các lớp cơ bản của .NET
using System.Collections.Generic;  // Dành cho các collections như List, Dictionary
using System.Linq;                 // Dành cho các phương thức LINQ
using System.Text;                 // Dành cho xử lý chuỗi văn bản
using System.Threading.Tasks;     // Dành cho async/await và tác vụ bất đồng bộ
using System.IO;                   // Dành cho thao tác với các tệp tin và thư mục
using System.Globalization;         // Dành cho các lớp xử lý văn hóa (ví dụ: định dạng ngày, số)
using System.Net.NetworkInformation; // Dành cho các lớp liên quan đến mạng, kiểm tra trạng thái kết nối mạng

namespace QuanLySinhVienUEH
{

    internal class Class6
    {

        static string[,] sinhViens = new string[1000, 8]; // Mảng lưu danh sách sinh viên (tối đa 100 sinh viên)
        static int sinhVienCount = 0; // Số lượng sinh viên hiện tại
        static string fileName = "SinhVien.txt"; // Đường dẫn đến file
        static void Main(string[] args)
        {
            //Hiển thị các kí tự tiếng Việt
            Console.OutputEncoding = Encoding.UTF8;
            // Hiển thị giao diện đăng nhập
            if (!DangNhap())
            {
                Console.WriteLine("\nĐăng nhập thất bại quá 3 lần. Chương trình sẽ thoát.");
                return;
            }

            // Tiếp tục hiển thị menu nếu đăng nhập thành công
            while (true)
            {
                ShowMenu();
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                Console.Write("\nChọn một tùy chọn (1-7): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ThemSinhVien();
                        break;
                    case "2":
                        TimTTSinhVien();
                        break;
                    case "3":
                        SuaSinhVien();
                        break;
                    case "4":
                        XoaSinhVien();
                        break;
                    case "5":
                        XuatDanhSachHocBong();
                        break;
                    case "6":
                        XetLoaiTotNghiep();
                        break;
                    case "7":
                        Console.WriteLine("Thoát chương trình.");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại!");
                        break;
                }

                Console.WriteLine("\nNhấn phím bất kỳ để quay lại menu...");
                Console.ReadKey();
            }
        }

        // Hàm đăng nhập
        static bool DangNhap()
        {
            const string taiKhoanDung = "uehquanlysinhvien";
            const string matKhauDung = "sinhvienueh10";
            int soLanThu = 0;
            const int maxThu = 3;

            while (soLanThu < maxThu)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                string title = @"  
   
 _       __     __                             __           __  __________  __   _____ __            __           __ 
| |     / /__  / /________  ____ ___  ___     / /_____     / / / / ____/ / / /  / ___// /___  ______/ /__  ____  / /_
| | /| / / _ \/ / ___/ __ \/ __ `__ \/ _ \   / __/ __ \   / / / / __/ / /_/ /   \__ \/ __/ / / / __  / _ \/ __ \/ __/
| |/ |/ /  __/ / /__/ /_/ / / / / / /  __/  / /_/ /_/ /  / /_/ / /___/ __  /   ___/ / /_/ /_/ / /_/ /  __/ / / / /_  
|__/|__/\___/_/\___/\____/_/ /_/ /_/\___/   \__/\____/   \____/_____/_/ /_/   /____/\__/\__,_/\__,_/\___/_/ /_/\__/  
                                                                                                                     
  
";
                int windowWidth = Console.WindowWidth;

                foreach (var line in title.Split('\n'))
                {
                    int spacesToAdd = (windowWidth - line.Length) / 2;
                    Console.WriteLine(new string(' ', spacesToAdd) + line);
                }

                Console.ResetColor();

                Console.WriteLine("\n");

                // Hiển thị "Tài khoản" căn giữa
                string taiKhoanText = "Tài khoản: ";
                int taiKhoanSpaces = (windowWidth - taiKhoanText.Length) / 2;
                Console.Write(new string(' ', taiKhoanSpaces));
                Console.Write(taiKhoanText);
                string taiKhoan = Console.ReadLine();

                // Hiển thị "Mật khẩu" căn giữa
                string matKhauText = "Mật khẩu: ";
                int matKhauSpaces = (windowWidth - matKhauText.Length) / 2;
                Console.Write(new string(' ', matKhauSpaces));
                Console.Write(matKhauText);
                string matKhau = Console.ReadLine();

                if (taiKhoan == taiKhoanDung && matKhau == matKhauDung)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nĐăng nhập thành công!");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    soLanThu++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nSai tài khoản hoặc mật khẩu. Bạn còn {maxThu - soLanThu} lần thử.");
                    Console.ResetColor();

                    // Hiển thị thông báo ngay lập tức và chờ người dùng nhấn phím
                    Console.WriteLine("\nNhấn phím bất kỳ để thử lại...");
                    Console.ReadKey();
                }
            }
            return false;
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            string title = @"
    ___                      _            ____  _       _      __     ___              _   _ _____ _   _ 
   / _ \ _   _  __ _ _ __   | |   _   _  / ___|(_)_ __ | |__   \ \   / (_) ___ _ __   | | | | ____| | | |
  | | | | | | |/ _` | '_ \  | |  | | | | \___ \| | '_ \| '_ \   \ \ / /| |/ _ \ '_ \  | | | |  _| | |_| |
  | |_| | |_| | (_| | | | | | |__| |_| |  ___) | | | | | | | |   \ V / | |  __/ | | | | |_| | |___|  _  |
     \__\_\\__,_|\__,_|_| |_| |_____\__, | |____/|_|_| |_|_| |_|    \_/  |_|\___|_| |_|  \___/|_____|_| |_|   
                                 |___/                                                                  
";
            int windowWidth = Console.WindowWidth;

            // Thiết lập màu sắc cho tiêu đề
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Tách tiêu đề thành từng dòng và căn giữa
            foreach (var line in title.Split('\n'))
            {
                int spacesToAdd = (windowWidth - line.Length) / 2;  // Tính toán khoảng cách để căn giữa
                Console.WriteLine(new string(' ', spacesToAdd) + line);  // In từng dòng đã được căn giữa
            }

            // In dòng phân cách đầy màn hình
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', windowWidth));  // Dùng ký tự '=' để lấp đầy toàn bộ chiều rộng cửa sổ
            Console.ResetColor();
            Console.WriteLine();

            // Các lựa chọn menu - Căn giữa từng dòng
            string[] menuOptions = new string[]
            {
                "1. Thêm Sinh Viên",
                "2. Xem Thông Tin Sinh Viên",
                "3. Sửa Thông Tin Sinh Viên",
                "4. Xóa Sinh Viên",
                "5. Xuất Danh Sách Học Bổng",
                "6. Xét Loại Tốt Nghiệp",
                "7. Thoát Khỏi Hệ Thống"
            };

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var option in menuOptions)
            {
                int spacesToAdd = (windowWidth - option.Length) / 2;  // Tính toán khoảng cách để căn giữa
                Console.WriteLine(new string(' ', spacesToAdd) + option);  // In từng lựa chọn đã được căn giữa
            }

            // In dòng phân cách ở dưới cùng, lấp đầy toàn bộ chiều rộng cửa sổ
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine();
            Console.WriteLine(new string('=', windowWidth));  // Dùng ký tự '=' để lấp đầy toàn bộ chiều rộng cửa sổ
            Console.ResetColor();
        }


        /// <summary>
        /// Đọc file SinhVien.txt
        /// </summary>
        static void DocFileSinhVien()
        {
            try
            {
                // Mở file SinhVien.txt và đọc dữ liệu
                using (StreamReader sr = new StreamReader("SinhVien.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Tách các trường từ dòng dữ liệu
                        string[] fields = line.Split('|');

                        if (fields.Length == 8)
                        {
                            sinhViens[sinhVienCount, 0] = fields[0].Trim();  // MSSV
                            sinhViens[sinhVienCount, 1] = fields[1].Trim();  // Lớp
                            sinhViens[sinhVienCount, 2] = fields[2].Trim();  // Họ Tên
                            sinhViens[sinhVienCount, 3] = fields[3].Trim();  // Ngày Sinh
                            sinhViens[sinhVienCount, 4] = fields[4].Trim();  // Nơi Sinh
                            sinhViens[sinhVienCount, 5] = fields[5].Trim();  // Giới Tính
                            sinhViens[sinhVienCount, 6] = fields[6].Trim();  // Điểm Trung Bình
                            sinhViens[sinhVienCount, 7] = fields[7].Trim();  // Điểm Rèn Luyện

                            sinhVienCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file SinhVien.txt: " + ex.Message);
            }
        }


        // Kiểm tra xem MSSV đã tồn tại trong file chưa
        static bool MSSVExist(string mssv)
        {
            try
            {
                // Đọc tất cả các dòng từ file
                string[] lines = File.ReadAllLines("SinhVien.txt");

                foreach (string line in lines)
                {
                    string[] data = line.Split('|');
                    if (data.Length > 0 && data[0].Trim() == mssv.Trim()) // Kiểm tra MSSV đã tồn tại trong file
                    {
                        return true; // MSSV đã tồn tại
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Đã có lỗi khi đọc file: {ex.Message}");
            }
            return false; // MSSV không tồn tại
        }

        static void ThemSinhVien()
        {
            if (sinhVienCount >= sinhViens.GetLength(0))
            {
                Console.WriteLine("Danh sách sinh viên đã đầy!");
                return;
            }

            Console.WriteLine("=== Thêm Sinh Viên ===");

            // Nhập MSSV
            string mssv;
            while (true)
            {
                try
                {
                    Console.Write("MSSV (8 số): ");
                    mssv = Console.ReadLine();

                    // Kiểm tra xem MSSV đã tồn tại trong file chưa
                    if (MSSVExist(mssv))
                    {
                        Console.WriteLine("Dữ liệu sinh viên đã tồn tại. Vui lòng nhập MSSV khác.");
                        continue;
                    }

                    if (mssv.Length != 8 || !mssv.All(char.IsDigit))
                    {
                        throw new Exception("MSSV phải có 8 số. Vui lòng nhập lại.");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            sinhViens[sinhVienCount, 0] = mssv;

            // Nhập Lớp
            string lop;
            while (true)
            {
                Console.Write("Lớp (Ví dụ: EEP001): ");
                lop = Console.ReadLine();
                if (lop.Length == 6 && lop.Substring(0, 3).All(char.IsLetter) && lop.Substring(3, 3).All(char.IsDigit))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Lớp phải có định dạng '3 chữ cái + 3 số'. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 1] = lop;

            // Nhập Họ Tên
            string hoTen;
            while (true)
            {
                Console.Write("Họ Tên: ");
                hoTen = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(hoTen))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Họ tên không thể để trống. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 2] = hoTen;

            // Nhập Ngày Sinh
            string ngaySinh;
            while (true)
            {
                Console.Write("Ngày Sinh (DD/MM/YYYY): ");
                ngaySinh = Console.ReadLine();
                if (DateTime.TryParseExact(ngaySinh, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ngày sinh không hợp lệ. Vui lòng nhập lại theo định dạng DD/MM/YYYY.");
                }
            }
            sinhViens[sinhVienCount, 3] = ngaySinh;

            // Nhập Nơi Sinh
            string noiSinh;
            while (true)
            {
                Console.Write("Nơi Sinh: ");
                noiSinh = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(noiSinh))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nơi sinh không thể để trống. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 4] = noiSinh;

            // Nhập Giới Tính
            string gioiTinh;
            while (true)
            {
                Console.Write("Giới Tính (Nam/Nữ): ");
                gioiTinh = Console.ReadLine();
                if (gioiTinh.ToLower() == "nam" || gioiTinh.ToLower() == "nữ")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Giới tính phải là 'Nam' hoặc 'Nữ'. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 5] = gioiTinh;

            // Nhập Điểm Trung Bình
            double dtb;
            while (true)
            {
                Console.Write("Điểm Trung Bình (ĐTB, từ 1 đến 10): ");
                string dtbInput = Console.ReadLine();
                if (double.TryParse(dtbInput, out dtb) && dtb >= 1 && dtb <= 10)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Điểm Trung Bình phải từ 1 đến 10. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 6] = dtb.ToString();

            // Nhập Điểm Rèn Luyện
            int drl;
            while (true)
            {
                Console.Write("Điểm Rèn Luyện (ĐRL, từ 50 đến 100): ");
                string drlInput = Console.ReadLine();
                if (int.TryParse(drlInput, out drl) && drl >= 50 && drl <= 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Điểm Rèn Luyện phải từ 50 đến 100. Vui lòng nhập lại.");
                }
            }
            sinhViens[sinhVienCount, 7] = drl.ToString();

            sinhVienCount++;
            Console.WriteLine("Thêm sinh viên thành công!");

            // Cập nhật vào file SinhVien.txt
            GhiFileSinhVien();
        }


        static void GhiFileSinhVien()
        {
            try
            {
                // Ghi thêm dòng mới vào cuối file
                using (StreamWriter sw = new StreamWriter(fileName, append: true))
                {
                    // Lấy sinh viên vừa thêm cuối cùng
                    string newStudent = $"{sinhViens[sinhVienCount - 1, 0]} | {sinhViens[sinhVienCount - 1, 1]} | {sinhViens[sinhVienCount - 1, 2]} | {sinhViens[sinhVienCount - 1, 3]} | {sinhViens[sinhVienCount - 1, 4]} | {sinhViens[sinhVienCount - 1, 5]} | {sinhViens[sinhVienCount - 1, 6]} | {sinhViens[sinhVienCount - 1, 7]}";

                    // Ghi dòng mới vào file
                    sw.WriteLine(newStudent);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi ghi file SinhVien.txt: " + ex.Message);
            }
        }


        static void TimTTSinhVien()
        {
            string mssv;
            bool index = false;

            while (true)
            {
                // Nhập MSSV. Kiểm tra cấu trúc của MSSV phải gồm 8 chữ số
                Console.Write("Nhập MSSV của sinh viên cần tìm: ");
                mssv = Console.ReadLine();

                if (mssv.Length != 8 || !mssv.All(char.IsDigit))
                {
                    Console.WriteLine("Mã số sinh viên phải bao gồm đúng 8 chữ số.");
                }
                else
                {
                    // Kiểm tra MSSV có tồn tại trong danh sách sinh viên hay không
                    index = MSSVExist(mssv);
                    if (index)
                        break;
                    else
                    {
                        Console.WriteLine("Không tìm thấy sinh viên với MSSV này.");
                        return;
                    }
                }
            }

            // Tìm sinh viên trong file SinhVien.txt và hiển thị thông tin
            try
            {
                // Đọc tất cả dòng từ file SinhVien.txt
                string[] lines = File.ReadAllLines("SinhVien.txt");

                // Duyệt qua các dòng trong file để tìm thông tin sinh viên
                foreach (var line in lines)
                {
                    string[] fields = line.Split('|');  // Tách các trường thông tin trong dòng

                    // Tìm sinh viên có MSSV trùng khớp
                    if (fields[0].Trim() == mssv)
                    {
                        // Lấy điểm trung bình từ file
                        double diemTB = double.Parse(fields[6]);

                        // Quy đổi điểm trung bình từ hệ 10 sang hệ 4
                        double diemHe4 = XetDiemCot4(diemTB);
                        Console.WriteLine($"MSSV: {fields[0]}");
                        Console.WriteLine($"Lớp: {fields[1]}");
                        Console.WriteLine($"Họ tên: {fields[2]}");
                        Console.WriteLine($"Ngày sinh: {fields[3]}");
                        Console.WriteLine($"Nơi sinh: {fields[4]}");
                        Console.WriteLine($"Giới tính: {fields[5]}");
                        Console.WriteLine($"Điểm trung bình: {fields[6]}");
                        Console.WriteLine($"Điểm rèn luyện: {fields[7]}");
                        Console.WriteLine($"Điểm hệ 4: {diemHe4}");
                        return;  // Nếu tìm thấy sinh viên thì kết thúc hàm
                    }
                }

                // Nếu không tìm thấy MSSV trong file
                Console.WriteLine("Không tìm thấy sinh viên với MSSV này trong file.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi đọc file
                Console.WriteLine("Lỗi khi đọc dữ liệu từ file: " + ex.Message);
            }
        }

        static void SuaSinhVien()
        {
            int hangSV;
            string mssv;
            bool index = true;
            Console.WriteLine("=== Sửa Sinh Viên ===");
            while (true)
            {
                try
                {
                    //Nhập MSSV. Kiếm tra cấu trúc của MSSV phải gồm 8 chữ số
                    Console.Write("Nhập MSSV của sinh viên cần sửa: ");
                    mssv = Console.ReadLine();
                    long StudentID = long.Parse(mssv);
                    if (mssv.Length != 8)
                    {
                        Console.WriteLine("Mã số sinh viên phải bao gồm đúng 8 chữ số.");
                    }
                    else
                    {
                        index = MSSVExist(mssv);
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Mã số sinh viên phải là số nguyên");
                }

            }
            //Tìm sinh viên dựa trên mã số, nếu không có thì thông báo
            if (index == false)
            {
                Console.WriteLine("Không tìm thấy sinh viên với MSSV này.");
                return;
            }
            else
            {
                hangSV = TimSinhVien(mssv) + 1;
                Console.WriteLine("=== Sửa Thông Tin Sinh Viên ===");
                while (true)
                {
                    try
                    {
                        //Nhập từng thông tin để sửa, nếu thông tin nào có định dạng đặc biệt thì cần kiểm tra
                        Console.Write("Lớp: ");
                        string lop = Console.ReadLine();

                        // Kiểm tra định dạng bằng Regular Expression
                        if (!System.Text.RegularExpressions.Regex.IsMatch(lop, @"^[a-zA-Z]{3}\d{3}$"))
                        {
                            Console.WriteLine("Mã lớp không đúng định dạng. Vui lòng nhập lại.");
                        }
                        else
                        {
                            //Nếu nhập đúng thì cập nhật thông tin
                            sinhViens[hangSV, 1] = lop;
                            break;
                        }

                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }

                }

                //Nhập họ tên sinh viên
                string hoTen;
                while (true)
                {
                    Console.Write("Họ Tên: ");
                    hoTen = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(hoTen))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Họ tên không thể để trống. Vui lòng nhập lại.");
                    }
                }
                sinhViens[hangSV, 2] = hoTen;

                //Nhập ngày sinh
                Console.Write("Ngày Sinh: ");
                DateTime NgaySinh;
                while (true)
                {
                    try
                    {
                        //Kiểm tra định dạng ngày sinh
                        string date = Console.ReadLine();
                        NgaySinh = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sinhViens[hangSV, 3] = date;
                        break; // Thoát khỏi vòng lặp nếu hợp lệ
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ngày sinh không đúng định dạng. Vui lòng nhập lại (dd/MM/yyyy): ");
                    }
                }
                // Nhập Nơi Sinh
                string noiSinh;
                while (true)
                {
                    Console.Write("Nơi Sinh: ");
                    noiSinh = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(noiSinh))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Nơi sinh không thể để trống. Vui lòng nhập lại.");
                    }
                }
                sinhViens[hangSV, 4] = noiSinh;

                // Nhập Giới Tính
                string gioiTinh;
                while (true)
                {
                    Console.Write("Giới Tính (Nam/Nữ): ");
                    gioiTinh = Console.ReadLine();
                    if (gioiTinh.ToLower() == "nam" || gioiTinh.ToLower() == "nữ")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Giới tính phải là 'Nam' hoặc 'Nữ'. Vui lòng nhập lại.");
                    }
                }
                sinhViens[hangSV, 5] = gioiTinh;
                //Nhập điểm trung bình
                Console.Write("Điểm Trung Bình (DTB): ");
                while (true)
                {
                    try
                    {
                        //Kiểm tra điểm trung bình phải là số dương và nhỏ hơn 10
                        string dtb = Console.ReadLine();
                        double Dtb = double.Parse(dtb);
                        if (Dtb > 10 | Dtb < 0)
                        {
                            Console.WriteLine("Điểm trung bình không hợp lệ.");
                        }
                        else
                        {
                            //Cập nhật điểm nếu đúng cú pháp
                            sinhViens[hangSV, 6] = dtb;
                            break;
                        }
                    }
                    catch (FormatException)
                    {
                        //Báo sai định dạng điểm
                        Console.WriteLine("Điểm trung bình không đúng định dạng. Phải là số");
                    }
                }

                //Nhập điểm trung bình
                Console.Write("Điểm Rèn Luyện (DRL): ");
                while (true)
                {
                    try
                    {
                        //Kiểm tra điểm trung bình phải là số dương và nhỏ hơn 100
                        string drl = Console.ReadLine();
                        int Drl = int.Parse(drl);
                        if (Drl > 100 | Drl < 0)
                        {
                            //Báo nếu nhập sai
                            Console.WriteLine("Điểm trung bình không hợp lệ.");
                        }
                        else
                        {
                            //Cập nhật điểm
                            sinhViens[hangSV, 7] = drl;
                            break;
                        }
                    }
                    catch (FormatException)
                    {
                        //Nếu điểm nhập vào không phải là số nguyên, chương trình sẽ thông báo
                        Console.WriteLine("Điểm trung bình không đúng định dạng. Phải là số");
                    }
                }
            }
            try
            {
                // Đọc tất cả dòng từ file SinhVien.txt
                string[] lines = File.ReadAllLines("SinhVien.txt");

                // Duyệt qua các dòng trong file để cập nhật thông tin sinh viên
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split('|');  // Tách các trường thông tin trong dòng

                    // Tìm sinh viên có MSSV trùng khớp
                    if (fields[0].Trim() == mssv)
                    {
                        // Cập nhật lại thông tin sinh viên vào dòng tương ứng
                        lines[i] = string.Join(" | ",
                            fields[0],  // MSSV giữ nguyên
                            sinhViens[hangSV, 1],  // Lớp
                            sinhViens[hangSV, 2],  // Họ Tên
                            sinhViens[hangSV, 3],  // Ngày Sinh
                            sinhViens[hangSV, 4],  // Nơi Sinh
                            sinhViens[hangSV, 5],  // Giới Tính
                            sinhViens[hangSV, 6],  // Điểm Trung Bình
                            sinhViens[hangSV, 7]   // Điểm Rèn Luyện
                        );
                        break;  // Thoát khỏi vòng lặp sau khi đã cập nhật thông tin
                    }
                }

                // Ghi lại toàn bộ dữ liệu vào file SinhVien.txt
                File.WriteAllLines("SinhVien.txt", lines);
                Console.WriteLine("Sửa thông tin thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật thông tin vào file: " + ex.Message);
            }
        }

        static void XoaSinhVien()
        {
            string mssv;
            bool sinhVienTimThay = false;

            while (true)
            {
                try
                {
                    // Nhập MSSV cần tìm
                    Console.Write("Nhập MSSV của sinh viên cần xóa: ");
                    mssv = Console.ReadLine();

                    if (mssv.Length != 8 || !mssv.All(char.IsDigit))
                    {
                        Console.WriteLine("Mã số sinh viên phải bao gồm đúng 8 chữ số.");
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Mã số sinh viên phải là số nguyên");
                }
            }

            // Đọc tất cả dòng từ file SinhVien.txt
            string[] lines = File.ReadAllLines("SinhVien.txt");
            List<string> updatedLines = new List<string>();

            // Duyệt qua từng dòng trong file để tìm và xóa sinh viên
            foreach (var line in lines)
            {
                string[] fields = line.Split('|');  // Tách các trường thông tin trong dòng

                if (fields[0].Trim() == mssv)
                {
                    // Nếu tìm thấy MSSV trùng khớp, không thêm dòng này vào danh sách updatedLines
                    sinhVienTimThay = true;
                }
                else
                {
                    updatedLines.Add(line);  // Thêm dòng không bị xóa vào danh sách
                }
            }

            // Kiểm tra xem sinh viên có được tìm thấy không
            if (!sinhVienTimThay)
            {
                Console.WriteLine("Không tìm thấy sinh viên với MSSV này.");
                return;
            }

            // Ghi lại các dòng còn lại vào file
            File.WriteAllLines("SinhVien.txt", updatedLines);

            Console.WriteLine("Xóa sinh viên thành công!");
        }

        static void XuatDanhSachHocBong()
        {
            Console.Clear();
            Console.WriteLine(">>> Danh Sách Sinh Viên Được Xét Học Bổng <<<");

            DocFileSinhVien(); // Đọc file để lấy dữ liệu
            SortSinhVienHocBong(); // Sắp xếp sinh viên theo DTB và DRL

            // Mảng lưu sinh viên theo từng khóa
            string[,] khoa49 = new string[100, 8];
            string[,] khoa48 = new string[100, 8];
            string[,] khoa47 = new string[100, 8];

            int index49 = 0, index48 = 0, index47 = 0;

            // Phân loại sinh viên theo khóa
            for (int i = 0; i < sinhVienCount; i++)
            {
                string mssv = sinhViens[i, 0];
                if (mssv.StartsWith("3123")) // Khoa 49
                {
                    CopyRow(sinhViens, khoa49, i, index49++);
                }
                else if (mssv.StartsWith("3122")) // Khoa 48
                {
                    CopyRow(sinhViens, khoa48, i, index48++);
                }
                else if (mssv.StartsWith("3121")) // Khoa 47
                {
                    CopyRow(sinhViens, khoa47, i, index47++);
                }
            }

            // Random số suất học bổng
            Random rand = new Random();
            int slots49 = rand.Next(8, 10);
            int slots48 = rand.Next(8, 10);
            int slots47 = rand.Next(8, 10);

            // Hiển thị kết quả và ghi file
            using (StreamWriter writer = new StreamWriter("ketQuaHocBong.txt"))
            {
                GhiVaHienThiBang(writer, "Danh sách học bổng Khóa 49", khoa49, slots49);
                GhiVaHienThiBang(writer, "Danh sách học bổng Khóa 48", khoa48, slots48);
                GhiVaHienThiBang(writer, "Danh sách học bổng Khóa 47", khoa47, slots47);
            }

            Console.WriteLine("\nKết quả đã xuất vào file 'ketQuaHocBong.txt'.");
            Console.ReadKey();
        }




        static void SortSinhVienHocBong()
        {
            for (int i = 1; i < sinhVienCount - 1; i++)
            {
                for (int j = i + 1; j < sinhVienCount; j++)
                {
                    // Lấy giá trị từ mảng và loại bỏ khoảng trắng dư thừa
                    string dtbString1 = sinhViens[i, 6]?.Trim().Replace(",", ".");
                    string dtbString2 = sinhViens[j, 6]?.Trim().Replace(",", ".");
                    string drlString1 = sinhViens[i, 7]?.Trim().Replace(",", ".");
                    string drlString2 = sinhViens[j, 7]?.Trim().Replace(",", ".");

                    // Kiểm tra dữ liệu không rỗng hoặc null
                    if (string.IsNullOrWhiteSpace(dtbString1) || string.IsNullOrWhiteSpace(dtbString2) ||
                        string.IsNullOrWhiteSpace(drlString1) || string.IsNullOrWhiteSpace(drlString2))
                    {
                        Console.WriteLine($"Lỗi: Dữ liệu bị thiếu tại dòng {i + 1} hoặc {j + 1}");
                        continue; // Bỏ qua dòng lỗi
                    }

                    // Chuyển đổi điểm trung bình (DTB)
                    if (!double.TryParse(dtbString1, out double dtb1))
                    {
                        Console.WriteLine($"Lỗi: Không thể chuyển đổi DTB '{dtbString1}' tại dòng {i + 1}");
                        continue;
                    }
                    if (!double.TryParse(dtbString2, out double dtb2))
                    {
                        Console.WriteLine($"Lỗi: Không thể chuyển đổi DTB '{dtbString2}' tại dòng {j + 1}");
                        continue;
                    }

                    // Chuyển đổi điểm rèn luyện (DRL)
                    if (!int.TryParse(drlString1, out int drl1))
                    {
                        Console.WriteLine($"Lỗi: Không thể chuyển đổi DRL '{drlString1}' tại dòng {i + 1}");
                        continue;
                    }
                    if (!int.TryParse(drlString2, out int drl2))
                    {
                        Console.WriteLine($"Lỗi: Không thể chuyển đổi DRL '{drlString2}' tại dòng {j + 1}");
                        continue;
                    }

                    // So sánh DTB và DRL, nếu cần thì hoán đổi
                    if (dtb1 < dtb2 || (dtb1 == dtb2 && drl1 < drl2))
                    {
                        // Hoán đổi toàn bộ dòng trong mảng
                        for (int k = 0; k < 8; k++)
                        {
                            string temp = sinhViens[i, k];
                            sinhViens[i, k] = sinhViens[j, k];
                            sinhViens[j, k] = temp;
                        }
                    }
                }
            }
        }





        static void CopyRow(string[,] source, string[,] dest, int sourceIndex, int destIndex)
        {
            for (int i = 0; i < 8; i++)
            {
                dest[destIndex, i] = source[sourceIndex, i];
            }
        }


        static void GhiVaHienThiBang(StreamWriter writer, string title, string[,] khoa, int slots)
        {
            Console.WriteLine(title);
            writer.WriteLine(title);

            // Cập nhật tiêu đề bảng
            Console.WriteLine("MSSV\t\tLớp\tHọ Tên\t\tDTB\tDRL\tLoại Học Bổng\tĐiểm Hệ 4");
            writer.WriteLine("MSSV    | Lớp  |    Họ Tên    | DTB | DRL |Học Bổng|Điểm Hệ 4");

            for (int i = 0; i < slots; i++)
            {
                if (string.IsNullOrEmpty(khoa[i, 0])) break; // Dừng nếu không còn sinh viên

                // Lấy dữ liệu từ mảng
                string mssv = khoa[i, 0];
                string hoTen = khoa[i, 1];
                string lop = khoa[i, 2];
                double dtb = double.Parse(khoa[i, 6].Replace(",", "."));
                int drl = int.Parse(khoa[i, 7]);

                // Gọi hàm xác định loại học bổng và điểm hệ 4
                string loaiHocBong = XetLoaiHocBong(dtb, drl);
                double diemHe4 = XetDiemCot4(dtb);

                // Xuất ra console
                if (loaiHocBong == "Xuất sắc")
                {
                    Console.WriteLine($"{mssv}\t{hoTen}\t{lop}\t{dtb:F2}\t{drl}\t{loaiHocBong}\t{diemHe4:F2}");
                }
                else
                {
                    Console.WriteLine($"{mssv}\t{hoTen}\t{lop}\t{dtb:F2}\t{drl}\t{loaiHocBong}\t\t{diemHe4:F2}");
                }

                // Ghi ra file
                writer.WriteLine($"{mssv}|{hoTen}|{lop}|{dtb:F2}|{drl}|{loaiHocBong}|{diemHe4:F2}");
            }
        }






        // Hàm xác định loại học bổng
        static string XetLoaiHocBong(double dtb, int drl)
        {
            if (dtb >= 9.0 && drl >= 90)
                return "Xuất sắc";
            else if (dtb >= 8.0 && drl >= 80)
                return "Giỏi";
            else if (dtb >= 7.0 && drl >= 70)
                return "Khá";
            else
                return "Không đủ điều kiện";
        }

        // Hàm cấp học bổng cho mỗi khóa
        static string[,] CapHocBong(string[,] sinhViens, int scholarshipSlots)
        {
            string[,] ketQua = new string[scholarshipSlots, 8];
            int index = 0;

            for (int i = 0; i < sinhViens.GetLength(0); i++)
            {
                if (index >= scholarshipSlots || sinhViens[i, 0] == null) break;

                double dtb = double.Parse(sinhViens[i, 3]);
                int drl = int.Parse(sinhViens[i, 4]);
                string loaiHocBong = XetLoaiHocBong(dtb, drl);

                if (loaiHocBong != "Không đủ điều kiện")
                {
                    for (int j = 0; j < 8; j++)
                        ketQua[index, j] = sinhViens[i, j];
                    ketQua[index, 7] = loaiHocBong; // Cập nhật loại học bổng
                    index++;
                }
            }

            return ketQua;
        }


        static int TimSinhVien(string mssv)
        {
            mssv = mssv.Trim();  // Loại bỏ khoảng trắng trước và sau

            // Đọc tất cả dòng từ file SinhVien.txt
            string[] lines = File.ReadAllLines("SinhVien.txt");

            // Duyệt qua các dòng trong file để tìm MSSV
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('|');  // Tách các trường trong dòng (giả sử bạn sử dụng dấu "|" để phân cách các trường)

                // Kiểm tra nếu MSSV trong file trùng khớp với mssv
                if (fields[0].Trim() == mssv)  // Loại bỏ khoảng trắng trong MSSV từ file
                {
                    return i; // Trả về chỉ số sinh viên trong file
                }
            }

            return -1; // Không tìm thấy sinh viên với MSSV này
        }
        static void XetLoaiTotNghiep()
        {
            string mssv;
            bool index = false;

            while (true)
            {
                // Nhập MSSV. Kiểm tra cấu trúc của MSSV phải gồm 8 chữ số
                Console.Write("Nhập MSSV của sinh viên cần tìm: ");
                mssv = Console.ReadLine();

                if (mssv.Length != 8 || !mssv.All(char.IsDigit))
                {
                    Console.WriteLine("Mã số sinh viên phải bao gồm đúng 8 chữ số.");
                }
                else
                {
                    // Kiểm tra MSSV có tồn tại trong danh sách sinh viên hay không
                    index = MSSVExist(mssv);
                    if (index)
                        break;
                    else
                    {
                        Console.WriteLine("Không tìm thấy sinh viên với MSSV này.");
                        return;  // Nếu không tìm thấy trong danh sách, thoát hàm
                    }
                }
            }

            // Tìm sinh viên trong file SinhVien.txt và hiển thị thông tin
            try
            {
                // Đọc tất cả dòng từ file SinhVien.txt
                string[] lines = File.ReadAllLines("SinhVien.txt");

                // Duyệt qua các dòng trong file để tìm thông tin sinh viên
                bool found = false;  // Biến kiểm tra xem đã tìm thấy sinh viên chưa

                foreach (var line in lines)
                {
                    string[] fields = line.Split('|');  // Tách các trường thông tin trong dòng

                    // Tìm sinh viên có MSSV trùng khớp
                    if (fields[0].Trim() == mssv)
                    {
                        found = true;  // Sinh viên được tìm thấy
                        Console.WriteLine("Thông tin sinh viên:");
                        Console.WriteLine($"MSSV: {fields[0]}");
                        Console.WriteLine($"Họ tên: {fields[1]}");
                        Console.WriteLine($"Lớp: {fields[2]}");
                        // Lấy điểm trung bình từ file
                        double diemTB = double.Parse(fields[6]);

                        string LoaiTotNghiep;
                        //Đổi điểm trung bình của sinh viên sang hệ 4
                        double Diem = XetDiemCot4(diemTB);
                        //Đánh giá hạng tốt nghiệp dựa trên điểm trung bình
                        if (Diem == 4)
                        {
                            LoaiTotNghiep = "Xuất sắc";
                            Console.WriteLine($"Điểm trung bình: {Diem}");
                            Console.WriteLine($"Với điểm trung bình hiện tại, loại tốt nghiệp tạm thời của bạn là {LoaiTotNghiep}! Hãy cố gắng học tập nhé!");
                        }
                        else if (Diem == 3.5)
                        {
                            LoaiTotNghiep = "Giỏi";
                            Console.WriteLine($"Điểm trung bình: {Diem}");
                            Console.WriteLine($"Với điểm trung bình hiện tại, loại tốt nghiệp tạm thời của bạn là {LoaiTotNghiep}! Hãy cố gắng học tập nhé!");
                        }
                        else if (Diem == 3 || Diem == 2.5)
                        {
                            LoaiTotNghiep = "Khá";
                            Console.WriteLine($"Điểm trung bình: {Diem}");
                            Console.WriteLine($"Với điểm trung bình hiện tại, loại tốt nghiệp tạm thời của bạn là {LoaiTotNghiep}! Hãy cố gắng học tập nhé!");
                        }
                        else if (Diem == 2)
                        {
                            LoaiTotNghiep = "Trung bình";
                            Console.WriteLine($"Điểm trung bình: {Diem}");
                            Console.WriteLine($"Với điểm trung bình hiện tại, loại tốt nghiệp tạm thời của bạn là {LoaiTotNghiep}! Hãy cố gắng học tập nhé!");
                        }
                        else
                        {
                            //Nếu sinh viên chưa đủ điều kiện thì báo
                            Console.WriteLine("Bạn chưa đủ điều kiện tốt nghiệp.");
                        }
                        if (!found)
                        {
                            Console.WriteLine("Không tìm thấy sinh viên với MSSV này trong file.");
                            // Nếu không tìm thấy MSSV trong file

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi đọc file
                Console.WriteLine("Lỗi khi đọc dữ liệu từ file: " + ex.Message);
            }
        }
        static double XetDiemCot4(double b)
        {
            //Quy đổi điểm hệ 10 sang điểm hệ 4
            if (b >= 8.5)
            {
                b = 4;
            }
            else if (b >= 8 && b <= 8.4)
            {
                b = 3.5;
            }
            else if (b >= 7 && b <= 7.9)
            {
                b = 3;
            }
            else if (b >= 6.5 && b <= 6.9)
            {
                b = 2.5;
            }
            else if (b >= 5.5 && b <= 6.4)
            {
                b = 2;
            }
            else if (b >= 5.4 && b <= 5)
            {
                b = 1.5;
            }
            else if (b >= 4 && b <= 4.9)
            {
                b = 1;
            }
            else
            {
                b = 0;
            }
            return b;
        }
    }
}

