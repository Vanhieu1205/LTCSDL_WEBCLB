using ClbTinhoc.Web.Data;
using ClbTinhoc.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClbTinhoc.Web.Controllers
{
    public class KhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KhoaHocController> _logger;

        public KhoaHocController(ApplicationDbContext context, ILogger<KhoaHocController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: KhoaHoc
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhoaHoc.ToListAsync());
        }

        // GET: KhoaHoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhoaHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenKhoaHoc,MoTa,NgayBatDau,NgayKetThuc")] KhoaHoc khoaHoc, IFormFile image)
        {
            try
            {
                // Loại bỏ validation cho trường image trong ModelState
                ModelState.Remove("image");

                // Kiểm tra file ảnh
                if (image == null || image.Length == 0)
                {
                    ModelState.AddModelError("image", "Hình ảnh là bắt buộc.");
                    _logger.LogWarning("No image file uploaded.");
                    return View(khoaHoc);
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("image", "Chỉ chấp nhận file .jpg, .jpeg, .png.");
                    _logger.LogWarning($"Invalid file extension: {extension}");
                    return View(khoaHoc);
                }

                // Kiểm tra kích thước file (tối đa 5MB)
                if (image.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("image", "Kích thước file không được vượt quá 5MB.");
                    _logger.LogWarning($"File size too large: {image.Length} bytes");
                    return View(khoaHoc);
                }

                // Đảm bảo thư mục images tồn tại
                string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Tạo tên file dựa trên TenKhoaHoc
                string sanitizedFileName = SanitizeFileName(khoaHoc.TenKhoaHoc);
                string fileName = sanitizedFileName + extension;
                string filePath = Path.Combine(imagesFolder, fileName);

                // Kiểm tra trùng lặp và thêm hậu tố nếu cần
                int counter = 1;
                while (System.IO.File.Exists(filePath))
                {
                    fileName = $"{sanitizedFileName}_{counter}{extension}";
                    filePath = Path.Combine(imagesFolder, fileName);
                    counter++;
                }

                // Lưu file ảnh
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Gán tên file vào model
                khoaHoc.image = fileName;

                // Kiểm tra ModelState
                if (!ModelState.IsValid)
                {
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning($"ModelState Error - {entry.Key}: {error.ErrorMessage}");
                        }
                    }
                    return View(khoaHoc);
                }

                // Lưu vào cơ sở dữ liệu
                _logger.LogInformation("Saving KhoaHoc to database...");
                _context.Add(khoaHoc);
                await _context.SaveChangesAsync();

                _logger.LogInformation("KhoaHoc saved successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating KhoaHoc: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm khóa học. Vui lòng thử lại.");
                return View(khoaHoc);
            }
        }

        // Hàm làm sạch tên file
        private string SanitizeFileName(string fileName)
        {
            // Loại bỏ ký tự không hợp lệ và thay khoảng trắng bằng dấu gạch dưới
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            string sanitized = Regex.Replace(fileName, invalidRegStr, "_");

            // Thay khoảng trắng bằng dấu gạch dưới
            sanitized = sanitized.Replace(" ", "_");

            // Giới hạn độ dài tên file (ví dụ: 100 ký tự)
            if (sanitized.Length > 100)
            {
                sanitized = sanitized.Substring(0, 100);
            }

            return sanitized;
        }

        // Các action khác giữ nguyên
        // GET: KhoaHoc/Details/5
        // GET: KhoaHoc/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var khoaHoc = await _context.KhoaHoc
                .Include(k => k.KhoaHoc_SinhVien)
                .ThenInclude(ks => ks.SinhVien)
                .FirstOrDefaultAsync(m => m.MaKhoaHoc == id);

            if (khoaHoc == null)
            {
                _logger.LogWarning($"KhoaHoc with ID {id} not found.");
                return NotFound();
            }

            // Lấy danh sách sinh viên chưa tham gia khóa học
            var enrolledStudentIds = khoaHoc.KhoaHoc_SinhVien
                .Select(ks => ks.MaSinhVien)
                .ToList();
            var availableStudents = await _context.SinhVien
                .Where(s => !enrolledStudentIds.Contains(s.MaSinhVien))
                .OrderBy(s => s.HoTen)
                .Select(s => new { s.MaSinhVien, s.HoTen })
                .ToListAsync();

            _logger.LogInformation($"Found {availableStudents.Count} available students for KhoaHoc ID {id}.");
            ViewBag.AvailableStudents = availableStudents;

            return View(khoaHoc);
        }

        // POST: KhoaHoc/RegisterCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCourse(int MaKhoaHoc, string MaSinhVien)
        {
            try
            {
                _logger.LogInformation($"RegisterCourse called with MaKhoaHoc: {MaKhoaHoc}, MaSinhVien: {MaSinhVien}");

                if (string.IsNullOrEmpty(MaSinhVien))
                {
                    _logger.LogWarning("MaSinhVien is empty.");
                    TempData["Error"] = "Vui lòng chọn sinh viên.";
                    return RedirectToAction("Details", new { id = MaKhoaHoc });
                }

                // Kiểm tra khóa học
                var khoaHoc = await _context.KhoaHoc.FindAsync(MaKhoaHoc);
                if (khoaHoc == null)
                {
                    _logger.LogWarning($"KhoaHoc with ID {MaKhoaHoc} not found.");
                    TempData["Error"] = "Khóa học không tồn tại.";
                    return RedirectToAction("Details", new { id = MaKhoaHoc });
                }

                // Kiểm tra sinh viên
                var sinhVien = await _context.SinhVien.FindAsync(MaSinhVien);
                if (sinhVien == null)
                {
                    _logger.LogWarning($"SinhVien with MaSinhVien {MaSinhVien} not found.");
                    TempData["Error"] = "Sinh viên không tồn tại.";
                    return RedirectToAction("Details", new { id = MaKhoaHoc });
                }

                // Kiểm tra đã tham gia
                var existing = await _context.KhoaHoc_SinhVien
                    .AnyAsync(ks => ks.MaKhoaHoc == MaKhoaHoc && ks.MaSinhVien == MaSinhVien);

                if (existing)
                {
                    _logger.LogWarning($"SinhVien {MaSinhVien} already enrolled in KhoaHoc {MaKhoaHoc}.");
                    TempData["Error"] = "Sinh viên đã tham gia khóa học này.";
                    return RedirectToAction("Details", new { id = MaKhoaHoc });
                }

                var khoaHocSinhVien = new KhoaHoc_SinhVien
                {
                    MaKhoaHoc = MaKhoaHoc,
                    MaSinhVien = MaSinhVien
                };

                _logger.LogInformation($"Adding KhoaHoc_SinhVien: MaKhoaHoc={MaKhoaHoc}, MaSinhVien={MaSinhVien}");
                _context.KhoaHoc_SinhVien.Add(khoaHocSinhVien);
                await _context.SaveChangesAsync();

                _logger.LogInformation("KhoaHoc_SinhVien saved successfully.");
                TempData["Success"] = "Đăng ký khóa học thành công.";
                return RedirectToAction("Details", new { id = MaKhoaHoc });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering course: {ex.Message}\nStackTrace: {ex.StackTrace}");
                TempData["Error"] = "Có lỗi xảy ra khi đăng ký khóa học. Vui lòng thử lại.";
                return RedirectToAction("Details", new { id = MaKhoaHoc });
            }
        }

        // GET: KhoaHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHoc.FindAsync(id);
            if (khoaHoc == null)
            {
                return NotFound();
            }
            return View(khoaHoc);
        }

        // POST: KhoaHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhoaHoc,TenKhoaHoc,MoTa,NgayBatDau,NgayKetThuc,image")] KhoaHoc khoaHoc, IFormFile ImageFile)
        {
            if (id != khoaHoc.MaKhoaHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý file hình ảnh mới nếu có
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Kiểm tra định dạng file
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ImageFile", "Chỉ chấp nhận file .jpg, .jpeg, .png.");
                            return View(khoaHoc);
                        }

                        // Kiểm tra kích thước file (tối đa 5MB)
                        if (ImageFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageFile", "Kích thước file không được vượt quá 5MB.");
                            return View(khoaHoc);
                        }

                        // Đảm bảo thư mục images tồn tại
                        string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        // Tạo tên file dựa trên TenKhoaHoc
                        string sanitizedFileName = SanitizeFileName(khoaHoc.TenKhoaHoc);
                        string fileName = sanitizedFileName + extension;
                        string filePath = Path.Combine(imagesFolder, fileName);

                        // Kiểm tra trùng lặp và thêm hậu tố nếu cần
                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            fileName = $"{sanitizedFileName}_{counter}{extension}";
                            filePath = Path.Combine(imagesFolder, fileName);
                            counter++;
                        }

                        // Lưu file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // Cập nhật tên file trong database
                        khoaHoc.image = fileName;
                    }

                    _context.Update(khoaHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoaHocExists(khoaHoc.MaKhoaHoc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khoaHoc);
        }

        // GET: KhoaHoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaHoc = await _context.KhoaHoc
                .FirstOrDefaultAsync(m => m.MaKhoaHoc == id);
            if (khoaHoc == null)
            {
                return NotFound();
            }

            return View(khoaHoc);
        }

        // POST: KhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khoaHoc = await _context.KhoaHoc.FindAsync(id);
            if (khoaHoc != null)
            {
                _context.KhoaHoc.Remove(khoaHoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoaHocExists(int id)
        {
            return _context.KhoaHoc.Any(e => e.MaKhoaHoc == id);
        }
    }
}