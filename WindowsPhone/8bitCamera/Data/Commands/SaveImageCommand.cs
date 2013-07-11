using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public class SaveImageCommand
    {
        public void Save(Stream stream, string fileName)
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.FileExists(fileName))
                    {
                        fileName = fileName + "(1)";
                    }

                    using (var targetStream = file.OpenFile(fileName, FileMode.Create))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytesRead;

                        while ((bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {
                stream.Close();
            }
        }
    }
}