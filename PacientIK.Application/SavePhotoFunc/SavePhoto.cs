using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PacientIK.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.SavePhotoFunc
{
    public class SavePhoto
    {
        private readonly Cloudinary dnr;
        private readonly SavePhotoId savePhoto;
        public SavePhoto(Cloudinary dr, SavePhotoId save)
        {
            dnr = dr;
            savePhoto = save;
        }
        
        public async Task<string> AddPhoto(IFormFile photo)
        {
            using var stream = photo.OpenReadStream();
            string id = Guid.NewGuid().ToString("N");
            savePhoto.SavePhoto = id;
            var paramss = new ImageUploadParams
            {
                File = new FileDescription(photo.FileName, stream),
                Folder = "photos",
                PublicId = id,
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = false
            };
            
            var result = await dnr.UploadAsync(paramss);
            
            if (result.Error != null)
            {
                throw new Exception($"Cloudinary error: {result.Error.Message}");
                
            }

            return result.SecureUrl.ToString();
        }

        public async Task<string> ChangePhoto(IFormFile? photo, string code,CancellationToken token)
        {
            if(photo == null)
            {
                return code;
            }
            using var stream = photo.OpenReadStream();

            var paramss = new ImageUploadParams
            {
                File = new FileDescription(photo.FileName, stream),
                Folder = "photos",
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = true,
                PublicId = code,
            };

            var result = await dnr.UploadAsync(paramss, token);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }
               
            return result.SecureUrl.ToString();
        }

        public async Task DeletePhoto(string name)
        {
            var deleteparams = new DeletionParams(name)
            {
                ResourceType = ResourceType.Image,
                PublicId = name,
            };

            var result = await dnr.DestroyAsync(deleteparams);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }
        }
    }
}
