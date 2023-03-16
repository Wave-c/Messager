using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using Messager.Models.Requests;
using System.Text.Json;
using Messager.Models;
using Messager.Models.Entitys;

namespace Messager.Helpers
{
    public static class BitmapHelper
    {
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        public static Bitmap FromBitmapImagetoBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public static async Task<BitmapImage> GetUserImageAsync(User currentUser)
        {
            using (var request = (GetUserImageRequest)await RequestsFactory.CreateRequestAsync<GetUserImageRequest, User>(currentUser))
            {
                Response response = await request.SendRequestAsync();
                if (response.ResponseCode == 200)
                {
                    return JsonSerializer.Deserialize<BitmapImage>(response.ResponseObj);
                }
                if (response.ResponseCode == 404)
                {
                    return BitmapToBitmapImage(new Bitmap("..//..//..//Resource/NoPhotoUser.png"));
                }
                throw new Exception();
            }
        }
    }
}
