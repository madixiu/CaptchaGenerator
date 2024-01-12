using System;
// using System.Drawing;
// using System.Drawing.Drawing2D;
// using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using Captcha.Net;

namespace CaptchaTest.Controllers
{
    public class CaptchaController : Controller
    {
        private const string CaptchaSessionKey = "CaptchaCode";
        
        public IActionResult Index()
        {
            // Generate a new captcha code and save to session
            string captchaCode = GenerateCaptchaCode();
            var captchaGenerator = new CaptchaGenerator();
			// var captchaCode = captchaGenerator.GenerateCaptchaCode();
            // HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);
            
            // Return captcha image to view
            // byte[] captchaBytes = GenerateCaptchaImage(captchaCode);
            var GenerateCaptchaImage = captchaGenerator.GenerateCaptchaImage(200, 100, captchaCode);
            var img = File(GenerateCaptchaImage.CaptchaByteData, "image/png");
            return img;
        }

        private string GenerateCaptchaCode()
        {
            // Generate a random 5 character captcha code
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        // private byte[] GenerateCaptchaImage(string code)
        // {
        //     // Generate a captcha image with the code
        //     using var bitmap = new Bitmap(130, 50);
        //     using var gfx = Graphics.FromImage(bitmap);
            
        //     gfx.DrawString(code, new Font("Arial", 26), 
        //         Brushes.Black, new PointF(2, 2));
            
        //     // Apply random noise
        //     var random = new Random();
        //     for (var i = 0; i < 100; i++)
        //     {
        //         int x = random.Next(bitmap.Width);
        //         int y = random.Next(bitmap.Height);
        //         bitmap.SetPixel(x, y, Color.FromArgb(
        //             random.Next(255), random.Next(255), random.Next(255)));
        //     }

        //     using var ms = new MemoryStream();
        //     bitmap.Save(ms, ImageFormat.Png);
        //     return ms.ToArray();
        // }

        // public IActionResult Validate(string captchaCode)
        // {
        //     if (captchaCode == HttpContext.Session.GetString(CaptchaSessionKey))
        //     {
        //         return Json(true); 
        //     }
        //     return Json("Invalid captcha");
        // }
    }
}
