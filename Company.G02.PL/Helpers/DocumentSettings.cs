namespace Company.G02.PL.Helpers
{
    public class DocumentSettings
    {
        // 1. Upload
        // ImageName
        public static string UploadFile(IFormFile file,string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "C:\\Users\\habib\\source\\repos\\Company.G02\\Company.G02.PL\\wwwroot\\Files\\folderName+";

            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\Files" , folderName);

            // 2.Get File Name and Make it Unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath,FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        // 2. Delete

        public static void DeleteFile(string fileName , string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files",folderName , fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
