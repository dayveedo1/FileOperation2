using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload
{
    public static class ServiceExtension
    {
        public static void ConfigureFileProvider (this IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "file_uploads")
                ));
        }
    }
}
