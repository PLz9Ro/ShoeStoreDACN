using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ShoeStore.Hellper
{
    public static class Unilities
    {
        public static int PAGE_SIZE = 20;
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists) Directory.CreateDirectory(path);
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split("");
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);

                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GetRandomKey(int length = 5)
        {
            //chuỗi mẫu (pattern)
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            if (url == null)
            {
                return string.Empty;
            }

            var Url = url.ToLower();
            Url = Regex.Replace(Url, @"[áààãàãàäãỗăăăăăă]", "a");
            Url = Regex.Replace(Url, @"[éèçèêêêêêêê]", "e");
            Url = Regex.Replace(Url, @"[οοφοοοοοοοοσάờợcỡ]", "o");
            Url = Regex.Replace(Url, @"[íìiii]", "i");
            Url = Regex.Replace(Url, @"[ýýỵіỹ]", "y");
            Url = Regex.Replace(Url, @"[uuuuuuuuuuữ]", "u");
            Url = Regex.Replace(Url, @"[d]", "d");
            Url = Regex.Replace(Url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            Url = Regex.Replace(Url.Trim(), @"\s+", "-");
            Url = Regex.Replace(Url, @"\s", "-");

            while (true)
            {
                if (Url.IndexOf("--") != -1)
                {
                    Url = Url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return Url;
        }
        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory); CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        
        }

    }

}
