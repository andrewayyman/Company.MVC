namespace Company.Route.PL.Helpers
{
    // this class is to upload files to the server
    // Steps :: 
    // 1 - Get Located Folder Path 
    // 2 - Get File Name And Make It Unique
    // 3 - Get File PAth [ Folder Path + FileName ]
    // 4 - Save File As Streams
    // 5 - Return File Name

    public class DocumentSettings
    {


        public static string UploadFile( IFormFile file, string foldername )
        {
            //--------------------------------  1. Get Located Folder Path  --------------------------------
            /// E:\Computer Science\Route_C42\07 Asp.NetCore MVC\Company.Route Solution\Company.Route.PL\wwwroot\files\images\
            /// Not True We need it Dynamic 
            ///string folderPath = Directory.GetCurrentDirectory() + "wwwroot\\files" + foldername; // WE can enhance and use combine 
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", foldername);


            //--------------------------------- 2 - Get File Name And Make It Unique  ----------------------------------
            string filename = $"{Guid.NewGuid()}_{file.FileName}";


            //--------------------------------- 3 - Get File PAth [ Folder Path + FileName ] ----------------------------------
            string filePath = Path.Combine(folderPath, filename);


            //--------------------------------- 4 - Save File As Streams  ----------------------------------
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);


            return filename;


        }

        public static void DeleteFile( string fileName, string folderName )
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);

            if ( File.Exists(filePath) )
            {
                File.Delete(filePath);
            }

        }




    }
}
