using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

//Referencias:
//http://www.andrealveslima.com.br/blog/index.php/2017/04/12/utilizando-api-google-drive-no-c-e-vb-net/
//https://stackoverflow.com/questions/60925278/google-drive-api-v3-c-sharp-uploadprogress-trigger-during-file-upload

namespace Google_Drive_API
{
    internal class Program
    {
        private static string[] Scopes = { DriveService.Scope.Drive };
        private static string ApplicationName = "StandBy";

        private static void Main(string[] args)
        {
            UserCredential credential;

            credential = GetCredentials();

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            CriarPastaDentroDePastas("teste1", service);

            //UploadBasicImage("C:\\Tchurendio\\StandBy System.rar", service);
            //ListarArquivosPaginados(service, 10);

            //string pageToken = null;
            //ListFiles(service);
            //UploadArquivoDentroPasta(service);
            //do
            //{
            //    ListFiles(service, ref pageToken);
            //} while (pageToken != null);

            //DeleteFileFolder("1gFgQsrIFTxe222222", service);
            Console.WriteLine("Done");
            Console.Read();
        }

        private static void ListarArquivosPaginados(Google.Apis.Drive.v3.DriveService servico, int arquivosPorPagina)
        {
            var request = servico.Files.List();
            request.Fields = "nextPageToken, files(id, name)";
            //request.Q = "trashed=false";
            // Default 100, máximo 1000.
            request.PageSize = arquivosPorPagina;
            var resultado = request.Execute();
            var arquivos = resultado.Files;

            while (arquivos != null && arquivos.Any())
            {
                foreach (var arquivo in arquivos)
                {
                    Console.WriteLine(arquivo.Name);
                }

                if (resultado.NextPageToken != null)
                {
                    Console.WriteLine("Digite ENTER para ir para a próxima página");
                    Console.ReadLine();
                    request.PageToken = resultado.NextPageToken;
                    resultado = request.Execute();
                    arquivos = resultado.Files;
                }
                else
                {
                    arquivos = null;
                }
            }
        }

        private static async Task UploadArquivoDentroPasta(DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "Test hello uploaded.txt",
                Parents = new List<string>() { "1iS3wbkFHhgzPtodb-8u5231pfwrLIV8l" }
            };

            string uploadedFileId;
            // Create a new file on Google Drive
            using (var fsSource = new FileStream("Test hello.txt", FileMode.Open, FileAccess.Read))
            {
                // Create a new file, with metadata and stream.
                var request = service.Files.Create(fileMetadata, fsSource, "text/plain");
                request.Fields = "*";
                var results = await request.UploadAsync(CancellationToken.None);

                if (results.Status == UploadStatus.Failed)
                {
                    Console.WriteLine($"Error uploading file: {results.Exception.Message}");
                }

                // the file id of the new file we created
                uploadedFileId = request.ResponseBody?.Id;
            }
        }

        //Teste1/teste2/teste3
        private static void CriarPastaDentroDePastas(string folderName, DriveService service, string folderId = "")
        {
            Google.Apis.Drive.v3.Data.File fileMetadata;
            string[] pastas = folderName.Split('/');
            string idFolder = string.Empty;
            for (int i = 0; i < pastas.Length; i++)
            {
                if (i == 0)
                {
                    fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = pastas[i],
                        MimeType = "application/vnd.google-apps.folder"
                    };
                }
                else
                {
                    fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = pastas[i],
                        Parents = new List<string>() { idFolder },
                        MimeType = "application/vnd.google-apps.folder"
                    };
                }

                Permission perm = new Permission();
                perm.Type = "user";
                //perm.PendingOwner = true;
                perm.EmailAddress = "psdislife@gmail.com";
                perm.Role = "writer";

                var request = service.Files.Create(fileMetadata);
                request.Fields = "*";

                try
                {
                    PermissionsResource.CreateRequest cc = service.Permissions.Create(perm, request.Execute().Id);
                    //cc.TransferOwnership = true;
                    cc.Execute();
                    //service.Permissions.Create(perm, request.Execute().Id).Execute(); //Creating Permission after folder creation.
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }

                ////Original, funcionando.
                //var request = service.Files.Create(fileMetadata);
                //request.Fields = "*";
                //var file = request.Execute();
                //idFolder = file.Id;
                //Console.WriteLine($"Folder name: {file.Name} ||| Folder ID: " + file.Id);
            }
        }

        private static void DeleteFileFolder(string id, DriveService service)
        {
            var request = service.Files.Delete(id);

            request.Execute();
        }

        private static void UploadBasicImage(string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Parents = new List<string>() { "1iS3wbkFHhgzPtodb-8u5231pfwrLIV8l" }, //ID da Pasta que criei
            };
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "application/rar";
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "application/rar");
                request.Fields = "id, name";
                request.ChunkSize = 262144;
                request.ProgressChanged += progress => Console.WriteLine($"IDownloadProgress: {progress.Status} ({(progress.BytesSent / 1024f) / 1024f} / {(stream.Length / 1024f) / 1024f}mb)"); ;
                request.Upload();
            }

            var file = request.ResponseBody;

            Console.WriteLine("File ID: " + file.Id);
        }

        private static void Request_ProgressChanged1(IUploadProgress obj)
        {
            double pc = (obj.BytesSent * 100);
            //File.AppendAllText(@"d:\date.txt", DateTime.Now.ToString("hh:mm:ss.fff") + obj.Status.ToString() + " - " + obj.BytesSent + "____" + pc.ToString("0.00") + Environment.NewLine);
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff") + obj.Status.ToString() + " - " + obj.BytesSent + "____" + pc + "<br>");
            // close the stream

            //  HttpContext.Current.Response.Write(string.Format(obj.Status + " " + obj.BytesSent));
            //   HttpContext.Current.Response.Write(obj.BytesSent.ToString());
        }

        private static void ListFiles(DriveService service)
        {
            var request = service.Files.List();
            //request.Q = "mimeType='application/vnd.google-apps.folder'";
            //request.Q = "name contains '.rar'";
            //request.Q = "name = 'hello'";
            var results = request.Execute();

            var list = results.Files.ToList().OrderBy(x => x.Name);

            foreach (var VARIABLE in list)
            {
                Console.WriteLine($"{VARIABLE.Name} - {VARIABLE.MimeType} - {VARIABLE.Id}");
            }

            #region Old

            //// Define parameters of request.
            //FilesResource.ListRequest listRequest = service.Files.List();
            //listRequest.PageSize = 10;
            ////listRequest.Fields = "nextPageToken, files(id, name)";
            //listRequest.Fields = "nextPageToken, files(name)";
            //listRequest.PageToken = pageToken;
            //listRequest.Q = "mimeType='image/jpeg'";

            //// List files.
            //var request = listRequest.Execute();

            //if (request.Files != null && request.Files.Count > 0)
            //{
            //    foreach (var file in request.Files)
            //    {
            //        Console.WriteLine("{0}", file.Name);
            //    }

            //    pageToken = request.NextPageToken;

            //    if (request.NextPageToken != null)
            //    {
            //        Console.WriteLine("Press any key to conti...");
            //        Console.ReadLine();
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No files found.");
            //}

            #endregion Old
        }

        private static UserCredential GetCredentials()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }
    }
}