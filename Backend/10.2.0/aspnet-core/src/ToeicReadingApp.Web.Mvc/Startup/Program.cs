using System;
using System.IO;
using System.Linq;
using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ToeicReadingApp.Exams.WordParser;

namespace ToeicReadingApp.Web.Startup;

public class Program
{
    public static void Main(string[] args)
    {
        EnsureSampleFilesGenerated();

        if (args.Length > 0 && args.Contains("--generate-samples"))
        {
            Console.WriteLine("Sample files generated successfully!");
            return;
        }

        CreateHostBuilder(args).Build().Run();
    }

    private static void EnsureSampleFilesGenerated()
    {
        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            string rootDir = null;
            while (dir != null)
            {
                if (dir.Name.Equals("Midterm_ToeicApp", StringComparison.OrdinalIgnoreCase))
                {
                    rootDir = dir.FullName;
                    break;
                }
                dir = dir.Parent;
            }

            if (string.IsNullOrEmpty(rootDir))
            {
                rootDir = @"c:\Users\ASUS\Downloads\Midterm_ToeicApp";
            }

            if (Directory.Exists(rootDir))
            {
                var validFilePath = Path.Combine(rootDir, "DeThi_TOEIC_Reading_Test05_Tagged.docx");
                var invalidFilePath = Path.Combine(rootDir, "DeThi_TOEIC_Loi_DinhDang.docx");

                var validBytes = WordExamSampleGenerator.GenerateValidSampleDocx();
                File.WriteAllBytes(validFilePath, validBytes);

                var invalidBytes = WordExamSampleGenerator.GenerateInvalidSampleDocx();
                File.WriteAllBytes(invalidFilePath, invalidBytes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Warning: Could not auto-generate sample files: " + ex.Message);
        }
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseCastleWindsor(IocManager.Instance.IocContainer);
}
