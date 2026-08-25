using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace Minsa.VPA.Common
{
    public class Common
    {
        public static string GetAppPath(Context context)
        {
            // File dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)
             File dir = new File(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath
            //File dir = new File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)
                 + File.Separator
                );
            if (!dir.Exists())
                dir.Mkdir();
            return dir.Path + File.Separator;
        }
        public static async Task WriteFileToStorageAsync(Context context, string filename)
        {
            AssetManager asset = context.Assets;
            if (new File(GetFilePath(context, filename)).Exists())
                return;
            try
            {
                var input = asset.Open(filename);
                var output = new FileOutputStream(GetFilePath(context, filename));
                byte[] buffer = new byte[8 * 1024];
                int length;
                while ((length = input.Read(buffer, 0, buffer.Length)) > 0)
                    output.Write(buffer, 0, length);
            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException ex)
            {
                ex.PrintStackTrace();
            }
        }

        public static string GetFilePath(Context context, string filename)
        {
            return context.FilesDir + File.Separator + filename;
        }
    }
}